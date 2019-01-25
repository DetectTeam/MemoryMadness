using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MemoryMadness
{

	public class CountDown : MonoBehaviour 
	{
		[SerializeField] private float totalTime = 30;
		[SerializeField] private float timeLeft;
		public float TimeLeft { get{ return timeLeft; } }


		[SerializeField] private TextMeshProUGUI timer;

		[SerializeField] private GameObject game;
		// Use this for initialization

		[SerializeField] private bool isFinished = false;
		[SerializeField] private bool isLevelTimeOut = false;

		private float previousTime;

		private float tmpTime;

		private void OnEnable()
		{
			Messenger.AddListener( "ResetTimer", ResetTimer );
			Messenger.AddListener( "StopCountDown", StopTimer );
			previousTime = timeLeft /10;
			timer.text = (timeLeft / 10).ToString();
			StartCoroutine( CountDownSequence() );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( "ResetTimer", ResetTimer );
			Messenger.RemoveListener( "StopCountDown", StopTimer );
			timeLeft = tmpTime;
		}


		private void Awake()
		{
			
		}

		private void Start () 
		{
			tmpTime = timeLeft;
		}

		private IEnumerator CountDownSequence()
		{
			int count = 0;
	
			yield return new WaitForSeconds( 1.0f );
			
			while( timeLeft > 0 && !isFinished )
			{
				yield return new WaitForSeconds( 0.1f );
				count ++;
				timeLeft --;
				
				if( count == 10 )
				{
					timer.text = (timeLeft / 10).ToString();
					count = 0;
				}
			}
			
			yield return new WaitForSeconds( 1.0f );
			
			if( !isFinished )
			{
				if( isLevelTimeOut )
				{
					Messenger.Broadcast( "Timeout" );
					Messenger.Broadcast( "ChangeLevel" );		
				}
				else
				{
					game.SetActive( true );
				}
			}

			isFinished = false;
		}

		public void StopTimer()
		{
			isFinished = true;
			StopCoroutine( CountDownSequence() );
		}

		public void ResetTimer()
		{
			timeLeft = totalTime;
			timer.text = ( timeLeft / 10 ).ToString();
			StartCoroutine( "CountDownSequence" );
		}

		public float CalculateTimeElapsed()
		{
			float currentTime = timeLeft / 10;

			//Debug.Log( "Previous Time: " + previousTime );
			//Debug.Log( "Current Time: " + currentTime  );
			float timeElapsed = previousTime - currentTime;
			previousTime = ( timeLeft / 10 );

			return timeElapsed;
		}
	}
}
