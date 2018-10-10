using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsScreen : MonoBehaviour {

	[SerializeField] private Vector3 startingPosition;
	private RectTransform rectTransform;
	
	private void OnDisable()
	{
		rectTransform.localPosition = startingPosition;
	}

	private void Start()
	{
		
		rectTransform = GetComponent<RectTransform>();
	}
}
