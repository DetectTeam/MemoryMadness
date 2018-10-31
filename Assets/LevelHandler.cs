using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {

	
	[SerializeField] private int levelsPerStage = 3;
	[SerializeField] private int numberOfStages = 4;

	[SerializeField] private int currentLevel;
	[SerializeField] private int currentStage;

	[SerializeField] private GameObject results;
	[SerializeField] private GameObject memoryPhase;

	[SerializeField] private bool isGameOver = false;
	
	private void OnEnable()
	{
		Messenger.AddListener( "LoadNextLevel", LoadNextLevel );
		
	}

	private void OnDisable()
	{
		Messenger.RemoveListener( "LoadNextLevel", LoadNextLevel );
	}
	
	public void IncrementStage()
	{
		currentStage ++;

		if( currentStage >= numberOfStages  )
		{
			currentStage = 0;
			PlayerPrefs.SetInt( "CurrentStage" , currentStage );
		}
	}


	
	public void LoadNextLevel()
	{
		
		

		currentLevel = 0;

		if( PlayerPrefs.HasKey( "CurrentLevel" ) )
		{
			currentLevel = PlayerPrefs.GetInt( "CurrentLevel" );

			currentLevel++;

			Debug.Log( "Current Level " + currentLevel + " " + levelsPerStage );

			if( currentLevel == levelsPerStage )
			{
				//Reset the level count
				currentLevel = 0;
				PlayerPrefs.SetInt( "CurrentLevel", currentLevel );

				currentStage ++;

				//Load the Stats screen
				results.SetActive( true );

				if( currentStage >= numberOfStages )
				{
					Debug.Log( "You have completed the game..." );
					return;
				}

			
			}
			else
			{
				PlayerPrefs.SetInt( "CurrentLevel" , currentLevel );
				memoryPhase.SetActive( true );
			}
		}
	}
}
