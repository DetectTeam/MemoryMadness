using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour 
{
	[SerializeField] private GameObject[] rows;

	void OnEnable()
	{
		StartCoroutine( Sequence() );
	}

	void OnDisable()
	{
		gameObject.SetActive( false );
	}

	// Use this for initialization
	void Start () 
	{
		//StartCoroutine( Sequence() );
	}

	private IEnumerator Sequence()
	{
		int count = 0;
		yield return null;

		while( count < rows.Length )
		{
			yield return new WaitForSeconds( 0.2f );
		
			rows[ count ].SetActive( true );
			count ++;
		}

		
	}
	
	
}
