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
		[SerializeField] private GameObject endLevelBackground;
		[SerializeField] private GameObject successMessage;
		[SerializeField] private GameObject failureMessage;
		[SerializeField] private GameObject randomLevelGenerator;

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
			Messenger.AddListener( "ResetLevelGenerator" , ResetLevelGenerator );
			Messenger.AddListener( "DecrementLife" , DecrementLifeCount );
			Messenger.AddListener( "ChangeLevel", ChangeLevel );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener<int>( "SetWinCount", SetWinCount );
			Messenger.RemoveListener( "CorrectButtonClick", IncrementCorrectButtonClickCount );
			Messenger.RemoveListener( "ResetCorrectButtonCount", ResetCorrectButtonClickCount );
			Messenger.AddListener( "ResetLevelGenerator" , ResetLevelGenerator );
			Messenger.RemoveListener( "DecrementLife" , DecrementLifeCount );
			Messenger.RemoveListener( "ChangeLevel", ChangeLevel );
		
		}

		// Use this for initialization
		void Start () 
		{
			//StartCoroutine( "RunGameLoop" );
		}

		private void SetWinCount( int max)
		{
			winCount = max;
			Debug.Log( "WINCOUNT : " + winCount );
			
		}

		private void CheckForWin()
		{
			if( correctButtonCount == winCount )
			{
				Debug.Log( "You WIN !!!!!" );
				Messenger.Broadcast( "StopCountDown" );
				endLevelBackground.SetActive( true );
				Success();
				ResetCorrectButtonClickCount();
				ChangeLevel();
				
			}
			
		}

	    private void IncrementCorrectButtonClickCount()
		{	
			correctButtonCount++;
			//Debug.Log( "INCREMENT ::: " + selectedButtonCount );
			CheckForWin();
		}

		private void ResetCorrectButtonClickCount()
		{
			correctButtonCount = 0;
		}

		private void Success()
		{
			if( successMessage.activeSelf )
				successMessage.SetActive( false );
			else
				successMessage.SetActive( true );
		}

		private void Failure()
		{
			if( failureMessage.activeSelf )
				failureMessage.SetActive( false );
			else
				failureMessage.SetActive( true );
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

		private void ResetLifeCount()
		{
			lifeCount = 3;
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
			
			 ResetLifeCount();
		
		}

		private void ResetLevelGenerator()
		{
			randomLevelGenerator.SetActive( false );
			randomLevelGenerator.SetActive( true );
			lifeCount = 3;
		}

		

		
		// private IEnumerator RunGameLoop()
		// {
		// 	yield return StartCoroutine( "IntroRoutine" );
		// 	yield return StartCoroutine( "StartGameRoutine" );
		// 	yield return StartCoroutine( "PlayGameRoutine" );
		// 	yield return StartCoroutine( "EndGameRoutine" );
		// }

		// private IEnumerator IntroRoutine()
		// {
		// 	//Activate intro screen
		// 	yield return null;
		// }

		// private IEnumerator StartGameRoutine()
		// {
		// 	//Start the game loop by triggering the random level generator
		// 	//then enable the Memory phase.
		// 	yield return null;
		// }

		// private IEnumerator PlayGameRoutine()
		// {
		// 	yield return null;
		// }

		// private IEnumerator EndGameRoutine()
		// {
		// 	yield return null;
		// }
		
		
	}

	
}