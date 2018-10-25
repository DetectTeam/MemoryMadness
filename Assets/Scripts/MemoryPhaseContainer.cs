using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPhaseContainer : MonoBehaviour 
{


	[SerializeField] private GameObject[] phases;
	[SerializeField] private float delay;

	[SerializeField] private GameObject results;

	private void OnEnable()
	{
		if( results.activeSelf ) 
			results.SetActive( false );

		

		StartCoroutine( LoadPhase() );
	}

	// Use this for initialization
	void Start () 
	{
		
	}

	private IEnumerator LoadPhase()
	{
		yield return new WaitForSeconds( delay );

		int currentPhase = 0;
		//Check the level count

		if( PlayerPrefs.HasKey( "CurrentLevel" ) )
		{
			currentPhase = PlayerPrefs.GetInt( "CurrentLevel" );
		}
		else
		{
			PlayerPrefs.SetInt( "CurrentLevel" , currentPhase );
		}
		
		phases[ currentPhase ].SetActive( true );
		//Based on level count set correct phase to active
	}
	
	
}
