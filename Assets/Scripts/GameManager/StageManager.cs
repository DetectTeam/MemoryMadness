using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryMadness
{

	[System.Serializable]
	public enum LevelType
	{			
		UnNameableColour,
		UnNameableNonColour,
		NameableColour,
		NameableNonColour
	}

	public class StageManager : Singleton<StageManager>
	{
		[SerializeField] private int levelCount = 0;
		public int LevelCount { get{ return levelCount; } }
		[SerializeField] private int levelsPerStage;
		public int LevelsPerStage { get{ return levelsPerStage; } }
		[SerializeField] private int currentLevelTypeCount;
		[SerializeField] private int maxNumLevelsPerStage = 7;
		[SerializeField] private static int currentStage;
		public int CurrentStage { get{ return currentStage; } set{ currentStage = value; } }
		[SerializeField] private  int currentLevel = 0;
		public int CurrentLevel { get{ return currentLevel; } set{ currentLevel = value; } }
		
		[SerializeField] private int numberOfStages;
		[SerializeField] private GameObject sameDifferentScreen;
		[SerializeField] private GameObject randomGameContainer;
		[SerializeField] private GameObject memoryPhaseScreen;
		[SerializeField] private GameObject resultsScreen;

		private const string LoadNextLevelMessage = "LoadNextLevel";
		private const string CurrentStageMessage = "CurrentStage";
		private const string CurrentLevelPrefs = "CurrentLevel";


		[SerializeField] private List<LevelType> stage;
		public List<LevelType> Stage { get{ return stage; } }

		[SerializeField] private static LevelType currentLevelType;	
		public LevelType CurrentLevelType { get{ return currentLevelType; } }

		//public static StageManager Instance = null;

		private void Awake()
		{
			LoadStage();
			levelCount = LoadLevel();
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
			//End The Session for this level
			Messenger.Broadcast<int, int>( "EndSession", levelCount , levelsPerStage );
			//SessionManager.Instance.EndSession( levelCount , levelsPerStage );
				
			if( levelCount < levelsPerStage )
			{
				//Request a Reset of the random level generator
				Messenger.Broadcast( "ResetLevelGenerator" );
				levelCount ++;
				memoryPhaseScreen.SetActive( true );
			}
			//else if( levelCount >= levelsPerStage && levelCount < maxNumLevelsPerStage )
			//{
				//Messenger.Broadcast( "DisableRandomLevelGenerator" );
				//Messenger.Broadcast( "ResetSDGenerator" );
					
				//randomGameContainer.SetActive( false );
				//sameDifferentScreen.SetActive( true );
					
				//memoryPhaseScreen.SetActive( true );
				//levelCount ++;
			//}
			else
			{	
				IncrementStage();
				Messenger.Broadcast( "DisableRandomLevelGenerator" );
				randomGameContainer.SetActive( false );
			
				resultsScreen.SetActive( true );
				Messenger.Broadcast( "Results" , 85.0f );
				Messenger.Broadcast( "ResetLevelGenerator" );
				levelCount = 1;
			}

			//SaveLevel();
		}

		private void IncrementLevel()
		{
			SetCurrentLevelType();	
		}

		private void IncrementStage()
		{
			currentStage ++;
			currentLevel = 0;
			currentLevelTypeCount = 0;
		
			SaveStage();
			RefreshStage();
			SetCurrentLevelType();
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
				currentStage = 1;
			}
		}

		private void SaveLevel()
		{
			PlayerPrefs.SetInt( "CurrentLevel", levelCount );
		}

		private int LoadLevel()
		{
			int level = 1;
			
			if( PlayerPrefs.HasKey( "CurrentLevel" ) )
				level = PlayerPrefs.GetInt( "CurrentLevel" );

			return level;	
		}

	
		public void SetCurrentLevelType()
		{
			Debug.Log( "Setting Current Level Type " + currentLevelTypeCount + " " + stage.Count );
			
			if( currentLevelTypeCount < stage.Count  )
			{
				currentLevelType = stage[ currentLevelTypeCount ]; 
				currentLevelTypeCount ++;
			}
		}

		private void RefreshStage()
		{
			stage.ShuffleList();
		}

		
	}
}
