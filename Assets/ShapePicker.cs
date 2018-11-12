using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapePicker : MonoBehaviour 
{

		[SerializeField] private string name;
		[SerializeField] private List<Sprite> imageList = new List<Sprite>();

		public List<Sprite> GetShapeList()
		{
			//var tmp = colourList;
			imageList.ShuffleList();

			return imageList;
		}
}
