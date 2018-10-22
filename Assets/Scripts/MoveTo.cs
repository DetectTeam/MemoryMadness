using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour 
{
	[SerializeField] private Vector3 target;

	[SerializeField] private float delay;

	// Use this for initialization
	
	private void OnEnable()
	{
		StartCoroutine( Move() );
	}


	private void Start () 
	{
		
	}


	private IEnumerator Move()
	{
		
		yield return new WaitForSeconds( delay );
		
		iTween.MoveTo( gameObject,iTween.Hash( 
			"position", target,
			"islocal" , true,
			"easetype",iTween.EaseType.easeInOutSine,
			"time", 0.3f ));    
	}
	
	
}
