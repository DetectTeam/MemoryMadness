using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryMadness
{
	public class MemoryPhaseContainer : MonoBehaviour 
	{
		[SerializeField] private GameObject[] phases;
		[SerializeField] private float delay;

		[SerializeField] private GameObject results;
		[SerializeField] private GameObject game;

		[SerializeField] private GameObject titleScreen;

		//[SerializeField] private GameObject randomLevelGenerator;
 
		private void OnEnable()
		{
			if( results.activeSelf ) 
				results.SetActive( false );
			
			if( gameObject.activeSelf )
				game.SetActive( false );

			// if( !randomLevelGenerator.activeSelf )	
			// 	randomLevelGenerator.SetActive( true );

			StartCoroutine( "DisableTitleScreen" );
			
		
			//StartCoroutine( LoadPhase() );
		}


		private IEnumerator DisableTitleScreen()
		{
			yield return new WaitForSeconds( 1.0f );
			if( titleScreen.activeSelf )
				titleScreen.SetActive( false );
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

			//Debug.Log( "currentPhase " + currentPhase );
			
			//Messenger.Broadcast( "LoadPhase" );
			//phases[ 0 ].SetActive( true );
			//Based on level count set correct phase to active
		}
		
		
	}

}
