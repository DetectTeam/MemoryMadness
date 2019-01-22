using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTutorial2 : StateMachineBehaviour 
{
	[SerializeField] private float delay = 1.0f;
	[SerializeField] private Animator anim;
	[SerializeField] private GameObject symbolContainer;
	[SerializeField] private List<Dialogue> dialogues;
	[SerializeField] private GameObject dialogueBox;
	[SerializeField] private GameObject gameOuterContainer;
	[SerializeField] private GameObject gameContainer;
	[SerializeField] private GameObject memoryPhaseOuterContainer;
	[SerializeField] private GameObject memoryPhaseContainer;
	[SerializeField] private GameObject memoryPhaseSymbols;
	[SerializeField] private GameObject[] symbolHighlights;

	private MoveTo moveDialog;

	[SerializeField] private int buttonCount;

	private bool isSectionComplete = false;
	public bool IsSectionComplete { get{ return isSectionComplete; } set{ isSectionComplete = value; } }


	public void OnEnable()
	{
		Messenger.AddListener( "SectionOver" , SectionOver );
	}

	public void OnDisable()
	{
		Messenger.RemoveListener( "SectionOver" , SectionOver );
	}

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		anim = animator;
		isSectionComplete = false;

		//Get the memory Phase Container
		memoryPhaseOuterContainer = GameObject.Find( "MemoryPhaseOuterContainer" );
		memoryPhaseContainer = memoryPhaseOuterContainer.transform.Find( "MemoryPhaseContainer" ).gameObject;

		//Swtich Memory Symbols
		var previousMemorySymbols = memoryPhaseContainer.transform.Find( "MemoryPhaseSymbols1" ).gameObject;
		var currentMemorySymbols = memoryPhaseContainer.transform.Find( "MemoryPhaseSymbols2" ).gameObject;

		if( !previousMemorySymbols  || !currentMemorySymbols)
		{
			Debug.Log( "Memory Symbols Not Found" );
			return;
		}

		previousMemorySymbols.SetActive( false );
		currentMemorySymbols.SetActive( true );

		gameOuterContainer = (GameObject)GameObject.Find( "GameOuterContainer" );
		gameContainer = gameOuterContainer.transform.Find( "GameContainer" ).gameObject;
		
		var randomGameContainer = gameContainer.transform.Find( "RandomLevelContainer" ).gameObject;

		if( !gameContainer )
			Debug.Log( "GameContainer Not Found" );

		//Switch Game Symbols
		var previousGameSymbols = randomGameContainer.transform.Find( "SymbolContainer1" ).gameObject;
		var currentGameSymbols = randomGameContainer.transform.Find( "SymbolContainer2" ).gameObject;

		if( !previousGameSymbols  || !currentGameSymbols)
		{
			Debug.Log( "Memory Symbols Not Found" );
			return;
		}

		previousGameSymbols.SetActive( false );
		currentGameSymbols.SetActive( true );

		dialogueBox = GameObject.Find( "DialogueBox" );
		
		if( !dialogueBox )
		{
			Debug.Log( "DialogueBox Not Found" );
			return;
		}

		moveDialog = dialogueBox.GetComponent<MoveTo>(); 
		
		
		if( !memoryPhaseContainer )
		{
			Debug.Log( "Memory phase container not found" );
			return;
		}

		CoRoutineSlave.Instance.ExecCoroutine( Sequence() );

	}

	//Tutorial 2	
	private IEnumerator Sequence()
	{
		
		Debug.Log( "Starting Tutorial 2" );
	
		//Display MemoryPhase Screen
		memoryPhaseOuterContainer.transform.SetSiblingIndex( 1 );
		memoryPhaseContainer.SetActive( true );
		yield return new WaitForSeconds( delay );

		
		//Play Dialog
		dialogueBox.transform.SetSiblingIndex( 2 );
		moveDialog.Move( 0.3f, new Vector3( 0f, -1000f, 0f ) , new Vector3( 0f, -350f, 0  ) );

		yield return new WaitForSeconds( 1f );

		//Display first Dialogue. 
		DialogueManager.Instance.StartDialogue( dialogues[0] );

		//Wait for user to exhaust dialogue
		while( !isSectionComplete )
			yield return null;


		//Hide dialog box
		moveDialog.Move( 0.3f, new Vector3( 0f, -350f, 0f ) , new Vector3( 0f, -1000f, 0  ) );

		yield return new WaitForSeconds( 1.0f );


		//Activate the Game Container
		gameOuterContainer.transform.SetSiblingIndex( 1 );
		gameContainer.SetActive( true );

		yield return new WaitForSeconds( 1.0f );

		//Show dialog Box
		moveDialog.Move( 0.3f, new Vector3( 0f, -1000f, 0f ) , new Vector3( 0f, -350f, 0  ) );

		yield return new WaitForSeconds( 1.0f );

		//Display first Dialogue. 
		DialogueManager.Instance.StartDialogue( dialogues[1] );
		
		//Wait for user to exhaust dialogue
		while( !isSectionComplete )
			yield return null;


		Debug.Log( "Done !!!!" );

		//anim.SetInteger( "Tutorial" , 3 );
		//Debug.Log( "Im done here..." );
	}

	private void SectionOver()
	{
		isSectionComplete = true;
	}
}
