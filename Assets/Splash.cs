using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		StartCoroutine( CountDown() );
	}

	private IEnumerator CountDown()
	{
		int count = 3;
		
		while ( count > 0 )
		{
			count --;
			yield return new WaitForSeconds( 1.0f );
		}

		SceneManager.LoadScene( "Memory_Madness_Random_Level" );
	}
	
}
