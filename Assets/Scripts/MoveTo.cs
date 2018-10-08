using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour 
{
	[SerializeField] private Vector3 target;

	// Use this for initialization
	void Start () 
	{
		iTween.MoveTo( gameObject,iTween.Hash( 
			"position", target,
			"islocal" , true,
			"easetype",iTween.EaseType.easeInOutSine,
			"time", 0.3f ));            
	}
	
	
}
