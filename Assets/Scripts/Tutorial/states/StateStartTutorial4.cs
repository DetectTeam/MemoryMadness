using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStartTutorial4 : StateMachineBehaviour 
{

	[SerializeField] private float delay = 3.0f;
	[SerializeField] private Animator anim;

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		anim = animator;
		
		IEnumerator coRoutine = Tutorial4();
		
		CoRoutineSlave.Instance.ExecCoroutine( coRoutine );	
	}

	private IEnumerator Tutorial4(  )
	{
		yield return new WaitForSeconds( delay );
		Debug.Log( "Finished Tutorial Four" );

		anim.SetInteger( "Tutorial" , 0 );
	}

	
	
}
