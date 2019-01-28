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
		
	public class SessionManager : MonoBehaviour//Singleton<SessionManager>
	{
		public static int SessionID;
		public static SessionManager Instance = null;
		
		private int orderCount = 1;
		[SerializeField] private int trialNumber = 0;
		[SerializeField] private int levelSize = 20;
		[SerializeField] private Session session;
		[SerializeField] private PlayerSelection playerSelection;
		
		private SessionState sessionState; 
	
		
		private void Awake()
		{
			
			Debug.Log( "Session Manager Running..." );
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
			//Messenger.AddListener(  "DecrementLife", UpdateLifeCount );

			Messenger.MarkAsPermanent ( "CreatePlayerSelection" );
			Messenger.MarkAsPermanent ( "CorrectSlotPosition" );
			Messenger.MarkAsPermanent ( "PlayerSelection" );
			Messenger.MarkAsPermanent ( "RecordTime" );
			Messenger.MarkAsPermanent ( "SelectedShapeDetails" );
			Messenger.MarkAsPermanent ( "EndSession" );
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

			//Get Device unique identifier for test only.
			session.SessionID = trialNumber.ToString();

			PlayerPrefs.SetInt( "SessionID" , trialNumber );
			
			orderCount = 1;

			sessionState = SessionState.Started;

			session.UserID = SystemInfo.deviceUniqueIdentifier;
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
			
			Debug.Log( "Study Items" );
			for( int i = 0; i < source.Count; i++ )
			{
				StudyItem item = new StudyItem(); 

				Debug.Log( item );
				
				item.StudyCellNumber = source[i].Index;
			 	item.ColourCode = source[i].BackgroundColor.ColourCode;
			 	item.ShapeCode = source[i].CurrentShape.ShapeCode;

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
		}


		private float relativeTime = 0;
		private void SetPlayerSelectionTime( float time )
		{
			time = (float)System.Math.Round (time * 1000 ); //Milliseconds

			relativeTime = relativeTime + time;
		
			playerSelection.RelativeTime = relativeTime.ToString();
			playerSelection.ReactionTime = time.ToString();

			session.PlayerSelections.Add( playerSelection );
			SaveSession(  );
		}

		private int selectionCount = 0;
		private void SetPlayerSelection( int selection )
		{
			selectionCount ++;
			playerSelection.Selection = selectionCount.ToString();
			
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
				
			paddingCount = maxCount - selectionCount;

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
			PersistenceManager.Instance.Save( session );
			string jsonString = JsonConvert.SerializeObject( session );	
		}	

		//Fix This !!
		public void EndSession( int levelCount , int levelsPerStage )
		{
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
