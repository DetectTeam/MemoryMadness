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

		// Use this for initialization
		void Start () 
		{
			StartCoroutine( "RunGameLoop" );
		}


		private IEnumerator RunGameLoop()
		{
			yield return StartCoroutine( "IntroRoutine" );
			yield return StartCoroutine( "StartGameRoutine" );
			yield return StartCoroutine( "PlayGameRoutine" );
			yield return StartCoroutine( "EndGameRoutine" );
		}

		private IEnumerator IntroRoutine()
		{
			//Activate intro screen
			yield return null;
		}

		private IEnumerator StartGameRoutine()
		{
			//Start the game loop by triggering the random level generator
			//then enable the Memory phase.
			yield return null;
		}

		private IEnumerator PlayGameRoutine()
		{
			yield return null;
		}

		private IEnumerator EndGameRoutine()
		{
			yield return null;
		}
		
		
	}

}