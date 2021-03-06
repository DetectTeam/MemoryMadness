﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json; //Json Library


namespace MemoryMadness
{
	public enum SessionState
	{
		Started,
		Ended	
	}
		
	public class SessionManager : MonoBehaviour//Singleton<SessionManager>
	{
		public static int SessionID;
		public static SessionManager Instance = null;
		
		//private int orderCount = 1;
		[SerializeField] private int trialNumber = 0;
		//[SerializeField] private int levelSize = 20;
		[SerializeField] private Session session;
		[SerializeField] private PlayerSelection playerSelection;

		private int lifeCount;

		private int timeOut;
		
		
		private SessionState sessionState; 

		private int currentLevel = 1;

		public int CurrentLevel { get{ return currentLevel; } set{ currentLevel = value; } }
	
		private float trialTime;
		private float startTime , endTime = 0 ;

		private void Awake()
		{
			
			
			//DontDestroyOnLoad(gameObject);
			 //Check if instance already exists
             if (Instance == null)
                
                //if not, set instance to this
                 Instance = this;
            
            //If instance already exists and it's not this:
             else if (Instance != this)
                
                 //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                 Destroy(gameObject);    
            
        //     //Sets this to not be destroyed when reloading scene
            // DontDestroyOnLoad(gameObject);
		}
		
		private void OnEnable()
		{
			Messenger.AddListener( "CreatePlayerSelection" , CreatePlayerSelection );
			
			Messenger.AddListener< string >( "CorrectSlotPosition" , SetCorrectSlot );
			//Messenger.AddListener< int >( "SetSelectionOrder", SetOrderSlot );
			Messenger.AddListener< int >( "PlayerSelection", SetPlayerSelection );
			Messenger.AddListener< float >( "RecordTime", SetPlayerSelectionTime );
			Messenger.AddListener< int , int ,int  >( "SelectedShapeDetails" , SetSelectedShapeDetails );
			Messenger.AddListener< int, int >( "EndSession", EndSession );
			Messenger.AddListener( "CurrentLevel", IncrementLevel );
			Messenger.AddListener<int>( "SetLifeCount",  SetLifeCount );
			Messenger.AddListener( "DecreaseLifeCount", DecreaseLifeCount );
			Messenger.AddListener( "Timeout", Timeout );
			

			//Messenger.AddListener(  "DecrementLife", UpdateLifeCount );

			Messenger.MarkAsPermanent ( "CreatePlayerSelection" );
			Messenger.MarkAsPermanent ( "CorrectSlotPosition" );
			Messenger.MarkAsPermanent ( "PlayerSelection" );
			Messenger.MarkAsPermanent ( "RecordTime" );
			Messenger.MarkAsPermanent ( "SelectedShapeDetails" );
			Messenger.MarkAsPermanent ( "EndSession" );
			Messenger.MarkAsPermanent ( "CurrentLevel" );
			Messenger.MarkAsPermanent ( "DecreaseLifeCount" );
			Messenger.MarkAsPermanent ( "SetLifeCount" );
			Messenger.MarkAsPermanent ( "Timeout" );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( "CreatePlayerSelection" , CreatePlayerSelection );
			Messenger.RemoveListener< string >( "CorrectSlotPosition" , SetCorrectSlot );
			//Messenger.RemoveListener< int >( "SetSelectionOrder", SetOrderSlot );
			Messenger.RemoveListener< float >( "RecordTime", SetPlayerSelectionTime );
			Messenger.RemoveListener< int >( "PlayerSelection", SetPlayerSelection );
			Messenger.RemoveListener< int , int ,int  >( "SelectedShapeDetails" , SetSelectedShapeDetails );
			Messenger.RemoveListener< int, int >( "EndSession", EndSession );
			Messenger.RemoveListener( "CurrentLevel", IncrementLevel );
			Messenger.RemoveListener<int>( "SetLifeCount", SetLifeCount );
			Messenger.RemoveListener( "DecreaseLifeCount", DecreaseLifeCount );
			Messenger.RemoveListener( "Timeout", Timeout );
			
			//Messenger.RemoveListener(  "DecrementLife", UpdateLifeCount );
		}

		public void CreateSession()
		{
					
			//if( PlayerPrefs.HasKey( "SessionID" ) )
				//trialNumber = PlayerPrefs.GetInt( "SessionID" );

			session = new Session();

			//trialNumber ++;

			//Get Device unique identifier for test only.
			session.SessionID = trialNumber.ToString();

			//PlayerPrefs.SetInt( "SessionID" , trialNumber );
			
			//orderCount = 1;

			sessionState = SessionState.Started;

			//session.UserID = SystemInfo.deviceUniqueIdentifier;
			session.SessionName = "Session_Name";
			session.Date = System.DateTime.Now.ToString( "dd_MM_yyyy" );
			session.AbsoluteTimeOfResponse = System.DateTime.Now.ToString( "HH:mm:ss tt" );
			session.SessionTimeStamp =  System.DateTime.Now.ToString( "yyyy_MM_dd_hh_mm_ss" );
			session.Stage = StageManager.Instance.CurrentStage;
			//session.SymbolArraySize = CalculateSymbolArraySize( StageManager.Instance.CurrentStage );
			//session.StudyCellSize = CalculateSymbolArraySize( StageManager.Instance.CurrentStage );	


			int stage = StageManager.Instance.CurrentStage;
			int size = 0;

			if( stage == 1 )
				size  = 2;
			else if( stage == 2 )
				size = 3;
			else if( stage == 3 )
				size = 4;

			session.SymbolArraySize = size;
			session.StudyCellSize = size;

			session.TrialNumber = 0;
			session.ApplicationQuit = "0";
		

			//Debug.Log( RandomLevelGenerator );

			if( RandomLevelGenerator_V2.Instance )
				SetStudyItems( session, RandomLevelGenerator_V2.Instance.MemoryPhaseSymbols );
			else
				Debug.Log( "Study Item Problem" );

			SetTestSlotItems();

			if( StageManager.Instance.CurrentLevelType == LevelType.NameableColour ||
				StageManager.Instance.CurrentLevelType == LevelType.UnNameableColour )
			{ 
				session.Condition = "Binding"; 
			}
			else
			{ 
				session.Condition = "Shape"; 
			}

			if( StageManager.Instance.CurrentLevelType == LevelType.UnNameableColour ||
				StageManager.Instance.CurrentLevelType == LevelType.UnNameableNonColour )
			{ 
				session.Nameability = "Abstract"; 
			}
			else
			{ 
				session.Nameability = "Nameable"; 
			}	

			//Debug.Log( "Condition: " + session.Condition );
			//Debug.Log( "Shape Type: " + session.Nameability );

			PersistenceManager.Instance.FileName = session.SessionName + "_" + session.SessionTimeStamp  + ".dat";
		}

		private int CalculateSymbolArraySize( int stage )
		{
			int res = 0; 

			if( stage <= 2 )
				res = 2;
			else if( stage == 3 )
				res = 3;
			else if( stage == 4 )
				res = 4;
			else if( stage >= 5 )
				res = 5;
			
			return res;
		}
	
		private void SetStudyItems( Session session, List<Symbol> source )
		{
			string notAvailable = "NA";
			Debug.Log( "Study Items" );
			for( int i = 0; i < source.Count; i++ )
			{
				StudyItem item = new StudyItem(); 

				item.StudyCellNumber = source[i].Index.ToString();
			 	item.ColourCode = source[i].BackgroundColor.ColourCode.ToString();
			 	item.ShapeCode = source[i].CurrentShape.ShapeCode.ToString();

				session.StudyItems.Add( item );
			}

		   	for( int x = source.Count; x < 4; x++ )
			{
				StudyItem item = new StudyItem(); 
				
				item.StudyCellNumber = notAvailable;
			 	item.ColourCode = notAvailable;
			 	item.ShapeCode = notAvailable;

				session.StudyItems.Add( item );
			}
		}

		private void SetTestSlotItems()
		{
			List<GameObject> symbolList = RandomLevelGenerator_V2.Instance.CurrentLevelSymbols;

			for( int i = 0; i < symbolList.Count; i++ )
			{
				MemorySymbols memSymbolsScript = symbolList[i].GetComponent<MemorySymbols>();
				TestSlot slot = new TestSlot();
				slot.ColourCode = memSymbolsScript.ColourCode.ToString();
				slot.ShapeCode = memSymbolsScript.ShapeCode.ToString();

				slot.CellNumber = (i+1).ToString();
				session.TestSlots.Add( slot );
			}
		}

		private void CreatePlayerSelection()
		{
			playerSelection = new PlayerSelection();
			playerSelection.Level = currentLevel;

			timeOut = 0;
	
		}

		private void SetCorrectSlot( string slot  )
		{
			playerSelection.CorrectPosition = slot;
		}


		private float relativeTime = 0;
		private void SetPlayerSelectionTime( float time )
		{
			time = (float)System.Math.Round (time * 1000 ); //Milliseconds

			relativeTime = relativeTime + time;
		
			playerSelection.RelativeTime = relativeTime.ToString();
			playerSelection.ReactionTime = time.ToString();

			playerSelection.MaxNumberOfLives = lifeCount;
			
			Debug.Log( "Saving.........timeout" );
			playerSelection.TimeOut = timeOut;

			session.PlayerSelections.Add( playerSelection );
			SaveSession(  );
		}

		private int selectionCount = 0;
		private void SetPlayerSelection( int selection )
		{
			selectionCount ++;
			playerSelection.Selection = selectionCount.ToString();
			playerSelection.TimeOfSelection = System.DateTime.Now.ToString( "yyyy_MM_dd_hh_mm_ss_ms" );
			trialNumber ++;
			playerSelection.TrialNumber = trialNumber;
			
			CheckSelectionCorrect( selection );
			SetPlayerSelectionLure( selection );
			SetPlayerSelectionOtherMiss( selection );

			
		}

		
		private void CheckSelectionCorrect( int selection )
		{
			if( selection == 1 )
			{
				playerSelection.Correct = "1";
				playerSelection.LifeLost = 0;

				playerSelection.NumberOfLivesRemaining = livesLeft;
				
			}
			else
			{
				playerSelection.Correct = "0";
				playerSelection.LifeLost = 1;
				
				livesLeft --;
				
				playerSelection.NumberOfLivesRemaining = livesLeft;
			}
			
			Debug.Log( "Player Selection Correct : " + playerSelection.Correct );
		}

		private void SetPlayerSelectionLure( int selection )
		{
			if( selection == 2 )
				playerSelection.Lure = "1";
			else
				playerSelection.Lure = "0"; 
		}

		private void SetPlayerSelectionOtherMiss( int selection )
		{
			if( selection == 3 )
				playerSelection.OtherMiss = "1";
			else
				playerSelection.OtherMiss = "0";
		}

		private void SetPlayerSelectionCorrectPosition( int position )
		{
			playerSelection.CorrectPosition = position.ToString();
		}

		private void SetSelectedShapeDetails( int shape, int colour, int position )
		{
			playerSelection.SelectedTestCellShape = shape.ToString();
			playerSelection.SelectedTestCellColour = colour.ToString();
			playerSelection.SelectedTestCellPosition = position.ToString();
		}

		private void PadSelections( int symbolCount, int selectionCount )
		{
			int maxCount = 0;
			int paddingCount = 0;
			string notAvailable = "NA";

			if( symbolCount ==  2 )
				maxCount = 3;
			else if( symbolCount == 3 )
				maxCount = 5;
			else if( symbolCount >= 4 )
				maxCount = 7;
				
			paddingCount = maxCount - selectionCount;

			for( int x = 0; x < paddingCount; x++ )
			{
				playerSelection = new PlayerSelection();
				playerSelection.Level = currentLevel;
				playerSelection.RelativeTime = notAvailable;
				playerSelection.ReactionTime = notAvailable;
				playerSelection.Selection = ( selectionCount + 1 ).ToString();
				playerSelection.Repeat = notAvailable;
				playerSelection.Interrupt = notAvailable;
				playerSelection.Lure = notAvailable;
				playerSelection.OtherMiss = notAvailable;
				playerSelection.Correct = notAvailable;
				playerSelection.CorrectPosition = notAvailable;
				playerSelection.SelectedTestCellColour = notAvailable;
				playerSelection.SelectedTestCellPosition = notAvailable;
				playerSelection.SelectedTestCellShape = notAvailable;
				playerSelection.MaxNumberOfLives = lifeCount;
				playerSelection.NumberOfLivesRemaining = livesLeft;
				playerSelection.TimeOut = timeOut;
				trialNumber ++;
				playerSelection.TrialNumber = trialNumber;
				playerSelection.TimeOfSelection = "";
				

				session.PlayerSelections.Add( playerSelection );
			}
		}

		private void SaveSession( )
		{
	
			PersistenceManager.Instance.Save( session );
			//string jsonString = JsonConvert.SerializeObject( session );	
		}	

		//Fix This !!
		public void EndSession( int levelCount , int levelsPerStage )
		{
			string jsonString = "";
			session.UserID = IDGenerator.Instance.UserID;
		
            PadSelections( session.SymbolArraySize, selectionCount );

			relativeTime = 0;
			selectionCount = 0;

			jsonString = JsonConvert.SerializeObject( session );

			if( levelCount <= levelsPerStage )
			{
				Messenger.Broadcast<string>( "PUT" , jsonString );
				SaveSession();
			}



			//if( trialNumber >= 32 )
				//trialNumber = 0;

			sessionState = SessionState.Ended;
		}

		private void IncrementLevel()
		{
			currentLevel ++;
			
			if( currentLevel > 8 )
				currentLevel = 1;
		}


		private int livesLeft = 0;
		private void SetLifeCount( int numLives )
		{
			lifeCount = numLives;
			livesLeft = numLives;
			//playerSelection.MaxNumberOfLives = lifeCount;
			Debug.Log( "Current Life Count: " + lifeCount );
		}


		private int livesLost = 0;
		private void DecreaseLifeCount()
		{
			
			livesLost = 1;
			Debug.Log( "Life Lost ...lives left " + livesLost );
		}

		private void Timeout()
		{
			Debug.Log( "Timed Out ............." );
			timeOut = 1;
			playerSelection.TimeOut = timeOut;
		}

		private void OnApplicationPause()
		{

		}

		private void OnApplicationQuit()
		{
			if( sessionState == SessionState.Started )
			{
				session.ApplicationQuit = "1";
				//jsonString = JsonConvert.SerializeObject( session );
				//SaveSession();

				#if !UNITY_EDITOR
					EndSession( StageManager.Instance.LevelCount , StageManager.Instance.LevelsPerStage );
				#endif
			}			
		}

	}
}
