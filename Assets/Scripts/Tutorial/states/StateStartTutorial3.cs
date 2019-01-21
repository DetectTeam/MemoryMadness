using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStartTutorial3 : StateMachineBehaviour 
{

	[SerializeField] private float delay = 3.0f;
	[SerializeField] private Animator anim;

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		anim = animator;
		
		IEnumerator coRoutine = Tutorial3();
		
		CoRoutineSlave.Instance.ExecCoroutine( coRoutine );	
	}

	private IEnumerator Tutorial3(  )
	{
		yield return new WaitForSeconds( delay );
		Debug.Log( "Finished Tutorial Three" );

		anim.SetInteger( "Tutorial" , 4 );
	}

	
	
}
