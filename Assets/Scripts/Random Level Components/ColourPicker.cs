using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;


	public class Colour
	{
		public int colourCode;
		public Color color;
	}

	public class ColourPicker : MonoBehaviour
	{

		[SerializeField] private List<Color> colourList = new List<Color>();
		public List<Color> ColourList { get{ return colourList; } } 


		private void OnEnable()
		{
			GetColourList();
		}

		private void Start()
		{
			GetColourList();
		}

		public Color GetRandomColour()
		{
			return colourList[ Random.Range( 0, colourList.Count - 1 ) ]; 
		}
	
		public List<Color> GetColourList()
		{
			//var tmp = colourList;
			colourList.ShuffleList();

			return colourList;
		}

	
}
