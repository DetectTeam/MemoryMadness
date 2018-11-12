using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryMadness
{
	public class MemorySymbols : MonoBehaviour 
	{

		//Boolean used to set whether a symbol is correct or not
		[SerializeField] private bool isCorrect;
		public bool IsCorrect { get{ return isCorrect; } set{ isCorrect = value; } }

		//ITween shake amount
		[SerializeField] private Vector3 shakeAmount;
		public Vector3 ShakeAmount { get{ return shakeAmount; } set{ shakeAmount = value; } }

		//Itween shake duration
		[SerializeField] private float shakeTime;
		public float ShakeTime { get{ return shakeTime; } set{ shakeTime = value; } }

		//Error Image displayed when user clicks on symbol and isCorrect is false
		[SerializeField] private GameObject errorImage;
		public GameObject ErrorImage { get{ return errorImage; } set{ errorImage = value; } }

		//Success Image displayed when user clicks on symbol and isCorrect is true
		[SerializeField] private GameObject successImage;
		public GameObject SuccessImage { get{ return successImage; } set{ successImage = value; } }

		//Symbol Background colour
		[SerializeField] private GameObject backgroundColor;
		public GameObject BackgroundColor { get{ return backgroundColor; } set{ backgroundColor = value; } }

		//Symbols rune or symbol
		[SerializeField] private GameObject rune;
		public GameObject Rune { get{ return rune; } set{ rune = value; } }

		//Button component of symbol
		[SerializeField] private GameObject button;
		public GameObject Button { get{ return button; } set{ button = value; } }

		//The symbols position on the game grid . A value between 1 - 20 usually
		[SerializeField] private int slotNumber;
		public int SlotNumber{ get{ return slotNumber; } set{ slotNumber = value; } }

		// Use this for initialization
		
		void OnEnable()
		{
			transform.localScale = new Vector3( 0.01f, 0.01f, 0.01f );
		}

		void OnDisable()
		{
			Reset();
		}
		
		void Start () 
		{
			transform.localScale = new Vector3( 0.01f, 0.01f, 0.01f );
		}


		public void ButtonCheck()
		{
			if( isCorrect )
			{
				PunchScale();
				successImage.SetActive( true );
				backgroundColor.SetActive( false );
				rune.SetActive( false );
				button.SetActive( false );

				Messenger.Broadcast( "IncrementButtonCount" ); //Request to Increment the correct selection count
				Messenger.Broadcast( "CheckForWin" );  //Request to check for a win 
				Messenger.Broadcast<int>( "IncreaseScore" , 100 );
				
			}
			else
			{
				ShakePosition();
				errorImage.SetActive( true );
				button.SetActive( false );
				Messenger.Broadcast( "DecrementLife" );
				Messenger.Broadcast<int>( "DecreaseScore" , 100 );
			
			}

			Messenger.Broadcast( "TriggerEffect" ); //ITween PunchScale effect . Used on score text
		}


		private void PunchScale()
		{
			iTween.PunchScale ( gameObject, iTween.Hash (
				"amount", new Vector3( -1.5f, -1.5f, -1.5f ),
				"time", 1.0f
			));
		}

		private void ShakePosition()
		{
			iTween.ShakePosition( gameObject, shakeAmount, shakeTime );
		}


		private void Reset()
		{
			successImage.SetActive( false ); 
			errorImage.SetActive( false );
			backgroundColor.SetActive( true );
			rune.SetActive( true );
			button.SetActive( true );
		}
		
		
	}

}
