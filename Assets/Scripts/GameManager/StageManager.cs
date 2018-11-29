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
		//RefreshStage();
		SetCurrentLevelType();
	}

	private void OnEnable()
	{
		Messenger.AddListener( "IncrementLevel", IncrementLevel );
		Messenger.AddListener( "IncrementStage", IncrementStage );
	}

	private void OnDisable()
	{
		Messenger.RemoveListener( "IncrementLevel", IncrementLevel );
		Messenger.RemoveListener( "IncrementStage", IncrementStage );
	}

	private void IncrementLevel()
	{
		currentLevel++;
		
		if( currentLevel > maxNumLevelsPerStage )
		{
			Debug.Log( "Level Greater than 7" );
			IncrementStage();
			currentLevel = 0;
		}

		Debug.Log( "CurrentLevel:  >>>>>>>>>> " + currentLevel );

		SetCurrentLevelType();	

		
	}

	private void IncrementStage()
	{
		Debug.Log( "Incrmenting Stage..." + currentStage );
		currentStage ++;
		Debug.Log( "Stage is now.. " + currentStage );
		SaveStage();
		//RefreshStage();

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
