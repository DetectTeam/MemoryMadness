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
		
	}

	private void OnDisable()
	{
		Messenger.RemoveListener<int>( "IncreaseScore" , IncrementScore );
		Messenger.RemoveListener<int>( "DecreaseScore" , DecrementScore );
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
			scoreText.text = "-0" + Mathf.Abs(score).ToString();
		}
		else if( score > 0 )
		{
			scoreText.text = "0" + score.ToString();
		} 
		else if( score == 0 )
		{
			scoreText.text = "0000";
		}
			
	}

}
