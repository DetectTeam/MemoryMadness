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
		[SerializeField] private GameObject background;

		// Use this for initialization

		[SerializeField] private bool isMessageActive = false;
		
		
		private void OnEnable()
		{
			Messenger.AddListener( "Success", Success );
			Messenger.AddListener( "Failure", Failure );
			Messenger.AddListener( "Timeout", TimeOut );
			Messenger.AddListener( "ResetMessage", ResetMessages );

			Messenger.MarkAsPermanent( "Success" );
			Messenger.MarkAsPermanent( "Failure" );
			Messenger.MarkAsPermanent( "Timeout" );
			Messenger.MarkAsPermanent( "ResetMessage" );

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

			if( background.activeSelf )
				background.SetActive( false );

			isMessageActive = false;	
		}

		private void Success()
		{
			if( !isMessageActive )
			{
				success.SetActive( true );
				EnableMessageBackground();
				isMessageActive = true;
			}
		}

		private void Failure()
		{
			if( !isMessageActive )
			{
				failure.SetActive( true );
				EnableMessageBackground();
				isMessageActive = true;
			}
		}

		private void TimeOut()
		{
			if( !isMessageActive )
			{
				timeOut.SetActive( true );
				EnableMessageBackground();
				isMessageActive = true;
			}
		}

		private void EnableMessageBackground()
		{
			background.SetActive( true );
		}
		
		
	}
}
