using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace  MemoryMadness
{
	public class MemorySymbolsTutorial : MonoBehaviour 
	{
		[SerializeField] private bool isTutorial;
		
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

		[SerializeField] private GameObject successColor;

		//Symbol Background colour
		[SerializeField] private GameObject backgroundColor;
		public GameObject BackgroundColor { get{ return backgroundColor; } set{ backgroundColor = value; } }

		//Button component of symbol
		[SerializeField] private GameObject button;
		public GameObject Button { get{ return button; } set{ button = value; } }

		[SerializeField] private bool interactive = true;
		public bool Interactive { get{ return interactive; } set{ interactive = value; }  }

		[SerializeField] private GameObject symbolHighlight;

		//Symbols rune or symbol
		[SerializeField] private GameObject rune;
		public GameObject Rune { get{ return rune; } set{ rune = value; } }


		private void OnDisable()
		{
			Reset();
		}

		private void Start () 
		{
			transform.localScale = new Vector3( 0.01f, 0.01f, 0.01f );
		}
	
		public void ButtonCheck()
		{	
			if( isCorrect && interactive )
			{
				PunchScale();
				successColor.SetActive( true );
				successImage.SetActive( true );

				symbolHighlight.SetActive( false );
			
				DisableButton();

				//Messenger.Broadcast( "DisplayNextSentence" );
				//Messenger.Broadcast( "IncrementDialogCount" );
				if( !isTutorial )
					Messenger.Broadcast<int>( "IncreaseScore" , 100 );
			}
			else if( !isCorrect && interactive )
			{
				ShakePosition();
				errorImage.SetActive( true );
				DisableButton();

				if( !isTutorial )
				{
					Messenger.Broadcast( "RemoveHeart" );
					Messenger.Broadcast<int>( "DecreaseScore" , 100 );
				}
			}
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

		public void EnableButton()
		{
			button.SetActive( true );  
		}
		public void DisableButton()
		{
			button.SetActive( false );
		}

		public void ToggleHighlight()
		{
			if( symbolHighlight.activeSelf )
				symbolHighlight.SetActive( false );
			else
				symbolHighlight.SetActive( true );
		}

		public void Reset()
		{
			//isCorrect = false;
			successColor.SetActive( false );
			successImage.SetActive( false ); 
			errorImage.SetActive( false );
			
			backgroundColor.SetActive( true );
			rune.SetActive( true );
			//EnableButton();
		}
	}
}
