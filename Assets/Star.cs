﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour 
{

	[SerializeField] private Image starImage;

	

	// Use this for initialization
	void Start () 
	{
		Debug.Log( "Start Called" );
		
		starImage = GetComponent<Image>();
		
		if( !starImage )
			Debug.Log( "NONE" );

		//Invoke( "Activate" , 5.0f );
	}
	
	public void Activate()
	{
		gameObject.transform.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
		iTween.PunchScale( gameObject, iTween.Hash( "x",-2, "y",-2, "time",0.75f));
		starImage.color = new Color( 0.9716f, 0.8722f, 0.1512f, 1 );
				
	}
}
