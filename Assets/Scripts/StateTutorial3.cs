using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MemoryMadness;

public class StateTutorial3 : StateMachineBehaviour 
{

	[SerializeField] private float delay = 1.0f;
	[SerializeField] private GameObject memoryPhaseOuterContainer;
	[SerializeField] private GameObject memoryPhaseContainer;
	[SerializeField] private GameObject gameOuterContainer;
	[SerializeField] private GameObject gameContainer;
	[SerializeField] private List<Dialogue> dialogues;
	[SerializeField] private GameObject dialogueBox;
	private MoveTo moveDialog;
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
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		//Get the memory Phase Container
		memoryPhaseOuterContainer = GameObject.Find( "MemoryPhaseOuterContainer" );
		memoryPhaseContainer = memoryPhaseOuterContainer.transform.Find( "MemoryPhaseContainer" ).gameObject;

		gameOuterContainer = (GameObject)GameObject.Find( "GameOuterContainer" );
		gameContainer = gameOuterContainer.transform.Find( "GameContainer" ).gameObject;


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


	private IEnumerator Sequence()
	{
		Debug.Log( "Starting Tutorial 3" );

		TutorialManager.Instance.BuildTutorialLevel( 0 );
	
		//Display MemoryPhase Screen
		memoryPhaseOuterContainer.transform.SetSiblingIndex( 1 );
		memoryPhaseContainer.SetActive( true );
		yield return new WaitForSeconds( delay );

		//Play Dialog
		dialogueBox.transform.SetSiblingIndex( 2 );
		moveDialog.Move( 0.3f, new Vector3( 0f, -1000f, 0f ) , new Vector3( 0f, -350f, 0  ) );

		//Start Second Dialogue
		DialogueManager.Instance.StartDialogue( dialogues[0] );

		isSectionComplete = false;

		//Wait for user to exhaust dialogue
		while( !isSectionComplete )
		{
			yield return null;
		}

		Debug.Log( "Got this far ...3" );
		
		//Hide dialog box
		moveDialog.Move( 0.3f, new Vector3( 0f, -350f, 0f ) , new Vector3( 0f, -1000f, 0  ) );

		yield return new WaitForSeconds( 1.0f );

		
		//Build Level

		TutorialManager.Instance.BuildTutorialLevel( 2 );
		
		//Activate the Game Container
		gameOuterContainer.transform.SetSiblingIndex( 1 );
		gameContainer.SetActive( true );
	}

	private void SectionOver()
	{
		isSectionComplete = true;
	}
}
