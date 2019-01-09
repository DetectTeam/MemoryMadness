using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seperator : MonoBehaviour 
{

	void OnEnable()
	{
		//ScaleTo();
	}

	private void ScaleTo( GameObject gameObject )
	{
		 iTween.ScaleTo ( gameObject, iTween.Hash (
			 "scale", new Vector3 ( 650f, 1f, 1f ), 
			 "time", 1.0f, 
			 "easetype", "linear"
		));
	}
	
}
