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

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		anim = animator;
		DialogueManager.Instance.IsSectionComplete = false;

		DialogueManager.Instance.Reset();

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
		if( memoryPhaseOuterContainer )
			memoryPhaseOuterContainer.transform.SetSiblingIndex( 1 );
		
		if( memoryPhaseContainer )
			memoryPhaseContainer.SetActive( true );
		
		yield return new WaitForSeconds( delay );

		//Play Dialog
		if( dialogueBox )
			dialogueBox.transform.SetSiblingIndex( 3 );
		
		if( moveDialog )
			moveDialog.Move( 0.3f, new Vector3( 0f, -1000f, 0f ) , new Vector3( 0f, -350f, 0  ) );

		yield return new WaitForSeconds( 1f );

		//Display first Dialogue. 
		if( DialogueManager.Instance )
			DialogueManager.Instance.StartDialogue( dialogues[0] );

		//Wait for user to exhaust dialogue
		while( DialogueManager.Instance && !DialogueManager.Instance.IsSectionComplete )
			yield return null;

		//Hide dialog box
		if( moveDialog )
			moveDialog.Move( 0.3f, new Vector3( 0f, -350f, 0f ) , new Vector3( 0f, -1000f, 0  ) );

		yield return new WaitForSeconds( 1.0f );

		//Activate the Game Container
		if( gameOuterContainer )
			gameOuterContainer.transform.SetSiblingIndex( 1 );
		
		if( gameContainer )
			gameContainer.SetActive( true );

		yield return new WaitForSeconds( 1.0f );

		//Show dialog Box
		if( moveDialog )
			moveDialog.Move( 0.3f, new Vector3( 0f, -1000f, 0f ) , new Vector3( 0f, -350f, 0  ) );

		yield return new WaitForSeconds( 1.0f );

		//Display first Dialogue. 
		if( DialogueManager.Instance )
		{
			DialogueManager.Instance.StartDialogue( dialogues[1] );
			DialogueManager.Instance.IsSectionComplete = false;
		}
		
		//Wait for user to exhaust dialogue
		while( DialogueManager.Instance && !DialogueManager.Instance.IsSectionComplete )
			yield return null;
		

		//Show dialog Box
		if( moveDialog )
			moveDialog.Move( 0.3f, new Vector3( 0f, -350f, 0f ) , new Vector3( 0f, -1000f, 0  ) );

		yield return new WaitForSeconds( 1.0f );

	
		if( anim )
			anim.SetInteger( "Tutorial" , 3 );
		
		Debug.Log( "State Tut 2 Im done here..." );
	}

	private void SectionOver()
	{
		isSectionComplete = true;
	}
}
