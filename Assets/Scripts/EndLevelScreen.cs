using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelScreen : MonoBehaviour 
{
	[SerializeField] private Vector3 startPosition;

	private void OnDisable()
	{
		Reset();
	}

	private void Reset()
	{
		gameObject.SetActive( false );
		transform.localPosition = startPosition;
	}	
}
