﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace  MemoryMadness
{
	public class MemorySymbolsTutorial : MonoBehaviour 
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

		[SerializeField] private GameObject successColor;

		//Symbol Background colour
		[SerializeField] private GameObject backgroundColor;
		public GameObject BackgroundColor { get{ return backgroundColor; } set{ backgroundColor = value; } }

		//Button component of symbol
		[SerializeField] private GameObject button;
		public GameObject Button { get{ return button; } set{ button = value; } }

		[SerializeField] private bool interactive = true;


		private void Start () 
		{
			transform.localScale = new Vector3( 0.01f, 0.01f, 0.01f );
		}
	
		public void ButtonCheck()
		{	
			
			if( !interactive )
				return;
			
			
			if( isCorrect )
			{
				PunchScale();
				successColor.SetActive( true );
				successImage.SetActive( true );
			
				DisableButton();

				Messenger.Broadcast<int>( "IncreaseScore" , 100 );
			}
			else
			{
				ShakePosition();
				errorImage.SetActive( true );
				DisableButton();

				Messenger.Broadcast( "RemoveHeart" );

				Messenger.Broadcast<int>( "DecreaseScore" , 100 );
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
	}
}
