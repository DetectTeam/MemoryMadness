using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

namespace MemoryMadness
{

	public class ColourPicker : MonoBehaviour
	{


		[SerializeField] private List<Color> colourList = new List<Color>();

		

		[SerializeField] private List<Color> result = new List<Color>();

		// Use this for initialization
		[SerializeField] private Color selectedColour;

		private void Start()
		{
			result = GetColourList();
		}

		public List<Color> GetColourList()
		{
			
			//var tmp = colourList;
			colourList.ShuffleList();

			return colourList;
		}

	}
}
