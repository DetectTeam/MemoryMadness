using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour 
{

	[SerializeField] private Vector3 target;

	// Use this for initialization
	void Start () 
	{
		//StartCoroutine( Sequence() );
	}

	private IEnumerator Sequence()
	{
		yield return new WaitForSeconds( 1.0f );
		ScalePop();

	}

	private void ScalePop()
	{
			iTween.ScaleTo( gameObject,iTween.Hash( 
			"scale", target,
			"islocal" , true,
			"easetype",iTween.EaseType.easeInOutElastic,
			"time", 1.0f ));   
	}
	

}
