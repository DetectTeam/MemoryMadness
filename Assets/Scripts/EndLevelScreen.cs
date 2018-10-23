using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelScreen : MonoBehaviour 
{
	[SerializeField] private Vector3 startPosition;

	private Transform _transform;

	private void OnDisable()
	{
		Reset();
	}

	private void Start()
	{
		_transform = transform;
	}

	private void Reset()
	{
		gameObject.SetActive( false );
		_transform.localPosition = startPosition;
	}	
}
