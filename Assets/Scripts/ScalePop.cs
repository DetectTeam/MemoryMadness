using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePop : MonoBehaviour 
{
	[SerializeField] private Vector3 target;
	// Use this for initialization
	void Start () 
	{
		StartCoroutine( Sequence() );
	}

	private IEnumerator Sequence()
	{
		yield return new WaitForSeconds( 1.0f );
		ScaleP();
	}



	private void ScaleP()
	{
			iTween.ScaleTo( gameObject,iTween.Hash( 
			"scale", target,
			"islocal" , true,
			"easetype",iTween.EaseType.easeInOutElastic,
			"time", 1.0f ));   
	}

	private void PunchScale()
	{
		iTween.PunchScale( gameObject, new Vector3( 2.0f, 2.0f, 0.0f ), 0.5f );
	}

}
