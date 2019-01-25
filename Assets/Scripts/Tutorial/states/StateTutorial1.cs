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
	[SerializeField] private IEnumerator currentCoRoutine;

	private MoveTo moveDialog;

	private bool isSectionComplete = false;
	public bool IsSectionComplete { get{ return isSectionComplete; } set{ isSectionComplete = value; } }

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
	
		currentCoRoutine = CoRoutineSlave.Instance.ExecCoroutine( Sequence() );
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
		if( DialogueManager.Instance )
			DialogueManager.Instance.StartDialogue( dialogues[0] );

		//Wait for user to exhaust dialogue
		while( DialogueManager.Instance && !DialogueManager.Instance.IsSectionComplete  )
		{
			yield return null;
			
			if( DialogueManager.Instance == null )
				break;
		}
	
		
		if( DialogueManager.Instance )
			DialogueManager.Instance.IsSectionComplete = false;
		
		Debug.Log( "Transitioning to Game Phase" );
	
		//Hide Dialogue Box

		if( dialogueBox )
			dialogueBox.transform.SetSiblingIndex( 3 );
		
		
		if( moveDialog )
			moveDialog.Move( 0.3f , new Vector3( 0f, -350f, 0f ) , new Vector3( 0f, -1000f, 0f ) );
		
		yield return new WaitForSeconds( 1.5f );

		//Activate the Game Container
		if( gameOuterContainer )
			gameOuterContainer.transform.SetSiblingIndex( 1 );
		
		if( gameContainer )
			gameContainer.SetActive( true );

		yield return new WaitForSeconds( 1f );

		//Display DialogueBox on Game Screen
		if( moveDialog )
			moveDialog.Move( 0.3f, new Vector3( 0f, -1000f, 0f ) , new Vector3( 0f, -350f, 0f ) );
		
		//Start Second Dialogue
		if( DialogueManager.Instance )
		 DialogueManager.Instance.StartDialogue( dialogues[1] );

		//Wait for user to exhaust dialogue
		while( DialogueManager.Instance && !DialogueManager.Instance.IsSectionComplete )
		{
			yield return null;
			if( DialogueManager.Instance == null )
				break;
			
		}

		//Hide Dialogue Box
		if( moveDialog )
			moveDialog.Move( 0.3f, new Vector3( 0f, -350f, 0f ) , new Vector3( 0f, -1000f, 0f ) );

		//Clear Dialogue Box
		
		yield return new WaitForSeconds( 0.6f );

		Debug.Log( "On to Tutorial 2" );

		if( anim )
			anim.SetInteger( "Tutorial" , 2 );
		else
			Debug.Log( "Anim does not exist...." );
	}

	

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Debug.Log( "ON STATE EXIT CALLLLLLLEEEEEDDDDDDDDDD" );
    }
}
