using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

	[System.Serializable]
	public class Colour
	{
		public int ColourCode;// { get; set; }
		public string ColourName;// { get; set; }
		public Color Color;// { get; set; } 
	}

	public class ColourPicker : MonoBehaviour
	{

		[SerializeField] private List<Colour> colourList = new List<Colour>();
		public List<Colour> ColourList { get{ return colourList; } } 


		private void OnEnable()
		{
			GetColourList();
		}

		private void Start()
		{
			GetColourList();
		}

		public Colour GetRandomColour()
		{
			return colourList[ Random.Range( 0, colourList.Count - 1 ) ]; 
		}
	
		public List<Colour> GetColourList()
		{
			//var tmp = colourList;
			colourList.ShuffleList();

			return colourList;
		}

	
}
