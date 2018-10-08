using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelectionScreen : MonoBehaviour 
{
	[SerializeField] private GameObject titleScreen;
	[SerializeField] private GameObject title;
	[SerializeField] private GameObject easy;
	[SerializeField] private GameObject normal;
	[SerializeField] private GameObject hard;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine( Sequence() );
	}
	
	private IEnumerator Sequence()
	{
		//yield return new WaitForSeconds( 1.0f );

		title.SetActive( true );

		yield return new WaitForSeconds( 1.0f );
		
		titleScreen.SetActive( false );
	}
}
