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

	private static int currentStage = 0;
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
		currentLevel ++;

		if( currentLevel > 8 )
			currentLevel = 1;

		SetCurrentLevel();	
	}

	private void IncrementStage()
	{
		currentStage ++;

		if( currentStage > 8 )
			currentStage = 1;


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
			currentStage = 0;
		}
	}

	private void SetCurrentLevel()
	{
		currentLevelType = stage[ currentLevel ]; 
	}

	private void RefreshStage()
	{
		stage.ShuffleList();
	}






}
