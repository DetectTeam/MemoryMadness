using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStartTutorial1 : StateMachineBehaviour 
{

	[SerializeField] private float delay = 3.0f;
	[SerializeField] private Animator anim;

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		anim = animator;
		
		IEnumerator coRoutine = Tutorial1();
		
		CoRoutineSlave.Instance.ExecCoroutine( coRoutine );

		
	}

	private IEnumerator Tutorial1(  )
	{
		yield return new WaitForSeconds( delay );
		Debug.Log( "Finished Tutorial One" );

		anim.SetInteger( "Tutorial" , 2 );

	}

}
