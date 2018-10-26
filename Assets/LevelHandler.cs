using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {

	
	[SerializeField] private int levelsPerStage = 6;
	[SerializeField] private int numberOfStages = 4;

	[SerializeField] private int currentLevel;
	[SerializeField] private int currentStage;

	[SerializeField] private GameObject results;
	[SerializeField] private GameObject memoryPhase;
	
	private void OnEnable()
	{
		Messenger.AddListener( "LoadNextLevel", LoadNextLevel );
	}

	private void OnDisable()
	{
		Messenger.RemoveListener( "LoadNextLevel", LoadNextLevel );
	}
	public void LoadNextLevel()
	{
		currentLevel = 0;
		
		if( PlayerPrefs.HasKey( "CurrentLevel" ) )
		{
			currentLevel = PlayerPrefs.GetInt( "CurrentLevel" );

			currentLevel++;

			Debug.Log( currentLevel + " " + levelsPerStage );
			
			if( currentLevel == levelsPerStage )
			{
				//Reset the level count
				currentLevel = 0;
				currentStage ++;

				//Update the current level count
				PlayerPrefs.SetInt( "CurrentLevel", currentLevel );
				PlayerPrefs.SetInt( "CurrentStage", currentStage );

				//Load the Stats screen
				results.SetActive( true );
			}
			else
			{
				PlayerPrefs.SetInt( "CurrentLevel" , currentLevel );
				memoryPhase.SetActive( true );
			}
		}
	}
}
