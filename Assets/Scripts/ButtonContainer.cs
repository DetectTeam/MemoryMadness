using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContainer : MonoBehaviour 
{

	[SerializeField] GameObject[] buttons;

	private void OnEnable()
	{
		Messenger.AddListener<GameType>( "GameType" , LoadButton );
	}

	private void OnDisable()
	{

	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void LoadButton( GameType gameType )
	{
		Debug.Log( "Load Button Called...." + gameType );

		if( gameType == GameType.SameDifferent )
		{
			Debug.Log( "Loading yes no buttons" );
			buttons[1].SetActive( false );
			buttons[0].SetActive( true );
		}
		else
		{
			Debug.Log( "Loading I got em all buttons..." );
			buttons[0].SetActive( false );
			buttons[1].SetActive( true );
		}
	}
}
