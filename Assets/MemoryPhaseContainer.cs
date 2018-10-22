using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPhaseContainer : MonoBehaviour 
{


	[SerializeField] private GameObject[] phases;
	[SerializeField] private float delay;

	private void OnEnable()
	{
		StartCoroutine( LoadPhase() );
	}

	// Use this for initialization
	void Start () 
	{
		
	}

	private IEnumerator LoadPhase()
	{
		yield return new WaitForSeconds( delay );

		int currentPhase = 2;
		//Check the level count

		if( PlayerPrefs.HasKey( "CurrentLevel" ) )
		{
			currentPhase = PlayerPrefs.GetInt( "CurrentLevel" );
		}
		
		phases[ currentPhase ].SetActive( true );
		//Based on level count set correct phase to active
	}
	
	
}
