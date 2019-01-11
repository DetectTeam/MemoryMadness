using System.Collections;
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
		
	public class SessionManager : Singleton<SessionManager>
	{
		public static int SessionID;
		//public static SessionManager Instance = null;
		
		private int orderCount = 1;
		[SerializeField] private int trialNumber = 0;
		[SerializeField] private int levelSize = 20;
		[SerializeField] private Session session;
		[SerializeField] private PlayerSelection playerSelection;
		
		private SessionState sessionState; 
	
		// private void Awake()
		// {
		// 	sessionState = SessionState.Ended;
			
		// 	if( Instance == null )
		// 	{
		// 		Instance = this;
		// 	}
		// 	else
		// 	{
		// 		Destroy( gameObject );
		// 	}
		// }

		private void OnEnable()
		{
			Messenger.AddListener( "CreatePlayerSelection" , CreatePlayerSelection );
			Messenger.AddListener< string >( "CorrectSlotPosition" , SetCorrectSlot );
			//Messenger.AddListener< int >( "SetSelectionOrder", SetOrderSlot );
			Messenger.AddListener< int >( "PlayerSelection", SetPlayerSelection );
			Messenger.AddListener< float >( "RecordTime", SetPlayerSelectionTime );
			Messenger.AddListener< int , int ,int  >( "SelectedShapeDetails" , SetSelectedShapeDetails );
			Messenger.AddListener< int, int >( "EndSession", EndSession );
			//Messenger.AddListener(  "DecrementLife", UpdateLifeCount );
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
			//Messenger.RemoveListener(  "DecrementLife", UpdateLifeCount );
		}

		public void CreateSession()
		{
			if( PlayerPrefs.HasKey( "SessionID" ) )
				trialNumber = PlayerPrefs.GetInt( "SessionID" );

			session = new Session();

			trialNumber ++;

			session.SessionID = trialNumber;

			PlayerPrefs.SetInt( "SessionID" , trialNumber );
			
			orderCount = 1;

			sessionState = SessionState.Started;

			session.UserID = "DummyID0001";
			session.SessionName = "Session_Name";
			session.Date = System.DateTime.Now.ToString( "dd_MM_yyyy" );
			session.AbsoluteTimeOfResponse = System.DateTime.Now.ToString( "hh_mm" );
			session.SessionTimeStamp =  System.DateTime.Now.ToString( "yyyy_MM_dd_hh_mm_ss" );
			session.Stage = StageManager.Instance.CurrentStage;
			session.Level = StageManager.Instance.CurrentLevel + 1;
			session.SymbolArraySize = CalculateSymbolArraySize( StageManager.Instance.CurrentStage );
			session.StudyCellSize = CalculateSymbolArraySize( StageManager.Instance.CurrentStage );			
			session.TrialNumber = trialNumber;
			session.ApplicationQuit = "0";
		
			SetStudyItems( session, RandomLevelGenerator.Instance.MemoryPhaseSymbols );
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

			Debug.Log( "Condition: " + session.Condition );
			Debug.Log( "Shape Type: " + session.Nameability );

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
			for( int i = 0; i < source.Count; i++ )
			{
				StudyItem item = new StudyItem(); 
				
				item.StudyCellNumber = source[i].Index;
			 	item.ColourCode = source[i].BackgroundColor.ColourCode;
			 	item.ShapeCode = source[i].CurrentShape.ShapeCode;

				session.StudyItems.Add( item );
			}	
		}

		private void SetTestSlotItems()
		{
			List<GameObject> symbolList = RandomLevelGenerator.Instance.CurrentLevelSymbols;

			for( int i = 0; i < symbolList.Count; i++ )
			{
				MemorySymbols memSymbolsScript = symbolList[i].GetComponent<MemorySymbols>();
				TestSlot slot = new TestSlot();
				slot.ColourCode = memSymbolsScript.ColourCode;
				slot.ShapeCode = memSymbolsScript.ShapeCode;

				slot.CellNumber = i+1;
				session.TestSlots.Add( slot );
			}
		}

		private void CreatePlayerSelection()
		{
			playerSelection = new PlayerSelection();
		}

		private void SetCorrectSlot( string slot  )
		{
			playerSelection.CorrectPosition = slot;
			Debug.Log( "Correct Slot Position: " + playerSelection.CorrectPosition );
		}


		private float relativeTime = 0;
		private void SetPlayerSelectionTime( float time )
		{
			time = (float)System.Math.Round (time * 1000 ); //Milliseconds

			relativeTime = relativeTime + time;
		
			playerSelection.RelativeTime = relativeTime.ToString();
			playerSelection.ReactionTime = time.ToString();

			Debug.Log( "Relative Time: " + playerSelection.RelativeTime );
			Debug.Log( "Reaction Time: " + playerSelection.ReactionTime );

			session.PlayerSelections.Add( playerSelection );
			SaveSession(  );
		}

		private int selectionCount = 0;
		private void SetPlayerSelection( int selection )
		{
			selectionCount ++;
			playerSelection.Selection = selectionCount.ToString();
			Debug.Log( "Players Current Selection " + playerSelection.Selection );

			CheckSelectionCorrect( selection );
			SetPlayerSelectionLure( selection );
			SetPlayerSelectionOtherMiss( selection );
		}


		private void CheckSelectionCorrect( int selection )
		{
			if( selection == 1 )
				playerSelection.Correct = "1";
			else
				playerSelection.Correct = "0";
			
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
			string notAvailable = "N/A";

			if( symbolCount ==  2 )
				maxCount = 3;
			else if( symbolCount == 3 )
				maxCount = 5;
			else if( symbolCount >= 4 )
				maxCount = 7;
			
			//Debug.Log( "Symbol Count: " + symbolCount );
			
			paddingCount = maxCount - selectionCount;

			//Debug.Log( maxCount + " - " + selectionCount );
			//Debug.Log( "Padding Count: " + paddingCount );

			for( int x = 0; x < paddingCount; x++ )
			{
				playerSelection = new PlayerSelection();

				playerSelection.RelativeTime = notAvailable;
				playerSelection.ReactionTime = notAvailable;
				playerSelection.Selection = ( selectionCount++ ).ToString();
				playerSelection.Repeat = notAvailable;
				playerSelection.Interrupt = notAvailable;
				playerSelection.Lure = notAvailable;
				playerSelection.OtherMiss = notAvailable;
				playerSelection.Correct = notAvailable;
				playerSelection.CorrectPosition = notAvailable;
				playerSelection.SelectedTestCellColour = notAvailable;
				playerSelection.SelectedTestCellPosition = notAvailable;
				playerSelection.SelectedTestCellShape = notAvailable;

				session.PlayerSelections.Add( playerSelection );
			}
		}

		private void SaveSession( )
		{
			Debug.Log( "Saving Session" );
			PersistenceManager.Instance.Save( session );
			string jsonString = JsonConvert.SerializeObject( session );
			Debug.Log( ">>>>>>> " + jsonString + " <<<<<<<<<<" );
		}	

		//Fix This !!
		public void EndSession( int levelCount , int levelsPerStage )
		{
			Debug.Log( "Ending Session...." );
			
			string jsonString = "";
		
            PadSelections( session.SymbolArraySize, selectionCount );

			relativeTime = 0;
			selectionCount = 0;

			jsonString = JsonConvert.SerializeObject( session );

			if( levelCount <= levelsPerStage )
			{
				Messenger.Broadcast<string>( "PUT" , jsonString );
				SaveSession();
			}

			if( trialNumber >= 32 )
				trialNumber = 0;

			sessionState = SessionState.Ended;
		}

		private void OnApplicationQuit()
		{
			Debug.Log( "QUITTING APPLICATION ......." );

			if( sessionState == SessionState.Started )
			{
				session.ApplicationQuit = "1";
				//jsonString = JsonConvert.SerializeObject( session );
				//SaveSession();

				EndSession( StageManager.Instance.LevelCount , StageManager.Instance.LevelsPerStage );
			}

			Debug.Log( "Clean Up Complete......." );
			
		}

	}
}
