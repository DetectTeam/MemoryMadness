﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryMadness
{
	public class Star : MonoBehaviour 
	{

		[SerializeField] private Image starImage;
		[SerializeField] private Vector3 scaleSize;

		private Color originalColor;


		private void OnDisable()
		{
			Reset();
		}

		// Use this for initialization
		private void Start () 
		{
			starImage = GetComponent<Image>();
			originalColor = starImage.color;
			
			if( !starImage )
				Debug.Log( "Star Image not Found..." );

			//Invoke( "Activate" , 5.0f );
		}
		
		public void Activate()
		{
			gameObject.transform.localScale = scaleSize;
			iTween.PunchScale( gameObject, iTween.Hash( "x",-2, "y",-2, "time",0.75f));
			starImage.color = new Color( 0.9716f, 0.8722f, 0.1512f, 1 );
					
		}

		private void Reset()
		{
			gameObject.transform.localScale = new Vector3( 1,1,1 );
			starImage.color = originalColor;
		}
	}

}
