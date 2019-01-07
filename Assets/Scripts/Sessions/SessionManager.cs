using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json; //Json Library


namespace MemoryMadness
{
	public class SessionManager : MonoBehaviour 
	{
		public static int SessionID;
		public static SessionManager Instance = null;
		[SerializeField] private int trialNumber = 0;
		[SerializeField] private int levelSize = 20;
		[SerializeField] private Session session;
		private int orderCount = 1;
	
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
		}

		private void OnEnable()
		{
			Messenger.AddListener<int , int>( "AccuracyUpdate" , SetAccuracySlot );
			Messenger.AddListener< int >( "SetSelectionOrder", SetOrderSlot );
			Messenger.AddListener< float >( "RecordTime", SetTimeSlot );
			Messenger.AddListener(  "DecrementLife", UpdateLifeCount );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener<int , int>( "AccuracyUpdate" , SetAccuracySlot );
			Messenger.RemoveListener< int >( "SetSelectionOrder", SetOrderSlot );
			Messenger.RemoveListener< float >( "RecordTime", SetTimeSlot );
			Messenger.RemoveListener(  "DecrementLife", UpdateLifeCount );
		}

		

		public void CreateSession()
		{
			SessionID ++;
			trialNumber ++;
			
			Debug.Log( "Starting new Session...." + SessionID );

			session = new Session();

			orderCount = 1;

			session.UserID = "DummyID0001";
			session.SessionName = "Session_Name";
			session.Date = System.DateTime.Now.ToString( "dd_MM_yyyy" );
			session.AbsoluteTimeOfResponse = System.DateTime.Now.ToString( "hh_mm" );
			session.SessionTimeStamp =  System.DateTime.Now.ToString( "yyyy_MM_dd_hh_mm_ss" );
			session.Stage = StageManager.Instance.CurrentStage;
			session.Level = StageManager.Instance.CurrentLevel + 1;
			session.SymbolArraySize = CalculateSymbolArraySize( StageManager.Instance.CurrentStage );
			session.TrialNumber = trialNumber;
			session.DistractorCount = levelSize - session.SymbolArraySize;
			
			if( !PlayerPrefs.HasKey( "CurrentStage" ) )
				session.TotalLives = 2;
			else 
				session.TotalLives = PlayerPrefs.GetInt( "CurrentStage" );
			
			session.LivesLost = 0;

			SetStudyItems( session, RandomLevelGenerator.Instance.MemoryPhaseSymbols );
			SetTestSlotItems();

			//PrepOrderSlot();

			if( StageManager.Instance.CurrentLevelType == LevelType.NameableColour ||
				StageManager.Instance.CurrentLevelType == LevelType.UnNameableColour )
			{ session.Condition = "Binding"; }
			else
			{ session.Condition = "Shape"; }

			if( StageManager.Instance.CurrentLevelType == LevelType.UnNameableColour ||
				StageManager.Instance.CurrentLevelType == LevelType.UnNameableNonColour )
			{ session.ShapeType = "Abstract"; }
			else
			{ session.ShapeType = "Nameable"; }	

			Debug.Log( "Condition: " + session.Condition );
			Debug.Log( "Shape Type: " + session.ShapeType );
			Debug.Log( "Num of Distractors" + session.DistractorCount );

			PersistenceManager.Instance.Test();

			PersistenceManager.Instance.FileName = session.SessionName + ".dat";
			//session.FileName = session.SessionName + ".dat";
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
				
			 	item.ColourCode = source[i].BackgroundColor.ColourCode;
			 	item.ShapeCode = source[i].CurrentShape.ShapeCode;

				session.StudyItems.Add( item );
			}	

			Debug.Log( session.StudyItems.Count );
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
				
				if( memSymbolsScript.IsCorrect )
					slot.Type = 1;
				else if( memSymbolsScript.IsColourSwitched )
					slot.Type = 2;
				else
					slot.Type = 2;
			
				slot.StudyOrder = "";

				//Debug.Log( "Slot Colour Code: " + slot.ColourCode );
				//Debug.Log( "Slot Shape Code: " + slot.ShapeCode );
				//Debug.Log( "Slot type : " + slot.Type );

				session.TestSlots.Add( slot );
			}
		}

		private void SetAccuracySlot( int slot , int slotStatus )
		{
			session.AccuracySlots[ slot - 1 ] = slotStatus;

			if( slotStatus == 1 ) //Correct
				session.SumAccuracy ++;
			else if( slotStatus == 2 ) //Lure Error
				session.LureErrors ++;
			else  if( slotStatus == 3 ) //Normal Error
				session.NormalErrors ++;	
		}

        

		 private void PrepOrderSlot()
		 {
			 for( int x = 0; x < session.OrderSlot.Length; x++ )
			 {
				 session.OrderSlot[ x ] = 0;
			 }
		 }
		private void SetOrderSlot( int slot )
		{
			if( orderCount <= 5 )
			{
				session.OrderSlot[ slot - 1 ] = orderCount;
				orderCount ++;
			}
			else
			{
				orderCount = 0;
			} 
		}

		private void SetTimeSlot( float time )
		{
			Debug.Log( "SetTimeSlot" + time );
			//session.TimeSlot[ slot - 1 ] = time;
			session.RelativeTime = session.RelativeTime + time;
			session.ReactionTime = time;

			Debug.Log( "Relative Time: " + session.RelativeTime );
			Debug.Log( "Reaction Time: " + session.ReactionTime );

		}

		public void EndSession()
		{
			Debug.Log( "Ending Session....." + session.LivesLost );


			string jsonString = JsonConvert.SerializeObject( session );

			//Debug.Log( "Session: " + jsonString );

			Messenger.Broadcast<string>( "PUT" , jsonString );
			PersistenceManager.Instance.Save( session );

			if( trialNumber >= 32 )
				trialNumber = 0;
		}	


		public void UpdateLifeCount()
		{
			Debug.Log( "UPDATE LIFE COUNT CALLED" );
			session.LivesLost ++;
		}
	}
}
