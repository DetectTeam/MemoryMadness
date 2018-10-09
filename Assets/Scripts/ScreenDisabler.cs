using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDisabler : MonoBehaviour 
{
	[SerializeField] private GameObject screen;

	void Start () 
	{

		if( !screen )
		{
			Debug.LogWarning( "Screen not set..." );
		}

		StartCoroutine( Sequence() );
	}
	
	private IEnumerator Sequence()
	{
		yield return new WaitForSeconds( 1.0f );

		if( screen.activeSelf )
			screen.SetActive( false );

	}
}
