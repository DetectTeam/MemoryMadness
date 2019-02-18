﻿using System.Collections;
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
	private Animator anim;
	private MoveTo moveDialog;
	private bool isSectionComplete = false;
	public bool IsSectionComplete { get{ return isSectionComplete; } set{ isSectionComplete = value; } }

	
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		anim = animator;

		if( DialogueManager.Instance )
		{
			DialogueManager.Instance.IsSectionComplete = false;
			DialogueManager.Instance.Reset();
		}

		if( TutorialManager.Instance )
		{
			TutorialManager.Instance.CurrentLevel = 3;
			
			//Reset Error and Correct Counts
			TutorialManager.Instance.ResetCounts();
		}
		
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

		if( TutorialManager.Instance )
		{
			TutorialManager.Instance.DisableBackground();
			TutorialManager.Instance.BuildTutorialLevel( 2 );
		}
	
		//Display MemoryPhase Screen
		memoryPhaseOuterContainer.transform.SetSiblingIndex( 1 );
		memoryPhaseContainer.SetActive( true );
		yield return new WaitForSeconds( delay );

		//Play Dialog
		if( dialogueBox )
			dialogueBox.transform.SetSiblingIndex( 3 );

		if( moveDialog )	
			moveDialog.Move( 0.3f, new Vector3( 0f, -1000f, 0f ) , new Vector3( 0f, -350f, 0  ) );

		//Start Second Dialogue
		if( DialogueManager.Instance )
		{
			DialogueManager.Instance.StartDialogue( dialogues[0] );
			DialogueManager.Instance.IsSectionComplete = false;
		}

		//Wait for user to exhaust dialogue
		while( DialogueManager.Instance && !DialogueManager.Instance.IsSectionComplete )
		{
			yield return null;
		}

		//Hide dialog box
		if( moveDialog )
			moveDialog.Move( 0.3f, new Vector3( 0f, -350f, 0f ) , new Vector3( 0f, -1000f, 0  ) );

		yield return new WaitForSeconds( 1.0f );

		//Activate the Game Container
		if( gameOuterContainer )
			gameOuterContainer.transform.SetSiblingIndex( 1 );
		
		if( gameContainer )
			gameContainer.SetActive( true );

		if( DialogueManager.Instance )
			DialogueManager.Instance.IsSectionComplete = false;

		while( DialogueManager.Instance && !DialogueManager.Instance.IsSectionComplete )
		{
			yield return null;
		}

		yield return new WaitForSeconds( 2.0f );
		Debug.Log( "Finished Level" );

		if( anim )
			anim.SetInteger( "Tutorial" , 4 );
	}

	private void SectionOver()
	{
		isSectionComplete = true;
	}

}