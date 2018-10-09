using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorImage : MonoBehaviour {

	[SerializeField] private GameObject errorImage;

	private void OnEnable()
	{
		StartCoroutine( FadeImage() );
	}

	private void OnDisable()
	{

	}
	private void Start()
	{
		//errorImage = gameObject.GetComponent<Image>();
	}

	private IEnumerator FadeImage()
	{
		// iTween.FadeTo( gameObject, iTween.Hash("alpha",1,"time",1,"Delay",1.5f));
		 yield return new WaitForSeconds( 1.5f );
		 gameObject.SetActive( false );

	}

	


}
