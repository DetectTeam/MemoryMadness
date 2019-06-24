using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryMadness
{

	public class MemoryPhaseScreen : MonoBehaviour 
	{

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

		// Use this for initialization

	}

}
