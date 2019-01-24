using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MemoryMadness;

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

		TutorialManager.Instance.CurrentLevel = 2;

		//Reset Error and Correct Counts
		TutorialManager.Instance.ResetCounts();
		
		//Hide the previous Game Symbols
		TutorialManager.Instance.DisableMemorySymbols( 0 );
		TutorialManager.Instance.EnableMemorySymbols( 1 );

		TutorialManager.Instance.DisableGameSymbols( 0 );
		TutorialManager.Instance.EnableGameSymbols( 1 );

		//Get the memory Phase Container
		memoryPhaseOuterContainer = GameObject.Find( "MemoryPhaseOuterContainer" );
		memoryPhaseContainer = memoryPhaseOuterContainer.transform.Find( "MemoryPhaseContainer" ).gameObject;

		gameOuterContainer = (GameObject)GameObject.Find( "GameOuterContainer" );
		gameContainer = gameOuterContainer.transform.Find( "GameContainer" ).gameObject;
		
		var randomGameContainer = gameContainer.transform.Find( "RandomLevelContainer" ).gameObject;

		if( !gameContainer )
		{
			Debug.Log( "GameContainer Not Found" );
			return;
		}


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
		TutorialManager.Instance.DisableBackground();
	
		//Display MemoryPhase Screen
		memoryPhaseOuterContainer.transform.SetSiblingIndex( 1 );
		memoryPhaseContainer.SetActive( true );
		yield return new WaitForSeconds( delay );

		
		//Play Dialog
		dialogueBox.transform.SetSiblingIndex( 3 );
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
		
		isSectionComplete = false;
		
		//Wait for user to exhaust dialogue
		while( !isSectionComplete )
			yield return null;

		//Show dialog Box
		moveDialog.Move( 0.3f, new Vector3( 0f, -350f, 0f ) , new Vector3( 0f, -1000f, 0  ) );

		yield return new WaitForSeconds( 1.0f );

		Debug.Log( "Done !!!!" );

		anim.SetInteger( "Tutorial" , 3 );
		Debug.Log( "State Tut 2 Im done here..." );
	}

	private void SectionOver()
	{
		isSectionComplete = true;
	}
}
