﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTutorial2 : StateMachineBehaviour 
{
	[SerializeField] private float delay = 1.0f;
	[SerializeField] private Animator anim;
	[SerializeField] private GameObject symbolContainer;
	[SerializeField] private List<Dialogue> dialogues;
	[SerializeField] private GameObject dialogueBox;
	[SerializeField] private GameObject gameContainer;

	[SerializeField] private GameObject memoryPhaseOuterContainer;
	[SerializeField] private GameObject memoryPhaseContainer;

	[SerializeField] private GameObject[] symbolHighlights;

	private MoveTo moveDialog;

	[SerializeField] private int buttonCount;

	private bool isSectionComplete = false;
	public bool IsSectionComplete { get{ return isSectionComplete; } set{ isSectionComplete = value; } }

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		anim = animator;

			//Get the memory Phase Container
		memoryPhaseOuterContainer = GameObject.Find( "MemoryPhaseOuterContainer" );
		memoryPhaseContainer = memoryPhaseOuterContainer.transform.Find( "MemoryPhaseContainer" ).gameObject;

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

		dialogueBox.transform.SetSiblingIndex( 2 );
		moveDialog.Move( 0.3f, new Vector3( 0f, -1000f, 0f ) , new Vector3( 0f, -350f, 0  ) );

		yield return new WaitForSeconds( 1f );

		//Display first Dialogue. 
		DialogueManager.Instance.StartDialogue( dialogues[0] );

		

		//anim.SetInteger( "Tutorial" , 3 );
		//Debug.Log( "Im done here..." );
	}
}
