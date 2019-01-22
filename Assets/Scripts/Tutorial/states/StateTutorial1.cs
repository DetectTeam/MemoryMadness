using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTutorial1 : StateMachineBehaviour 
{
	[SerializeField] private float delay = 3.0f;
	[SerializeField] private Animator anim;
	[SerializeField] private GameObject symbolContainer;
	[SerializeField] private List<Dialogue> dialogues;
	[SerializeField] private GameObject dialogueBox;
	[SerializeField] private GameObject gameContainer;

	[SerializeField] private GameObject[] symbolHighlights;

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

		dialogueBox = GameObject.Find( "DialogueBox" );
		
		if( !dialogueBox )
			Debug.Log( "DialogueBox Not Found" );

		gameContainer = (GameObject)GameObject.Find( "GameOuterContainer" ).transform.Find( "GameContainer" ).gameObject;

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
		MoveDialogueBox( new Vector3( 0f, -350f, 0f ) , new Vector3( 0f, -1000f, 0f ) );
		
		yield return new WaitForSeconds( 1.5f );

		//Activate the Game Container
		gameContainer.SetActive( true );

		yield return new WaitForSeconds( 1f );

		//Display DialogueBox on Game Screen
		MoveDialogueBox( new Vector3( 0f, -1000f, 0f ) , new Vector3( 0f, -350f, 0f ) );
		
		//Start Second Dialogue
		DialogueManager.Instance.StartDialogue( dialogues[1] );

		//Wait for user to exhaust dialogue
		while( !isSectionComplete )
			yield return null;


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
