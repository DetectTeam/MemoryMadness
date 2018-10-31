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
	
	[SerializeField] private GameObject memoryPhase;
	public GameObject MemoryPhase { get{ return memoryPhase; } set{ memoryPhase = value; } }
	
	[SerializeField] private GameObject level;
	public GameObject LevelObj { get{ return level; } set{  level = value; } }

	[SerializeField] private bool isMatch;
	public bool IsMatch { get{ return isMatch; } set{  isMatch = value; } }

}

[System.Serializable]
public struct Stage
{
	[SerializeField] private string name;
	public string Name { get{ return name; } set{  name = value; } }
	[SerializeField] private Level[] level;
	public Level[] Level { get{ return level; } set{  level = value; } }
 }


 

public class BoardManager : MonoBehaviour 
{

	[SerializeField] private int selectedButtonCount;
	[SerializeField] private float delay;
	[SerializeField] private Stage[] stages;
	[SerializeField] private  Level[] levels;
	[SerializeField] private GameObject successMessage;
	[SerializeField] private GameObject failureMessage;
	[SerializeField] private GameObject endLevelBackground;
	[SerializeField] private GameObject resultPanel;
  	[SerializeField] private float percentage = 65;
	[SerializeField] private int currentPhase = 0;

   	public float Percentage { get{ return percentage; } set{ percentage = value; } }



	private int levelToLoad = 0; 


	private void OnEnable()
	{
		Messenger.AddListener( "LoadPhase" , LoadPhase );
		Messenger.AddListener( "LoadLevel" , LoadLevel );
		Messenger.AddListener( "IncrementButtonCount", IncrementButtonCount );
		//StartCoroutine( LoadLevel() );
	}

	private void OnDisable()
	{
		Messenger.RemoveListener( "LoadPhase" , LoadPhase );
		Messenger.RemoveListener( "LoadLevel" , LoadLevel );
		Messenger.RemoveListener( "IncrementButtonCount", IncrementButtonCount );
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


	private void LoadPhase()
	{
		StartCoroutine( IELoadPhase() );
	}

	private IEnumerator IELoadPhase()
	{
		yield return new WaitForSeconds( delay );

		//Get current Stage
		int currentStage = PlayerPrefs.GetInt( "CurrentStage" );

		Debug.Log( "Current Stage " + currentStage );
	
		//Check the level count

		if( PlayerPrefs.HasKey( "CurrentLevel" ) )
		{
			currentPhase = PlayerPrefs.GetInt( "CurrentLevel" );
		}
		else
		{
			PlayerPrefs.SetInt( "CurrentLevel" , currentPhase );
		}
		
		
		if( currentStage < stages.Length )
		{
			stages[ currentStage ].Level[ currentPhase ].MemoryPhase.SetActive( true );
		}
		
		//phases[ 0 ].SetActive( true );
		//Based on level count set correct phase to active
	}

	private void LoadLevel()
	{
		StartCoroutine( IELoadLevel() );
	}
	private IEnumerator IELoadLevel()
	{
		
		yield return new WaitForSeconds ( 0.1f );

		int currentStage  = 0;

		if( PlayerPrefs.HasKey( "CurrentStage" ) )
			currentStage = PlayerPrefs.GetInt( "CurrentStage" );

		levelToLoad = 0;

		Debug.Log("Update" + levelToLoad);
		Debug.Log( "Level " + levelToLoad + " " + levels.Length  );

		if( PlayerPrefs.HasKey( "CurrentLevel" ) )
			levelToLoad  = PlayerPrefs.GetInt( "CurrentLevel" );
	
		//Enable the selected level
		if( levelToLoad <= ( stages[ currentStage ].Level.Length ) )
		{
			stages[ currentStage ].Level[ levelToLoad ].LevelObj.SetActive( true );

			//Broadcast Type of Game to relevant listeners...
			Messenger.Broadcast<GameType>( "GameType" , levels[ levelToLoad ].GameType );
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

	public void IncrementButtonCount()
	{
		selectedButtonCount++;
	}

	public void CheckForWinMM( )
	{
		
		Debug.Log( selectedButtonCount + " " + levels[ levelToLoad ].WinCount );
		endLevelBackground.SetActive( true );
		
		if( selectedButtonCount == levels[ levelToLoad ].WinCount )
		{
			 Success();
			 selectedButtonCount = 0;
		
			
		}
		else
		{
			Failure();
		}

		Invoke( "ChangeLevel", 3 );
	}

	private void ChangeLevel()
	{
		Messenger.Broadcast( "LoadNextLevel" );
		//resultPanel.SetActive( true );
		successMessage.SetActive( false );
		levels[ currentPhase ].MemoryPhase.SetActive( false );
	}

	private void Success()
	{
		successMessage.SetActive( true );
	}

	private void Failure()
	{
		failureMessage.SetActive( true );
	}

	public float calculateScore()
	{
		return  100;//Random.Range( 0, 100 ); 
	}

	
	
}
