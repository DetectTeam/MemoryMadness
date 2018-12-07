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

		public void CreateSession()
		{
			SessionID ++;
			trialNumber ++;
			Debug.Log( "Starting new Session...." + SessionID );
			
			Debug.Log(  System.DateTime.Now.ToString( "yyyy_MM_dd_hh_mm_ss" ) );
			
			session = new Session();

			session.UserID = "DummyID0001";
			session.SessionName = "Session Name";
			session.SessionTimeStamp =  System.DateTime.Now.ToString( "yyyy_MM_dd_hh_mm_ss" );
			session.Stage = StageManager.Instance.CurrentStage;
			session.Level = StageManager.Instance.CurrentLevel + 1;
			session.SymbolArraySize = CalculateSymbolArraySize( StageManager.Instance.CurrentStage );
			session.TrialNumber = trialNumber;
			session.DistractorCount = levelSize - session.SymbolArraySize;

			SetStudyItems( RandomLevelGenerator.Instance.MemoryPhaseSymbols );
			
			// if( PlayerPrefs.HasKey( "CurrentStage" ) )
			// 	session.Stage = PlayerPrefs.GetInt( "CurrentStage" );
			// else
			// 	session.Stage = 1;

			Debug.Log( "Current Stage: " +  session.Stage );
			Debug.Log( "Current Level: " +  session.Level );
			Debug.Log( "Symbol Array Size: " + session.SymbolArraySize );
			Debug.Log( "Trial Number: " + session.TrialNumber );
			

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


		[SerializeField] private StudyItem item;
		private void SetStudyItems( List<Symbol> source )
		{
			
			for( int i = 0; i < source.Count; i++ )
			{
				//StudyItem item = new StudyItem(); 
				// Debug.Log( "COLOR: " + source[i].BackgroundColor.ToString() );
				// Debug.Log( source[i].Name );
				// item.Colour = source[i].BackgroundColor.ToString();
				// item.Shape = source[i].Name;

				Debug.Log( "Colour Code : " +source[i].BackgroundColor.ColourCode );

			}
			
		}


		public void EndSession()
		{
			Debug.Log( "Ending Session....." + SessionID );
			if( trialNumber >= 32 )
				trialNumber = 0;
		}	
	}
}
