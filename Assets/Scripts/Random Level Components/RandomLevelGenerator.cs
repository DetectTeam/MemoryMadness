using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomLevelGenerator : MonoBehaviour 
{

	[SerializeField] private ColourPicker colorPicker;
	[SerializeField] private ShapePicker namedShapePicker;
	[SerializeField] private ShapePicker unamedShapePicker;
	[SerializeField] private List<Color> colourList;
    [SerializeField] private List<Sprite> namedShapes;
	[SerializeField] private List<Sprite> unamedShapes;
	
	// Use this for initialization
	void Start () 
	{
		LoadLists();
	}

	private void LoadLists()
	{
			colorPicker = GetComponent<ColourPicker>();
	
		if( colourList != null )
			colourList = colorPicker.GetColourList();
		else
			Debug.Log( "Colour List not set..." );

		if ( namedShapePicker != null )
			namedShapes = namedShapePicker.GetShapeList();
		else
			Debug.Log( "Named Shape List not set..." );

		if ( unamedShapePicker != null )
			unamedShapes = unamedShapePicker.GetShapeList();
		else
			Debug.Log( "UNamed Shape List not set..." );


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
