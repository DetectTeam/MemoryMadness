using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MemoryMadness;

public class StateTutorial7 : StateMachineBehaviour 
{
	[SerializeField] private Animator anim;
	
	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
		if( TutorialManager.Instance )
			TutorialManager.Instance.EndTutorial.SetActive( true );
	}	
}
