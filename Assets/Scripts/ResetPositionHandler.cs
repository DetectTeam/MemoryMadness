using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPositionHandler : MonoBehaviour {

	[SerializeField] private Vector3 startPosition;

	private void OnDisable()
	{
		Reset();
	}

	
	private void Reset()
	{
		transform.localPosition = startPosition;
	}
	
}
