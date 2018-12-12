using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryMadness
{


	public class MemorySymbols : MonoBehaviour 
	{
		[SerializeField] private string name;
		public string Name { get{ return name; } set{ name = value; } }

		[SerializeField] private int colourCode;
		public int ColourCode { get{ return colourCode; } set{ colourCode = value; } }

		[SerializeField] private int shapeCode;
		public int ShapeCode { get{ return shapeCode; } set{ shapeCode = value; } }

		//Boolean used to set whether a symbol is correct or not
		[SerializeField] private bool isCorrect;
		public bool IsCorrect { get{ return isCorrect; } set{ isCorrect = value; } }

		[SerializeField] private bool isColourSwitched;
		public bool IsColourSwitched { get{ return isColourSwitched; } set{ isColourSwitched = value; } }

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

		[SerializeField] private GameObject successColor;

		//Symbol Background colour
		[SerializeField] private GameObject backgroundColor;
		public GameObject BackgroundColor { get{ return backgroundColor; } set{ backgroundColor = value; } }

		//Symbols rune or symbol
		[SerializeField] private GameObject rune;
		public GameObject Rune { get{ return rune; } set{ rune = value; } }

		//Button component of symbol
		[SerializeField] private GameObject button;
		public GameObject Button { get{ return button; } set{ button = value; } }

		[SerializeField] private GameObject timer;


		//The symbols position on the game grid . A value between 1 - 20 usually
		[SerializeField] private int slotNumber;
		public int SlotNumber{ get{ return slotNumber; } set{ slotNumber = value; } }

		private float previousTime = 0;

		void OnEnable()
		{
			transform.localScale = new Vector3( 0.01f, 0.01f, 0.01f );

			timer = GameObject.Find( "Timer" );

			if( timer )
			{
				
				Debug.Log( "Timer Found...." );
			}
			else
			{
				Debug.Log( "Timer Game Object Not Found" );
			}
				
		}

		void OnDisable()
		{
			//Reset();
			previousTime = 0;
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
				successColor.SetActive( true );
				successImage.SetActive( true );
			
				DisableButton();

				Messenger.Broadcast<int>( "IncreaseScore" , 100 );
				Messenger.Broadcast( "CorrectButtonClick" ); //Let the game manager know that the correct button was clicked
				//Messenger.Broadcast( "CheckForWin" );  //Request to check for a win 
				Messenger.Broadcast<int , int>( "AccuracyUpdate" , slotNumber , 1 );

				
			}
			else
			{
				ShakePosition();
				errorImage.SetActive( true );
				DisableButton();
				Messenger.Broadcast( "DecrementLife" );
				Messenger.Broadcast<int>( "DecreaseScore" , 100 );
			

				if( isColourSwitched )
					Messenger.Broadcast<int , int>( "AccuracyUpdate" , slotNumber , 2 ); //Binding Error
				else
					Messenger.Broadcast<int , int>( "AccuracyUpdate" , slotNumber , 3 ); //Normal Error
			}

			Messenger.Broadcast<int>( "SetSelectionOrder" , slotNumber );
				
			Messenger.Broadcast< int, float  >( "RecordTime", slotNumber , RecordTimeBetweenButtonPress() );

			Messenger.Broadcast( "TriggerEffect" ); //ITween PunchScale effect . Used on score text
		}


		private void PunchScale()
		{
			iTween.PunchScale ( gameObject, iTween.Hash (
				"amount", new Vector3( -1.5f, -1.5f, -1.5f ),
				"time", 1.0f
			));
		}

		private float RecordTimeBetweenButtonPress()
		{	
			return timer.GetComponent<CountDown>().CalculateTimeElapsed();
		}


		private void ShakePosition()
		{
			iTween.ShakePosition( gameObject, shakeAmount, shakeTime );
		}

		public void EnableButton()
		{
			button.SetActive( true );  
		}
		public void DisableButton()
		{
			button.SetActive( false );
		}
		public void Reset()
		{
			isCorrect = false;
			successColor.SetActive( false );
			successImage.SetActive( false ); 
			errorImage.SetActive( false );
			
			backgroundColor.SetActive( true );
			rune.SetActive( true );
			EnableButton();
		}
	
	}

}
