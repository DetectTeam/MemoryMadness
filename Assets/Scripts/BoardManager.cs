using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameType
{
	SameDifferent,
	MulitMatch
}


[System.Serializable]
public struct Level
{
	
	[SerializeField] private string name;
	public string Name { get{ return name; } set{  name = value; } }
	[SerializeField] private GameType gameType;
	public GameType GameType { get{ return gameType; } set{  gameType = value; } }
	[SerializeField] private int winCount;
	public int WinCount { get{ return winCount; } set{  winCount = value; } }
	[SerializeField] private GameObject level;
	public GameObject LevelObj { get{ return level; } set{  level = value; } }

	[SerializeField] private bool isMatch;
	public bool IsMatch { get{ return isMatch; } set{  isMatch = value; } }

}

public class BoardManager : MonoBehaviour 
{

	[SerializeField] private  Level[] levels;
	[SerializeField] private GameObject successMessage;
	[SerializeField] private GameObject failureMessage;
	[SerializeField] private GameObject endLevelBackground;
	[SerializeField] private GameObject resultPanel;
  	[SerializeField] private float percentage = 65;
   	public float Percentage { get{ return percentage; } set{ percentage = value; } }



	private int levelToLoad = 0; 


	private void OnEnable()
	{
		StartCoroutine( LoadLevel() );
	}

	// Use this for initialization
	private void Start () 
	{
		//Check which level to load
		if( PlayerPrefs.HasKey( "CurrentLevel" ) )
		{
			levelToLoad = PlayerPrefs.GetInt( "CurrentLevel" );
		}
	}


	private IEnumerator LoadLevel()
	{
		

		yield return new WaitForSeconds ( 0.1f );

		levelToLoad = 0;

		Debug.Log("Update" + levelToLoad);
		Debug.Log( levelToLoad + " " + levels.Length  );

		//Enable the selected level
		if( levelToLoad <= (levels.Length ) )
		{
			levels[ levelToLoad ].LevelObj.SetActive( true );

			//Broadcast Type of Game to relevant listeners...
			Messenger.Broadcast<GameType>( "GameType" , levels[ 0 ].GameType );
		}
	}


	//Check for win on the Same Different Game
	public void CheckForWinSD( bool b )
	{
		endLevelBackground.SetActive( true );
		
		if( b && levels[ levelToLoad ].IsMatch )
		{
			Debug.Log( "ITS A WIN !!!" );
			Success();
		}
		else if( b && !levels[ levelToLoad ].IsMatch )
		{
			Debug.Log( "YOU FAILED ...You chose yes but no was required" );
			Failure();
		}
		else if( !b && levels[ levelToLoad ].IsMatch )
		{
			Debug.Log( "YOU FAILED ....You chose no when it should have been yes" );
			Failure();
			
		}
		else if( !b && !levels[ levelToLoad ].IsMatch )
		{
			Debug.Log( "CORRECT THERE IS NO MATCH" );
			Success();
			
		}
		else
		{

		}

		
		Invoke( "ChangeLevel", 3 );
	
			
	}

	private void ChangeLevel()
	{
		Messenger.Broadcast( "LoadNextLevel" );
		//resultPanel.SetActive( true );
		successMessage.SetActive( false );
	}

	private void Success()
	{
		successMessage.SetActive( true );
	}

	private void Failure()
	{
		//failureMessage.SetActive( true );
	}

	public float calculateScore()
	{
		return  100;//Random.Range( 0, 100 ); 
	}

	
	
}
