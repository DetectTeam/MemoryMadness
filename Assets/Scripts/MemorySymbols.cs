using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemorySymbols : MonoBehaviour 
{
	[SerializeField] private bool isCorrect;
	[SerializeField] private Vector3 shakeAmount;
	[SerializeField] private float shakeTime;

	[SerializeField] private GameObject errorImage;


	[SerializeField] private GameObject successImage;
	[SerializeField] private GameObject backgroundColor;
	[SerializeField] private GameObject rune;

	[SerializeField] private GameObject button;

	// Use this for initialization
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
		}
		else
		{
			ShakePosition();
			errorImage.SetActive( true );
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
	
	
}
