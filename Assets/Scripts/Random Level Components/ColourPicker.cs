using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;


	public class ColourPicker : MonoBehaviour
	{

		[SerializeField] private List<Color> colourList = new List<Color>();
	
		public List<Color> GetColourList()
		{
			//var tmp = colourList;
			colourList.ShuffleList();

			return colourList;
		}

	
}
