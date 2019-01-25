using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManagerTut : MonoBehaviour 
{

	public static ScoreManagerTut Instance = null;
	
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private int score;

	 List<float> listOfLevelTimes = new List<float>();


	void Awake()
	{
			 //Check if instance already exists
             if (Instance == null)
                
                //if not, set instance to this
                 Instance = this;
            
            //If instance already exists and it's not this:
             else if (Instance != this)
                
                 //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                 Destroy(gameObject);    
            
        //     //Sets this to not be destroyed when reloading scene
             DontDestroyOnLoad(gameObject);
	}
	private void OnEnable()
	{
		Messenger.AddListener<int>( "IncreaseScore" , IncrementScore );
		Messenger.AddListener<int>( "DecreaseScore" , DecrementScore );
		Messenger.AddListener<float>( "RecordLevelTime", AddLevelTime );

		Messenger.MarkAsPermanent( "IncreaseScore" );
		Messenger.MarkAsPermanent( "DecreaseScore" );
		Messenger.MarkAsPermanent( "RecordLevelTime" );
		
	}

	private void OnDisable()
	{
		Messenger.RemoveListener<int>( "IncreaseScore" , IncrementScore );
		Messenger.RemoveListener<int>( "DecreaseScore" , DecrementScore );
		Messenger.RemoveListener<float>( "RecordLevelTime", AddLevelTime );
	}

	private void IncrementScore( int amt )
	{	
		score = score + amt;
		UpdateScore( score );
		
	}

	private void DecrementScore( int amt )
	{
		score = score - amt;

		if( score < 0 )
			score = 0;
		
		UpdateScore( score );	
	}

	private void UpdateScore( int score )
	{
		
		if( score > 0 )
		{
			scoreText.text = "0" + score.ToString();
		} 
		else if( score <= 0   )
		{
			scoreText.text = "0000";
		}
			
	}

	private void AddLevelTime(  float time )
	{
		listOfLevelTimes.Add( time );
	}

}
