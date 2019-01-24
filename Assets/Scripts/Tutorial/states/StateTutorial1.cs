using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MemoryMadness;

public class StateTutorial1 : StateMachineBehaviour 
{
	[SerializeField] private float delay = 3.0f;
	[SerializeField] private Animator anim;
	[SerializeField] private GameObject symbolContainer;
	[SerializeField] private List<Dialogue> dialogues;
	[SerializeField] private GameObject dialogueBox;
	
	[SerializeField] private GameObject gameOuterContainer;
	[SerializeField] private GameObject gameContainer;

	[SerializeField] private GameObject memorySymbolContainer;

	[SerializeField] private GameObject[] symbolHighlights;

	[SerializeField] private int buttonCount;

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

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		DialogueManager.Instance.Reset();
		TutorialManager.Instance.CurrentLevel = 1;
		//Reset Error and Correct Counts
		TutorialManager.Instance.ResetCounts();
		
		anim = animator;

		dialogueBox = GameObject.Find( "DialogueBox" );
		
		if( !dialogueBox )
		{
			Debug.Log( "DialogueBox Not Found" );
			return;
		}

		moveDialog = dialogueBox.GetComponent<MoveTo>();

		if( !moveDialog )
		{
			Debug.Log( "Dialog Script MoveTo Not Found" );
			return;
		}
	
		gameOuterContainer = (GameObject)GameObject.Find( "GameOuterContainer" );
		gameContainer = gameOuterContainer.transform.Find( "GameContainer" ).gameObject;

		if( !gameContainer )
			Debug.Log( "GameContainer Not Found" );
	
		CoRoutineSlave.Instance.ExecCoroutine( Sequence() );
	}
	
	private void SectionOver()
	{
		isSectionComplete = true;
	}

	//Tutorial 1	
	private IEnumerator Sequence()
	{
		//Memory Phase
		Debug.Log( "Tutorial 1...Memory Phase" );
		
		yield return new WaitForSeconds( 1.5f );
		
		//Display first Dialogue. 
		DialogueManager.Instance.StartDialogue( dialogues[0] );

		//Wait for user to exhaust dialogue
		while( !isSectionComplete )
			yield return null;
	
		isSectionComplete = false;
		Debug.Log( "Transitioning to Game Phase" );
	
		//Hide Dialogue Box
		dialogueBox.transform.SetSiblingIndex( 3 );
		
		moveDialog.Move( 0.3f , new Vector3( 0f, -350f, 0f ) , new Vector3( 0f, -1000f, 0f ) );
		
		yield return new WaitForSeconds( 1.5f );

		//Activate the Game Container
		gameOuterContainer.transform.SetSiblingIndex( 1 );
		gameContainer.SetActive( true );

		yield return new WaitForSeconds( 1f );

		//Display DialogueBox on Game Screen
		moveDialog.Move( 0.3f, new Vector3( 0f, -1000f, 0f ) , new Vector3( 0f, -350f, 0f ) );
		
		//Start Second Dialogue
		DialogueManager.Instance.StartDialogue( dialogues[1] );

		//Wait for user to exhaust dialogue
		while( !isSectionComplete )
			yield return null;

		//Hide Dialogue Box
		moveDialog.Move( 0.3f, new Vector3( 0f, -350f, 0f ) , new Vector3( 0f, -1000f, 0f ) );

		//Clear Dialogue Box
		
		yield return new WaitForSeconds( 0.6f );

		Debug.Log( "On to Tutorial 2" );

		anim.SetInteger( "Tutorial" , 2 );
	}

	private void MoveDialogueBox( Vector3 startPosition, Vector3 endPosition )
	{
		var moveTo = dialogueBox.GetComponent<MoveTo>();

		if( !moveTo )
		{
			Debug.Log( "Move To not found..." );
			return;
		}

		moveTo.Delay = 0.3f;
		moveTo.StartPosition = startPosition;
		moveTo.Target = endPosition;
		moveTo.Move();
	}
}
