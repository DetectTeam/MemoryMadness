using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum LevelType
{			
	UnNameableColour,
	UnNameableNonColour,
	NameableColour,
	NameableNonColour
}

public class StageManager : MonoBehaviour 
{

	[SerializeField] private int levelCount = 0;
	[SerializeField] private int levelsPerStage = 3;
	[SerializeField] private int numberOfStages = 4;
	[SerializeField] private GameObject memoryPhase;
	[SerializeField] private GameObject results;

	private const string LoadNextLevelMessage = "LoadNextLevel";
	private const string CurrentStageMessage = "CurrentStage";
	private const string CurrentLevelPrefs = "CurrentLevel";

	[SerializeField] private int maxNumLevelsPerStage = 7;
	[SerializeField] private static int currentStage = 3;
	public static int CurrentStage { get{ return currentStage; } set{ currentStage = value; } }
	private static int currentLevel = 0;
	public static int CurrentLevel { get{ return currentLevel; } set{ currentLevel = value; } }

	[SerializeField] private List<LevelType> stage;
	public List<LevelType> Stage { get{ return stage; } }

	[SerializeField] private static LevelType currentLevelType;	
	public static LevelType CurrentLevelType { get{ return currentLevelType; } }

	private void Awake()
	{
		LoadStage();
		RefreshStage();
		SetCurrentLevelType();
	}

	private void OnEnable()
	{
		Messenger.AddListener( "IncrementLevel", IncrementLevel );
		Messenger.AddListener( "IncrementStage", IncrementStage );
		Messenger.AddListener( LoadNextLevelMessage, LoadNextLevel );
	}

	private void OnDisable()
	{
		Messenger.RemoveListener( "IncrementLevel", IncrementLevel );
		Messenger.RemoveListener( "IncrementStage", IncrementStage );
		Messenger.RemoveListener( LoadNextLevelMessage, LoadNextLevel );
	}

		public void LoadNextLevel()
		{
			if( levelCount < 7 )
			{
				Debug.Log( "Loding level " + levelCount );
				//Request a Reset of the random level generator
				Messenger.Broadcast( "ResetLevelGenerator" );
				levelCount ++;
				memoryPhase.SetActive( true );
			}
			else if( levelCount >= 7 && levelCount < 9 )
			{
				Debug.Log( "READY TO LOAD SD LEVELS ....." );
				Messenger.Broadcast( "DisableRandomLevelGenerator" );
				Messenger.Broadcast( "ResetSDGenerator" );
				memoryPhase.SetActive( true );
				levelCount ++;
			}
			else
			{
				Debug.Log( "End of stage..." );
				IncrementStage();
				results.SetActive( true );
				Messenger.Broadcast( "Results" , 85.0f );
				Messenger.Broadcast( "ResetRandomLevelGenerator" );
				levelCount = 0;
			}
		}

	private void IncrementLevel()
	{
		currentLevel++;
		
		// if( currentLevel > maxNumLevelsPerStage )
		// {
		// 	Debug.Log( "Level Greater than 7" );
		// 	IncrementStage();
		// 	currentLevel = 0;
		// }

		Debug.Log( "CurrentLevel:  >>>>>>>>>> " + currentLevel );
        if( currentLevel < maxNumLevelsPerStage )
			SetCurrentLevelType();	
		else
			currentLevel = 0;

		
	}

	private void IncrementStage()
	{
		Debug.Log( "Incrmenting Stage..." + currentStage );
		currentStage ++;
		Debug.Log( "Stage is now.. " + currentStage );
		SaveStage();
		RefreshStage();

	}

	private void SaveStage( )
	{
		PlayerPrefs.SetInt( "CurrentStage", currentStage );
	}

	private void LoadStage( )
	{
		if( PlayerPrefs.HasKey( "CurrentStage" ) )
		{
			currentStage = PlayerPrefs.GetInt( "CurrentStage" );
		}
		else
		{
			currentStage = 2;
		}
	}

	public void SetCurrentLevelType()
	{
		if( currentLevel < stage.Count  )
			currentLevelType = stage[ currentLevel ]; 
	}

	private void RefreshStage()
	{
		stage.ShuffleList();
	}

}
