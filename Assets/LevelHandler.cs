using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryMadness
{
	public class LevelHandler : MonoBehaviour {

		
		[SerializeField] private int levelsPerStage = 3;
		[SerializeField] private int numberOfStages = 4;

		[SerializeField] private int currentLevel;
		[SerializeField] private int currentStage;

		[SerializeField] private GameObject results;
		[SerializeField] private GameObject memoryPhase;

		[SerializeField] private bool isGameOver = false;

		//Messages

		private const string LoadNextLevelMessage = "LoadNextLevel";
		private const string CurrentStageMessage = "CurrentStage";
		private const string CurrentLevelPrefs = "CurrentLevel";
		
		private void OnEnable()
		{
			Messenger.AddListener( LoadNextLevelMessage, LoadNextLevel );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( LoadNextLevelMessage, LoadNextLevel );
		}
		
		public void IncrementStage()
		{
			currentStage ++;

			if( currentStage >= numberOfStages  )
			{
				currentStage = 0;
				PlayerPrefs.SetInt( LoadNextLevelMessage , currentStage );
			}
		}

		public void LoadNextLevel()
		{
			currentLevel = 0;

			if( PlayerPrefs.HasKey( CurrentLevelPrefs ) )
			{
				currentLevel = PlayerPrefs.GetInt( CurrentLevelPrefs );

				currentLevel++;

				if( currentLevel == levelsPerStage )
				{
					//Reset the level count
					currentLevel = 0;
					PlayerPrefs.SetInt( CurrentLevelPrefs, currentLevel );

					currentStage ++;

					//Load the Stats screen
					results.SetActive( true );

					if( currentStage >= numberOfStages )
					{
						return;
					}

				}
				else
				{
					PlayerPrefs.SetInt( CurrentLevelPrefs , currentLevel );
					memoryPhase.SetActive( true );
				}
			}
		}
	}

}
