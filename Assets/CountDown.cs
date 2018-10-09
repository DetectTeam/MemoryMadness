using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour 
{

	[SerializeField] private int timeLeft;
	[SerializeField] private TextMeshProUGUI timer;

	[SerializeField] private GameObject game;
	// Use this for initialization
	private void Start () 
	{
		StartCoroutine( Sequence() );
	}

	private IEnumerator Sequence()
	{
		yield return new WaitForSeconds( 1.0f );

		while( timeLeft > 0 )
		{
			yield return new WaitForSeconds( 1.0f );
			timeLeft --;
			timer.text = timeLeft.ToString();
		}

		yield return new WaitForSeconds( 1.0f );

		game.SetActive( true );


	}
	
	
}
