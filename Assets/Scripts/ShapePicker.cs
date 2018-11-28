using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapePicker : MonoBehaviour 
{

		[SerializeField] private string name;
		[SerializeField] private List<Sprite> imageList = new List<Sprite>();
		public List<Sprite> ImageList { get{ return ImageList; } } 
		private void OnEnable()
		{
			GetShapeList();
		}

		private void Start()
		{
			GetShapeList();
		}

		public List<Sprite> GetShapeList()
		{
			//var tmp = colourList;
			imageList.ShuffleList();

			return imageList;
		}

		public Sprite GetRandomShape()
		{
			return imageList[ Random.Range( 0, imageList.Count - 1 ) ];
		}
}
