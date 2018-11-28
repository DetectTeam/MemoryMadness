using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MemoryMadness
{
	public class SameDifferentGenerator : MonoBehaviour 
	{
		[SerializeField] private int stage;
		[SerializeField] private GameObject sameDifferentContainer;
		[SerializeField] private List<GameObject> topList;
		[SerializeField] private List<GameObject> bottomList;
		[SerializeField] private bool isCorrect;

		[SerializeField] private ColourPicker colorPicker;
		[SerializeField] private ShapePicker namedShapePicker;
		[SerializeField] private ShapePicker unamedShapePicker;
		private void Start()
		{
			if( !sameDifferentContainer )
			{
				Debug.Log( "Same Different Container not set" );
			} 
			else
			{
				sameDifferentContainer.SetActive( true );
				BuildLevel( stage );
			} 
		}

		private void BuildLevel( int numSymbols )
		{
			int numSymbolsToDisplay = 0;
			ResetSymbols();
			
			if( numSymbols <= 2 )
				//Display 2 Symbols
				numSymbolsToDisplay = 2;
			else
				//Display 3 - 5 Symbols
				numSymbolsToDisplay = numSymbols;
			
		
			PositionSymbols( numSymbolsToDisplay );
			DisplaySymbols( numSymbolsToDisplay );
		}

		private void ResetSymbols()
		{
			for( int x = 0; x < topList.Count; x++ )
			{
				topList[x].SetActive( false );
				bottomList[x].SetActive( false );
			}
		}

		private void RandomizeSymbols()
		{
			for ( int x =0; x < topList.Count; x++ )
			{
				topList[x].transform.Find( "BackgroundColor" ).GetComponent<Image>().color = Color.red; //colorPicker.ColourList[ colourPickerIndex ];
				topList[x].transform.Find( "Rune" ).GetComponent<Image>().sprite = namedShapePicker.GetRandomShape();
			}
		}

		private void DisplaySymbols( int symbolCount )
		{
			for( int x = 0; x < symbolCount; x++ )
			{
				topList[ x ].SetActive( true );
				bottomList[ x ].SetActive( true );
			}
		}

		private void PositionSymbols( int symbolCount )
		{
			Vector3 pos = Vector3.zero;
			int x = 0;

			if( symbolCount <= 2 )
				x = 240;
			else if( symbolCount == 3 )
				x = 160;
			else if( symbolCount == 4 )
				x = 80;
			else if( symbolCount == 5 )
				x = 0;

			pos = new Vector3( x, -200f, 0f );
			sameDifferentContainer.transform.localPosition = pos;
		}
	}
}
