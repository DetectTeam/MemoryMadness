using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace MemoryMadness
{
	
	public class GameManager : MonoBehaviour 
	{
		public static GameManager Instance = null;
		public UnityEvent setupEvent;
		public UnityEvent startGameEvent;
		public UnityEvent playingGameEvent;
		public UnityEvent endingGameEvent;

		[SerializeField] private int lifeCount;
		[SerializeField] private int winCount;
		[SerializeField] private int correctButtonCount;
		[SerializeField] private GameObject randomLevelGenerator;
		[SerializeField] private GameObject sameDifferentLevelGenerator;
		[SerializeField] private int currentLevel = 1;
		public int CurrentLevel { get{ return currentLevel; } set{ currentLevel = value; }  }

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
			Messenger.AddListener<int>( "SetWinCount", SetWinCount );
			Messenger.AddListener( "CorrectButtonClick", IncrementCorrectButtonClickCount );
			Messenger.AddListener( "ResetCorrectButtonCount", ResetCorrectButtonClickCount );
			Messenger.AddListener( "ResetLevelGenerator", ResetRandomLevelGenerator );
			Messenger.AddListener( "DisableRandomLevelGenerator", DisableRandomLevelGenerator );
			Messenger.AddListener( "ResetSDGenerator" , ResetSDLevelGenerator );
			Messenger.AddListener( "DecrementLife" , DecrementLifeCount );
			Messenger.AddListener( "ChangeLevel", ChangeLevel );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener<int>( "SetWinCount", SetWinCount );
			Messenger.RemoveListener( "CorrectButtonClick", IncrementCorrectButtonClickCount );
			Messenger.RemoveListener( "ResetCorrectButtonCount", ResetCorrectButtonClickCount );
			Messenger.RemoveListener( "ResetLevelGenerator" , ResetRandomLevelGenerator );
			Messenger.RemoveListener( "DisableRandomLevelGenerator", DisableRandomLevelGenerator );
			Messenger.RemoveListener( "ResetSDGenerator" , ResetSDLevelGenerator );
			Messenger.RemoveListener( "DecrementLife" , DecrementLifeCount );
			Messenger.RemoveListener( "ChangeLevel", ChangeLevel );
		}

		// Use this for initialization
		void Start () 
		{
			ResetLifeCount();
		}

		private void SetWinCount( int max)
		{
			winCount = max;
		}

		private void CheckForWin()
		{
			if( correctButtonCount == winCount )
			{
				Messenger.Broadcast( "StopCountDown" );
			
				Messenger.Broadcast( "Success" );
				
				ResetCorrectButtonClickCount();
				
				ChangeLevel();	
			}	
		}

	    private void IncrementCorrectButtonClickCount()
		{	
			correctButtonCount++;
		
			CheckForWin();
		}

		private void ResetCorrectButtonClickCount()
		{
			correctButtonCount = 0;
		}

		public void DecrementLifeCount()
		{
			lifeCount --;
			Messenger.Broadcast( "RemoveHeart" );

			if( lifeCount == 0 )
			{
				Messenger.Broadcast( "StopCountDown" );
				Messenger.Broadcast( "Failure" );
				ChangeLevel(); 
			}
		}

		private void ResetLifeCount()
		{
			int currentStage = 1;

			if( PlayerPrefs.HasKey( "CurrentStage" ) )
			{
				currentStage = PlayerPrefs.GetInt( "CurrentStage" );
			}

			if( currentStage == 1 )
			{
				lifeCount = 2;
			}
			else if( currentStage >= 4 )
			{
				lifeCount = 4;
			}
			else
			{
				lifeCount = currentStage;
			}

		}

		private void ChangeLevel()
		{
			Messenger.Broadcast( "IncrementLevel" );
			StartCoroutine( IEChangeLevel() );
		}

		private IEnumerator IEChangeLevel()
		{
			yield return new WaitForSeconds( 3.0f );
			
			Messenger.Broadcast( "LoadNextLevel" );
			Messenger.Broadcast( "ResetMessage" );
			
			ResetLifeCount();
		}

		private void ResetRandomLevelGenerator()
		{
			randomLevelGenerator.SetActive( false );
			randomLevelGenerator.SetActive( true );
		}

		//Reset the Same different level Generator
		private void ResetSDLevelGenerator()
		{
			sameDifferentLevelGenerator.SetActive( false );
			sameDifferentLevelGenerator.SetActive( true );
		}

		private void DisableRandomLevelGenerator()
		{
			randomLevelGenerator.SetActive( false );
		}	

		private void OnApplicationPause( bool pauseStatus )
    	{
			Debug.Log( "The game is paused ?! " + pauseStatus );
   		}


		private void OnApplicationQuit()
    	{
        	Debug.Log("Application ending after " + Time.time + " seconds");
    	}
	}	
}