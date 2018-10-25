using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour 
{
	[SerializeField] private Image progressBar;
	[SerializeField] private TextMeshProUGUI percentageText;

	[SerializeField] private BoardManager boardManager;

	[SerializeField] private Star star1;
	[SerializeField] private Star star2;
	[SerializeField] private Star star3;

	private int speed = 1;

	private void OnEnable()
	{		
		if( boardManager )
			StartCoroutine( Fill( boardManager.calculateScore() ) );
	
	}



	// Use this for initialization
	private void Start () 
	{
	
	}

	private IEnumerator Fill( float percentage )
	{
		float currentValue = 0;
		progressBar.fillAmount = 0f;

		yield return new WaitForSeconds( 2.0f );
		
		while( currentValue < percentage )
		{
			yield return null;
			currentValue ++; //speed; //* Time.deltaTime;
			percentageText.text = currentValue.ToString()  +  "%";
			progressBar.fillAmount = ( currentValue / 133 );

			if( currentValue == 33 )
				star1.Activate();
			
			if( currentValue == 66 )
				star2.Activate();

			if( currentValue == 99 )
				star3.Activate();

		}
	}
	
	
}
