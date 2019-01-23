using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MemoryMadness;

public class StateTutorial3 : StateMachineBehaviour 
{

	[SerializeField] private float delay = 1.0f;

	[SerializeField] private GameObject memoryPhaseOuterContainer;
	[SerializeField] private GameObject memoryPhaseContainer;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		//Get the memory Phase Container
		memoryPhaseOuterContainer = GameObject.Find( "MemoryPhaseOuterContainer" );
		memoryPhaseContainer = memoryPhaseOuterContainer.transform.Find( "MemoryPhaseContainer" ).gameObject;

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
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
