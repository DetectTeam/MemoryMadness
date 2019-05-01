using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MemoryMadness
{
	[System.Serializable]
	public class Symbol 
	{
		public int Index { get; set; }
		[SerializeField] private string name;
		public string Name { get{ return name; } set{ name = value; } }
		[SerializeField] private Colour backgroundColor;
		public Colour BackgroundColor { get{ return backgroundColor; } set{ backgroundColor = value; } }

		[SerializeField] private Shape currentShape;
		public Shape CurrentShape { get{ return currentShape; } set{ currentShape = value; } }

		private Image rune;
		public Image Rune { get{ return rune; } set{ rune = value; } }

		private string letter;
		public string Letter { get{ return letter; } set{ letter = value; } }
	}

}
