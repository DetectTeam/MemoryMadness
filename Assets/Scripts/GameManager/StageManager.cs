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

	

	public class StageManager : MonoBehaviour 
	{
		[SerializeField] private int levelCount = 0;
		public int LevelCount { get{ return levelCount; } }
		[SerializeField] private int levelsPerStage = 3;
		[SerializeField] private int numberOfStages = 4;
		[SerializeField] private GameObject sameDifferentScreen;
		[SerializeField] private GameObject randomGameContainer;
		[SerializeField] private GameObject memoryPhaseScreen;
		[SerializeField] private GameObject resultsScreen;

		private const string LoadNextLevelMessage = "LoadNextLevel";
		private const string CurrentStageMessage = "CurrentStage";
		private const string CurrentLevelPrefs = "CurrentLevel";

		[SerializeField] private int maxNumLevelsPerStage = 7;
		[SerializeField] private static int currentStage;
		public int CurrentStage { get{ return currentStage; } set{ currentStage = value; } }
		private  int currentLevel = 0;
		public int CurrentLevel { get{ return currentLevel; } set{ currentLevel = value; } }

		[SerializeField] private List<LevelType> stage;
		public List<LevelType> Stage { get{ return stage; } }

		[SerializeField] private static LevelType currentLevelType;	
		public LevelType CurrentLevelType { get{ return currentLevelType; } }

		public static StageManager Instance = null;

		private void Awake()
		{
			if( Instance == null )
			{
				Instance = this;
			}
			else
			{
				Destroy( gameObject );
			}
			
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
				//End The Session for this level
				SessionManager.Instance.EndSession();
				
				if( levelCount < 7 )
				{
					
					//Request a Reset of the random level generator
					Messenger.Broadcast( "ResetLevelGenerator" );
					levelCount ++;
					memoryPhaseScreen.SetActive( true );
				}
				else if( levelCount >= 7 && levelCount < 9 )
				{
					Messenger.Broadcast( "DisableRandomLevelGenerator" );
					Messenger.Broadcast( "ResetSDGenerator" );
					
					randomGameContainer.SetActive( false );
					sameDifferentScreen.SetActive( true );
					
				
					//memoryPhaseScreen.SetActive( true );
					levelCount ++;
				}
				else
				{
					IncrementStage();
					resultsScreen.SetActive( true );
					Messenger.Broadcast( "Results" , 85.0f );
					Messenger.Broadcast( "ResetLevelGenerator" );
					levelCount = 0;
				}
			}

		private void IncrementLevel()
		{
			currentLevel++;
		
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
				currentStage = 1;
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
}
