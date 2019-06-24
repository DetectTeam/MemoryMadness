using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsHandler : MonoBehaviour {


	[SerializeField] private bool clearPlayerPrefs;
	// Use this for initialization

	private void Start()
	{
		//PlayerPrefs.DeleteAll();
	}
	void Update ()
	{
			if( clearPlayerPrefs )
			{
				PlayerPrefs.DeleteAll();
				clearPlayerPrefs = false;
			}
	} 

	public void DeletePlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
	
	
	
}
