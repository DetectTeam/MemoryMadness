using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour 
{


	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private int score;
	
	private void OnEnable()
	{
		Messenger.AddListener<int>( "IncreaseScore" , IncrementScore );
		Messenger.AddListener<int>( "DecreaseScore" , DecrementScore );
		//Messenger.AddListener<GameObject>( "FloatScore" , FloatingScore );
	}

	private void OnDisable()
	{
		Messenger.RemoveListener<int>( "IncreaseScore" , IncrementScore );
		Messenger.RemoveListener<int>( "DecreaseScore" , DecrementScore );
		//Messenger.RemoveListener<GameObject>( "FloatScore" , FloatingScore );
	}

	
	private void IncrementScore( int amt )
	{	
		score = score + amt;
		UpdateScore( score );
		
	}

	private void DecrementScore( int amt )
	{
		score = score - amt;
		UpdateScore( score );	
	}

	private void UpdateScore( int score )
	{
		if( score < 0 )
		{
			scoreText.text = "-000" + Mathf.Abs(score).ToString();
		}
		else if( score > 0 )
		{
			scoreText.text = "000" + score.ToString();
		} 
		else if( score == 0 )
		{
			scoreText.text = "000000";
		}
			
	}


	// 	iTween.MoveBy( clone, iTween.Hash(
    //  		"y"   , 100f,
    //  		"time", 0.65f
 	// 	));

	// 	yield return new WaitForSeconds( 0.75f );
	// 	Destroy( clone );

		
 
	// }
		
	

}
