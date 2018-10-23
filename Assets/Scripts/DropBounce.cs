using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBounce : MonoBehaviour {

	// Use this for initialization


	[SerializeField] private Vector3 target;

	[SerializeField] private bool delayEnabled = false;
	[SerializeField] private float delay;

	private void OnEnable()
	{
		StartCoroutine( Drop() );
		//PlayerPrefs.DeleteAll();
	}
	private void Start () 
	{
		
		//Drop();
	}

	private IEnumerator Drop()
	{
		yield return null;

		if( delayEnabled )
		{
			yield return new WaitForSeconds( delay );
		}

		 iTween.MoveTo(gameObject,iTween.Hash(
			  "position", target,
			  "islocal", true,
			  "easetype",iTween.EaseType.easeOutBounce,
			  "time",1.0f)
		  );
	}
	

}
