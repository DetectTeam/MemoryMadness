﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : MonoBehaviour {

	[SerializeField] private Vector3 startingPosition;
	private RectTransform rectTransform;
	
	private void OnEnable()
	{
		//Messenger.Broadcast( "LoadLevel" );
	}
	private void OnDisable()
	{
		rectTransform.localPosition = startingPosition;
		
	}

	private void Start()
	{
		
		rectTransform = GetComponent<RectTransform>();
	}
}
