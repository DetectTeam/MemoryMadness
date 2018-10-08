using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour 
{
	[SerializeField] private GameObject[] rows;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine( Sequence() );
	}

	private IEnumerator Sequence()
	{
		int count = 3;
		yield return null;

		while( count >= 0 )
		{
			yield return new WaitForSeconds( 0.35f );
			Debug.Log( count );
			rows[count].SetActive( true );
			count --;
		}

		
	}
	
	
}
