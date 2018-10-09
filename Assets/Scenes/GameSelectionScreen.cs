﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelectionScreen : MonoBehaviour 
{
	[SerializeField] private GameObject titleScreen;
	[SerializeField] private GameObject title;
	[SerializeField] private GameObject easy;
	[SerializeField] private GameObject normal;
	[SerializeField] private GameObject hard;

	private RectTransform rectTransform;

	private void OnEnable()
	{
		Messenger.AddListener( "Disable" , Disable );
	}

	private void OnDisable()
	{
		Messenger.RemoveListener( "Disable" , Disable );
		Debug.Log( "Resetting..." );
		rectTransform.transform.localPosition = new Vector3( 0, 1600, 0 );
	}

	// Use this for initialization
	private void Start () 
	{
		StartCoroutine( Sequence() );
		rectTransform = GetComponent<RectTransform>();
		Debug.Log( rectTransform.position.y );
	}
	
	private IEnumerator Sequence()
	{
		//yield return new WaitForSeconds( 1.0f );

		//title.SetActive( true );

		yield return new WaitForSeconds( 1.0f );
		
		titleScreen.SetActive( false );
	}

	private void Disable()
	{
		Debug.Log( gameObject + " Disabling self" );
		gameObject.SetActive( false );
	}
}
