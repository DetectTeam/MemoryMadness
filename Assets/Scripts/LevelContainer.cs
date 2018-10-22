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
		int count = rows.Length;
		yield return null;

		while( count > 0 )
		{
			yield return new WaitForSeconds( 0.2f );
		
			rows[count-1].SetActive( true );
			count --;
		}

		
	}
	
	
}
