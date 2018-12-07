using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MemoryMadness
{
	public class SessionManager : MonoBehaviour 
	{
		public static int SessionID;
		public static SessionManager Instance = null;
		[SerializeField] private int trialNumber = 0;
		[SerializeField] private int levelSize = 20;
		[SerializeField] private Session session;
	
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
		}

		private void OnDisable()
		{
			Messenger.RemoveListener<int , int>( "AccuracyUpdate" , SetAccuracySlot );
		}

		public void CreateSession()
		{
			SessionID ++;
			trialNumber ++;
			Debug.Log( "Starting new Session...." + SessionID );
				
			session = new Session();

			session.UserID = "DummyID0001";
			session.SessionName = "Session Name";
			session.SessionTimeStamp =  System.DateTime.Now.ToString( "yyyy_MM_dd_hh_mm_ss" );
			session.Stage = StageManager.Instance.CurrentStage;
			session.Level = StageManager.Instance.CurrentLevel + 1;
			session.SymbolArraySize = CalculateSymbolArraySize( StageManager.Instance.CurrentStage );
			session.TrialNumber = trialNumber;
			session.DistractorCount = levelSize - session.SymbolArraySize;

			SetStudyItems( session, RandomLevelGenerator.Instance.MemoryPhaseSymbols );
			SetTestSlotItems();
			
			
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

				Debug.Log( "Slot Colour Code: " + slot.ColourCode );
				Debug.Log( "Slot Shape Code: " + slot.ShapeCode );
				Debug.Log( "Slot type : " + slot.Type );

				session.TestSlots.Add( slot );
			}
		}

		private void SetAccuracySlot( int slot , int slotStatus )
		{
			
			Debug.Log( ">>>>> " + slot + " >>>>> " + slotStatus  );
			session.AccuracySlots[ slot - 1 ] = slotStatus;

			if( slotStatus == 1 ) //Correct
				session.SumAccuracy ++;
			else if( slotStatus == 2 ) //Lure Error
				session.LureErrors ++;
			else  if( slotStatus == 3 ) //Normal Error
				session.NormalErrors ++;	
		}

		public void EndSession()
		{
			Debug.Log( "Ending Session....." + SessionID );
			if( trialNumber >= 32 )
				trialNumber = 0;
		}	
	}
}
