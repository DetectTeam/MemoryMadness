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
		[SerializeField] private TextMeshProUGUI timer;

		[SerializeField] private GameObject game;
		// Use this for initialization

		[SerializeField] private bool isFinished = false;
		[SerializeField] private bool isLevelTimeOut = false;

		private float tmpTime;

		private void OnEnable()
		{
			Messenger.AddListener( "ResetTimer", ResetTimer );
			Messenger.AddListener( "StopCountDown", StopTimer );

			timer.text = timeLeft.ToString();
			StartCoroutine( CountDownSequence() );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( "ResetTimer", ResetTimer );
			Messenger.RemoveListener( "StopCountDown", StopTimer );
			timeLeft = tmpTime;
		}

		private void Start () 
		{
			tmpTime = timeLeft;
		}

		private IEnumerator CountDownSequence()
		{
			Debug.Log( "Starting Countdown Sequence..." );
			yield return new WaitForSeconds( 1.0f );

			while( timeLeft > 0 && !isFinished )
			{
				yield return new WaitForSeconds( 1.0f );
				timeLeft --;
				timer.text = timeLeft.ToString();
			}

			yield return new WaitForSeconds( 1.0f );
			
			if( !isFinished )
			{
				
				if( isLevelTimeOut )
				{
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
			Debug.Log( "Resetting Timer" + totalTime );
			timeLeft = totalTime;
			timer.text = timeLeft.ToString();
			StartCoroutine( "CountDownSequence" );

		}
	}
}
