using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MemoryMadness
{
	[System.Serializable]
	public class Shape
	{
		[SerializeField] private int shapeCode;
		public int ShapeCode { get{ return shapeCode; } set{ shapeCode = value; } }
		[SerializeField] private string name;
		public string Name { get{ return name;  } set { name = value; } }
		[SerializeField] private Sprite image;
		public Sprite Image { get{ return image; } set{ image = value; } }
	}

	public class ShapePicker : MonoBehaviour 
	{
			[SerializeField] private string name;
			[SerializeField] private List<Shape> imageList = new List<Shape>();
			public List<Shape> ImageList { get{ return ImageList; } } 
			private void OnEnable()
			{
				GetShapeList();
			}

			private void Start()
			{
				GetShapeList();
			}

			public List<Shape> GetShapeList()
			{
				//var tmp = colourList;
				imageList.ShuffleList();

				return imageList;
			}

			public Shape GetRandomShape()
			{
				return imageList[ Random.Range( 0, imageList.Count - 1 ) ];
			}
	}
}
