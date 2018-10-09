using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBounce : MonoBehaviour {

	// Use this for initialization

	[SerializeField] private Vector3 target;

	private void OnEnable()
	{
		Drop();
	}
	private void Start () 
	{
		  Drop();
	}

	private void Drop()
	{
		 iTween.MoveTo(gameObject,iTween.Hash(
			  "position", target,
			  "islocal", true,
			  "easetype",iTween.EaseType.easeOutBounce,
			  "time",1.0f)
		  );
	}
	

}
