using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTutorial2 : StateMachineBehaviour 
{
	[SerializeField] private float delay = 3.0f;

	[SerializeField] private Animator anim;
    [SerializeField] private GameObject symbolContainer;

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		anim = animator;
		CoRoutineSlave.Instance.ExecCoroutine( Sequence() );
	}


	//Tutorial 2	
	private IEnumerator Sequence()
	{
		Debug.Log( "Starting Tutorial 2" );
	
		yield return new WaitForSeconds( delay );

		anim.SetInteger( "Tutorial" , 3 );
		Debug.Log( "Im done here..." );
	}
}
