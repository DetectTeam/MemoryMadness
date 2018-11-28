
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Symbol 
{
	[SerializeField] private string name;
	public string Name { get{ return name; } set{ name = value; } }
	[SerializeField] private Color backgroundColor;
	public Color BackgroundColor { get{ return backgroundColor; } set{ backgroundColor = value; } }
	private Image rune;
	public Image Rune { get{ return rune; } set{ rune = value; } }

}
