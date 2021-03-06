﻿using System.Collections;
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

			//StartCoroutine( "DisableGameContainer" );
			StartCoroutine( "DisableTitleScreen" );

			SessionManager.Instance.CreateSession();
			
		
			//StartCoroutine( LoadPhase() );
		}

		private IEnumerator DisableGameContainer()
		{
				yield return new WaitForSeconds( 1.0f );
				if( gameObject.activeSelf )
				game.SetActive( false );
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
				Debug.Log( currentPhase );
				
			}
			else
			{
				PlayerPrefs.SetInt( "CurrentLevel" , currentPhase );
			}

		}
		
		
	}

}
