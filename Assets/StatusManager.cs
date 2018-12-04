using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryMadness
{

	public class StatusManager : MonoBehaviour 
	{
		[SerializeField] private GameObject success;
		[SerializeField] private GameObject failure;
		[SerializeField] private GameObject timeOut;

		// Use this for initialization
		
		
		private void OnEnable()
		{
			Messenger.AddListener( "Success", Success );
			Messenger.AddListener( "Failure", Failure );
			Messenger.AddListener( "Timeout", TimeOut );
			Messenger.AddListener( "ResetMessage", ResetMessages );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( "Success", Success );
			Messenger.RemoveListener( "Failure", Failure );
			Messenger.RemoveListener( "Timeout", TimeOut );
			Messenger.RemoveListener( "ResetMessage", ResetMessages );
		}
		
		private void Start () {
			
			ResetMessages();

		}

		private void ResetMessages()
		{
			if( success.activeSelf )
				success.SetActive( false );
			
			if( failure.activeSelf )
				failure.SetActive( false );
			
			if( timeOut.activeSelf )
				timeOut.SetActive( false );
		}

		private void Success()
		{
			success.SetActive( true );
		}

		private void Failure()
		{
			failure.SetActive( true );
		}

		private void TimeOut()
		{
			timeOut.SetActive( true );
		}
		
		
	}
}
