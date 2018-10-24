using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour 
{
	[SerializeField] private Image progressBar;
	[SerializeField] private TextMeshProUGUI percentageText;

	private int speed = 1;
	

	// Use this for initialization
	void Start () 
	{
		StartCoroutine( Fill() );
	}

	private IEnumerator Fill()
	{
		float currentValue = 0;

		yield return new WaitForSeconds( 2.0f );
		
		while( currentValue < 100 )
		{
			yield return null;
			currentValue ++; //speed; //* Time.deltaTime;
			percentageText.text = currentValue.ToString()  +  "%";
			progressBar.fillAmount = ( currentValue / 100 );

			
		}
	}
	
	
}
