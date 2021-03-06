﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryMadness
{
	public class LevelHandler : MonoBehaviour 
	{


		//[SerializeField] private int levelCount = 0;
		
		//[SerializeField] private int levelsPerStage = 3;
		//[SerializeField] private int numberOfStages = 4;



		[SerializeField] private int currentLevel;
		[SerializeField] private int currentStage;

		
		//[SerializeField] private bool isGameOver = false;

		//Messages

		private const string LoadNextLevelMessage = "LoadNextLevel";
		private const string CurrentStageMessage = "CurrentStage";
		private const string CurrentLevelPrefs = "CurrentLevel";

		[SerializeField] private GameObject memoryPhase;
		[SerializeField] private GameObject results;
		
		private void OnEnable()
		{
			//Messenger.AddListener( LoadNextLevelMessage, LoadNextLevel );
		}

		private void OnDisable()
		{
			//Messenger.RemoveListener( LoadNextLevelMessage, LoadNextLevel );
		}
		
		// public void IncrementStage()
		// {
		// 	currentStage ++;

		// 	if( currentStage >= numberOfStages  )
		// 	{
		// 		currentStage = 0;
		// 		PlayerPrefs.SetInt( LoadNextLevelMessage , currentStage );
		// 	}
		// }

		// public void LoadNextLevel()
		// {
			
		// 	if( levelCount < 7 )
		// 	{
		// 		//Request a Reset of the random level generator
		// 		Messenger.Broadcast( "ResetLevelGenerator" );
		// 		levelCount ++;
		// 		memoryPhase.SetActive( true );
		// 	}
		// 	else if( levelCount >= 7 && levelCount < 9 )
		// 	{
		// 		Debug.Log( "READY TO LOAD SD LEVELS ....." );
		// 		Messenger.Broadcast( "ResetSDGenerator" );
		// 		levelCount ++;
		// 	}
		// 	else
		// 	{
		// 		results.SetActive( true );
		// 		Messenger.Broadcast( "Results" , 85.0f );
		// 		Messenger.Broadcast( "ResetLevelGenerator" );
		// 		levelCount = 0;
		// 	}

		// }
	}

}
