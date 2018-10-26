using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsHandler : MonoBehaviour {


	[SerializeField] private bool clearPlayerPrefs;
	// Use this for initialization
	void Update ()
	{
			if( clearPlayerPrefs )
			{
				PlayerPrefs.DeleteAll();
				clearPlayerPrefs = false;
			}
	} 
	
	
	
}
