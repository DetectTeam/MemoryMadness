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
	}
	private void Start () 
	{
		Debug.Log( SystemInfo.deviceType );
		Debug.Log( SystemInfo.deviceName );
		Debug.Log( SystemInfo.deviceModel );
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
