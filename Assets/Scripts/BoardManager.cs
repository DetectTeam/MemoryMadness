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
	public string name;
	public GameType gameType;
	public int winCount;
	public GameObject level;
}

public class BoardManager : MonoBehaviour 
{

	[SerializeField] private  Level[] levels;


	private void OnEnable()
	{
		StartCoroutine( LoadLevel() );
	}

	// Use this for initialization
	private void Start () 
	{
		
	}


	private IEnumerator LoadLevel()
	{
		int levelToLoad = 0; 

		yield return new WaitForSeconds ( 0.1f );

		//Check which level to load
		if( PlayerPrefs.HasKey( "CurrentLevel" ) )
		{
			levelToLoad = PlayerPrefs.GetInt( "CurrentLevel" );
		}
	
		Debug.Log( levelToLoad + " " + levels.Length  );

		//Enable the selected level
		if( levelToLoad < (levels.Length ) )
		{
			levels[ levelToLoad ].level.SetActive( true );

			//Broadcast Type of Game to relevant listeners...
			Messenger.Broadcast<GameType>( "GameType" , levels[ levelToLoad ].gameType );
		}
	}
	
}
