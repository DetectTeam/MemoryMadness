using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryMadness
{

	public enum GameType
	{
		SameDifferent,
		MulitMatch
	}


	[System.Serializable]
	public struct Level
	{
		
		[SerializeField] private string name;
		public string Name { get{ return name; } set{  name = value; } }
		[SerializeField] private GameType gameType;
		public GameType GameType { get{ return gameType; } set{  gameType = value; } }
		[SerializeField] private int winCount;
		public int WinCount { get{ return winCount; } set{  winCount = value; } }
		
		[SerializeField] private GameObject memoryPhase;
		public GameObject MemoryPhase { get{ return memoryPhase; } set{ memoryPhase = value; } }
		
		[SerializeField] private GameObject level;
		public GameObject LevelObj { get{ return level; } set{  level = value; } }

		[SerializeField] private bool isMatch;
		public bool IsMatch { get{ return isMatch; } set{  isMatch = value; } }



	}

	[System.Serializable]
	public struct Stage
	{
		[SerializeField] private string name;
		public string Name { get{ return name; } set{  name = value; } }
		[SerializeField] private Level[] level;
		public Level[] Level { get{ return level; } set{  level = value; } }
	}


	public class BoardManager : MonoBehaviour 
	{
		[SerializeField] private GameObject randomLevelGenerator;
		[SerializeField] private GameObject memoryPhaseScreen;
		[SerializeField] private int selectedButtonCount;
		[SerializeField] private float delay;
		[SerializeField] private Stage[] stages;
		//[SerializeField] private  Level[] levels;
		[SerializeField] private GameObject successMessage;
		[SerializeField] private GameObject failureMessage;
		[SerializeField] private GameObject endLevelBackground;
		[SerializeField] private GameObject resultPanel;
		[SerializeField] private float percentage = 65;
		[SerializeField] private int currentPhase = 0;




		[SerializeField] private int winCount;

		public int WinCount { get{ return winCount; } set{ winCount = value; } }

		public float Percentage { get{ return percentage; } set{ percentage = value; } }

		private int levelToLoad = 0; 
		private	int currentStage  = 0;

		[SerializeField] private int lifeCount;


		private void OnEnable()
		{
			//Messenger.AddListener( "LoadPhase" , LoadPhase );
			Messenger.AddListener( "LoadLevel" , LoadLevel );
			Messenger.AddListener( "IncrementButtonCount", IncrementButtonCount );
			Messenger.AddListener( "DecrementLife" , DecrementLifeCount );
			Messenger.AddListener( "CheckForWin", CheckWinStatus );
			Messenger.AddListener( "Failure", Failure );
			Messenger.AddListener( "ChangeLevel", ChangeLevel );
			Messenger.AddListener( "ResetLevelGenerator" , ResetLevelGenerator );
			//StartCoroutine( LoadLevel() );

			
		}

		private void OnDisable()
		{
			//Messenger.RemoveListener( "LoadPhase" , LoadPhase );
			Messenger.RemoveListener( "LoadLevel" , LoadLevel );
			Messenger.RemoveListener( "IncrementButtonCount", IncrementButtonCount );
			Messenger.RemoveListener( "DecrementLife" , DecrementLifeCount );
			Messenger.RemoveListener( "CheckForWin", CheckWinStatus );
			Messenger.RemoveListener( "Failure", Failure );
			Messenger.RemoveListener( "ChangeLevel", ChangeLevel );
		}

		// Use this for initialization
		private void Start () 
		{
			//Check which level to load
			if( PlayerPrefs.HasKey( "CurrentLevel" ) )
			{
				levelToLoad = PlayerPrefs.GetInt( "CurrentLevel" );
			}

		}

		private void ResetLevelGenerator()
		{
			randomLevelGenerator.SetActive( false );
			randomLevelGenerator.SetActive( true );
			lifeCount = 3;
		}

		private void LoadPhase()
		{
			//selectedButtonCount = 0;
			//StartCoroutine( IELoadPhase() );
		}

		// private IEnumerator IELoadPhase()
		// {
		// 	yield return new WaitForSeconds( delay );

		// 	//Get current Stage
		// 	currentStage = PlayerPrefs.GetInt( "CurrentStage" );

		

		// 	Debug.Log( "Current Stage " + currentStage );
		
		// 	//Check the level count

		// 	if( PlayerPrefs.HasKey( "CurrentLevel" ) )
		// 	{
		// 		currentPhase = PlayerPrefs.GetInt( "CurrentLevel" );
		// 	}
		// 	else
		// 	{
		// 		PlayerPrefs.SetInt( "CurrentLevel" , currentPhase );
		// 	}
			
			
		// 	if( currentStage < stages.Length )
		// 	{
		// 		stages[ currentStage ].Level[ currentPhase ].MemoryPhase.SetActive( true );
		// 	}
			
		// 	//phases[ 0 ].SetActive( true );
		// 	//Based on level count set correct phase to active
		// }

		private void LoadLevel()
		{
			StartCoroutine( IELoadLevel() );
		}


		private IEnumerator IELoadLevel()
		{
			
			yield return new WaitForSeconds ( 0.1f );

			if( PlayerPrefs.HasKey( "CurrentStage" ) )
				currentStage = PlayerPrefs.GetInt( "CurrentStage" );

			levelToLoad = 0;

			if( PlayerPrefs.HasKey( "CurrentLevel" ) )
				levelToLoad  = PlayerPrefs.GetInt( "CurrentLevel" );
		
			//Enable the selected level
			if( levelToLoad <= ( stages[ currentStage ].Level.Length ) )
			{
				stages[ currentStage ].Level[ levelToLoad ].LevelObj.SetActive( true );

				//Broadcast Type of Game to relevant listeners...
				Messenger.Broadcast<GameType>( "GameType" , stages[ currentStage ].Level[ levelToLoad ].GameType );
			    
				if( stages[ currentStage ].Level[ levelToLoad ].GameType == GameType.MulitMatch )
					lifeCount = stages[ currentStage ].Level[ levelToLoad ].WinCount;
			}
		}


		//Check for win on the Same Different Game
		public void CheckForWinSD( bool b )
		{
			endLevelBackground.SetActive( true );
			
			if( b && stages[ currentStage ].Level[ levelToLoad ].IsMatch )
			{
				Debug.Log( "ITS A WIN !!!" );
				Success();
			}
			else if( b && !stages[ currentStage ].Level[ levelToLoad ].IsMatch )
			{
				Debug.Log( "YOU FAILED ...You chose yes but no was required" );
				Failure();
			}
			else if( !b && stages[ currentStage ].Level[ levelToLoad ].IsMatch )
			{
				Debug.Log( "YOU FAILED ....You chose no when it should have been yes" );
				Failure();
				
			}
			else if( !b && !stages[ currentStage ].Level[ levelToLoad ].IsMatch )
			{
				Debug.Log( "CORRECT THERE IS NO MATCH" );
				Success();
				
			}
			else
			{

			}

			ChangeLevel();
				
		}

		public void IncrementButtonCount()
		{	
			selectedButtonCount++;
			Debug.Log( "INCREMENT ::: " + selectedButtonCount );
		}


		public void DecrementLifeCount()
		{
			
			lifeCount --;
			Messenger.Broadcast( "RemoveHeart" );

			if( lifeCount == 0 )
			{
				
				Messenger.Broadcast( "StopCountDown" );
				endLevelBackground.SetActive( true );
				Failure(); //Request failure message
				ChangeLevel(); //Request level change

			}
		}


		public void CheckWinStatus()
		{
			if( stages[ currentStage ].Level[ levelToLoad ].GameType == GameType.SameDifferent )
			{

			}
			else
			{
				CheckForWinMM();
			}
		}

		public void CheckForWinMM( )
		{
			
			Debug.Log( "Check for win : " + selectedButtonCount + " " + stages[ currentStage ].Level[ levelToLoad ].WinCount );
			
			
			if( selectedButtonCount == stages[ currentStage ].Level[ levelToLoad ].WinCount )
			{
				Messenger.Broadcast( "StopCountDown" );
				endLevelBackground.SetActive( true );
				Success();
				ChangeLevel();
				selectedButtonCount = 0;
			}
			
		}

		private void ChangeLevel()
		{
			StartCoroutine( IEChangeLevel() );
		}
		private IEnumerator IEChangeLevel()
		{
			yield return new WaitForSeconds( 3.0f );
			
			Messenger.Broadcast( "LoadNextLevel" );
		
			//resultPanel.SetActive( true );
			
			 if( successMessage.activeSelf )
				successMessage.SetActive( false );
			
			 if( failureMessage.activeSelf )	
				failureMessage.SetActive( false );
			
			// stages[ currentStage ].Level[ currentPhase ].MemoryPhase.SetActive( false );
		}

		private void Success()
		{
			
			successMessage.SetActive( true );
		}

		private void Failure()
		{
			failureMessage.SetActive( true );
		}

		public float calculateScore()
		{
			return  100;//Random.Range( 0, 100 ); 
		}
	
	}

}
