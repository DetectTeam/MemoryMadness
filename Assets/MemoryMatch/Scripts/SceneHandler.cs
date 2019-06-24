using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour 
{
	[SerializeField] private string sceneName;

	public void LoadScene( string sceneName )
	{
		if( sceneName.Length > 0 )
			SceneManager.LoadScene( sceneName  );
		else
			Debug.Log( "Not a valid scene Name" );
	}	
}
