using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerTut : MonoBehaviour
{
	public static SceneManagerTut Instance;

	private void Awake()
	{
		//Check if instance already exists
             if (Instance == null)
                //if not, set instance to this
                 Instance = this;
        	//If instance already exists and it's not this:
             else if (Instance != this)
                 //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                 Destroy(gameObject);  
	}
	
	public void LoadScene( string sceneToLoad )
	{
		SceneManager.LoadScene( sceneToLoad );
	}	
}
