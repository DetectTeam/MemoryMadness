using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour 
{
	[SerializeField] private float speed;
	[SerializeField] private float scale;
	[SerializeField] private Vector3 originalScale;
	//[SerializeField] private float alpha;
	 private Color color;
	[SerializeField] private bool isScaling;

	[SerializeField] private float deltaTime;
	[SerializeField] private Vector3 targetScale;

	[SerializeField] private float mag; 
	[SerializeField] private float mag2; 

	private void OnEnable()
	{
		originalScale = transform.localScale;
		color = gameObject.GetComponent<Image>().color;

		mag2 = targetScale.magnitude; 
		//StartCoroutine( DisplayHighLight() );
		isScaling = true;

		Messenger.AddListener( "ToggleHighLight" , ToggleHighlight );

	}

	private void OnDisable()
	{
		Messenger.RemoveListener( "ToggleHighLight" , ToggleHighlight );
	}

	private void ToggleHighlight()
	{
		if( color.a == 1.0f )
			color.a = 0.0f;
		else
			color.a = 1.0f;
	}

	// private void Update()
	// {
	// 	if( isScaling )
	// 	{
	// 		 transform.localScale = Vector3.Lerp ( transform.localScale, targetScale, speed * Time.deltaTime );
	// 		 mag = transform.localScale.magnitude;

	// 		float dist = Vector3.Distance( transform.localScale, targetScale);

	// 		if( dist < 1.0f )
	// 			FadeOut( speed );

	// 		 if( dist < 0.6f )
	// 		 { 
	// 			 isScaling = false;
	// 			 Debug.Log( "Finished Scaling..." );
				
	// 			// FadeOut( speed - 0.5f );
	// 		 }	
	// 	}
	// 	else
	// 	{
	// 		Reset();
	// 		isScaling = true;
	// 	}
	// }

	private float alpha = 1.0f;
	[SerializeField] private float fadeSpeed;
	private void FadeOut( float time )
	{
		float maximum = 1.0f;
		float minimum = 0.0f;

		alpha = Mathf.Lerp( alpha, minimum, time * fadeSpeed * Time.deltaTime );
		Color newColor = new Color( color.r, color.g, color.b , alpha );
		gameObject.GetComponent<Image>().color = newColor;
	}

	private void Reset()
	{
		Debug.Log( "Reset Called" ); 
		transform.localScale = new Vector3( 0.25f, 0.25f, 0.937f );
		//transform.localScale = originalScale;
		scale = 2;
		alpha = 1;
	}
}
