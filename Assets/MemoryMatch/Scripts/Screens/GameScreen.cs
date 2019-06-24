using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : MonoBehaviour {

	[SerializeField] private Vector3 startingPosition;
	private RectTransform rectTransform;

	[SerializeField] private GameObject levelContainer; 
	
	private void OnEnable()
	{
		//Messenger.Broadcast( "LoadLevel" );
		if( !levelContainer.activeSelf )
			levelContainer.SetActive( true );
		
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
