using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


namespace MemoryMadness
{
	
	public class GameManager : MonoBehaviour //Singleton<GameManager>
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

		[SerializeField] private TextMeshProUGUI userIdText;
		public int CurrentLevel { get{ return currentLevel; } set{ currentLevel = value; }  }

		private void OnEnable()
		{
			Messenger.AddListener<int>( "SetWinCount", SetWinCount );
			Messenger.MarkAsPermanent( "SetWinCount" );

			Messenger.AddListener( "CorrectButtonClick", IncrementCorrectButtonClickCount );
			Messenger.MarkAsPermanent( "CorrectButtonClick" );

			Messenger.AddListener( "ResetCorrectButtonCount", ResetCorrectButtonClickCount );
			Messenger.MarkAsPermanent( "ResetCorrectButtonCount");

			Messenger.AddListener( "ResetLevelGenerator", ResetRandomLevelGenerator );
			Messenger.MarkAsPermanent( "ResetLevelGenerator");

			Messenger.AddListener( "DisableRandomLevelGenerator", DisableRandomLevelGenerator );
			Messenger.MarkAsPermanent( "DisableRandomLevelGenerator");

			Messenger.AddListener( "ResetSDGenerator" , ResetSDLevelGenerator );
			Messenger.MarkAsPermanent( "ResetSDGenerator" );

			Messenger.AddListener( "DecrementLife" , DecrementLifeCount );
			Messenger.MarkAsPermanent( "DecrementLife"  );

			Messenger.AddListener( "ChangeLevel", ChangeLevel );
			Messenger.MarkAsPermanent( "ChangeLevel" );
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

		private void Awake()
		{
			//DontDestroyOnLoad(gameObject);
			 //Check if instance already exists
             if (Instance == null)
                //if not, set instance to this
                 Instance = this;
             else if (Instance != this)
                Destroy(gameObject);    
            
     
             //DontDestroyOnLoad(gameObject);
		}

		// Use this for initialization
		void Start () 
		{
			
			userIdText.text = IDGenerator.Instance.UserID;
			
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
			else if( currentStage == 2 )
			{
				lifeCount = 3;
			}
			else if( currentStage > 2 )
			{
				lifeCount = 4;
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

		// void OnApplicationFocus( bool hasFocus )
    	// {
        // 	Debug.Log( "Is Focused ? + " + hasFocus );
    	// }

		private void OnApplicationQuit()
    	{
        	Debug.Log("Application ended after " + Time.time + " seconds");
			
			//Set Application Ended Field 

			//Save  Session 


    	}
	}	
}