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
		[SerializeField] private ColourPicker colorPicker;
		[SerializeField] private ShapePicker namedShapePicker;
		[SerializeField] private ShapePicker unamedShapePicker;
		[SerializeField] private List<Sprite> symbolList;
		[SerializeField] private bool isNamed = false;
		[SerializeField] private bool isColoured = false;
		[SerializeField] private bool isCorrect;

		
		private void OnEnable()
		{
			if( !sameDifferentContainer )
			{
				Debug.Log( "Same Different Container not set" );
			} 
			else
			{
				if( PlayerPrefs.HasKey( "CurrentStage" ) )
				{
					stage = PlayerPrefs.GetInt( "CurrentStage" );
				}
			
			    isNamed = ( Random.value > 0.5f );
				//isColoured = ( Random.value > 0.5f );
				isColoured = true;
				isCorrect = ( Random.value > 0.5f );

				sameDifferentContainer.SetActive( true );
				BuildLevel( stage );
			} 

		}

		private void BuildLevel( int numSymbols )
		{
			int numSymbolsToDisplay = 0;

			ResetSymbols();
			
			if( numSymbols <= 2 )
			{
				//Display 2 Symbols
				numSymbolsToDisplay = 2;
			}
			else if( numSymbols > 5 )
			{
				numSymbolsToDisplay = 5;
			}
			else
			{
				//Display 3 - 5 Symbols
				numSymbolsToDisplay = numSymbols;
			}
			
			RandomizeSymbols( numSymbolsToDisplay );
			PositionSymbolContainer( numSymbolsToDisplay );
			DisplaySymbols( numSymbolsToDisplay );

			Messenger.Broadcast( "ResetTimer" );
		}

		private void ResetSymbols()
		{
			for( int x = 0; x < topList.Count; x++ )
			{
				topList[x].SetActive( false );
				//bottomList[x].SetActive( false );
			}

			if( bottomList.Count > 0 )
			{
				foreach( GameObject symbol in bottomList )
				{
					Destroy( symbol );
				}
			}
		}

		
		private void RandomizeSymbols( int numSymbols )
		{
			int rand = 0;
			//int numSymbols = 2;

			List<Color> allColours = new List<Color>( colorPicker.ColourList );

			List<Color> bottomListColours = new List<Color>();

			//Color levelColour = colorPicker.ColourList[ rand ];

			if( isNamed ) //Named Symbol if true. Un named symbol if false
				symbolList = new List<Sprite>( namedShapePicker.GetShapeList() );
			else
				symbolList = new List<Sprite>( unamedShapePicker.GetShapeList() ); 

			//Build and display Top Row Symbols
			for ( int x = 0; x < numSymbols; x++ )
			{
				rand = Random.Range( 0, symbolList.Count );

				if( isColoured )
				{
					topList[x].transform.Find( "BackgroundColor" ).GetComponent<Image>().color = allColours[ rand ];
					bottomListColours.Add( allColours[ rand ] );
					allColours.RemoveAt( rand );
				}
				// else
				// {
				// 	topList[x].transform.Find( "BackgroundColor" ).GetComponent<Image>().color = levelColour;		
				// }

				topList[x].transform.Find( "Rune" ).GetComponent<Image>().sprite = symbolList[ rand ];
				
				symbolList.RemoveAt( rand );
			}

			//Build and Display Bottom Row Symbols

			bottomList.Clear();

			Debug.Log( topList.Count );
			
			//Clone the top row and position it 200 below 
			foreach( GameObject symbol in topList )
			{
				var clone = Instantiate( symbol , symbol.transform.position, Quaternion.identity );
				
				clone.transform.parent = sameDifferentContainer.transform;
	
				Vector3 tmpTransform = clone.transform.position;

				//Adjust the y position
				tmpTransform.y = clone.transform.position.y - 200f;
					
				clone.transform.position = tmpTransform;

				// if( !isCorrect )
				// {
				// 	clone.transform.Find( "BackgroundColor" ).GetComponent<Image>().color = bottomListColours[ Random.Range( 0, bottomListColours.Count ) ];
				// }
				
				bottomList.Add( clone );	
			}
			
			//randomize bottomList
			//Switch 2 colors 

			
		}

		private void SwitchTwoColours( List<GameObject> list  )
		{
			int rand = Random.Range( 0, list.Count - 1 );
			Color colourOne, colourTwo = Color.blue;
			
			GameObject symbolOne  =  list[ rand ];

			list.RemoveAt( rand );

			rand = Random.Range( 0, list.Count - 1 );
			GameObject symbolTwo = list[ rand ];
			list.RemoveAt( rand );

			colourOne = symbolOne.transform.Find( "BackgroundColor" ).GetComponent<Image>().color;
			colourTwo = symbolTwo.transform.Find( "BackgroundColor" ).GetComponent<Image>().color;
			
			symbolOne.transform.Find( "BackgroundColor" ).GetComponent<Image>().color = colourTwo;
			symbolTwo.transform.Find( "BackgroundColor" ).GetComponent<Image>().color = colourOne;

			list.Add( symbolOne );
			list.Add( symbolTwo );

			list.ShuffleList();
		}

		private void DisplaySymbols( int symbolCount )
		{
			Debug.Log( "DISPLAY SYMBOLS : " + symbolCount );
			
			List<GameObject> tmp = new List<GameObject>();

			for( int x = 0; x < symbolCount; x++ )
			{
				topList[ x ].SetActive( true );
				//bottomList[ x ].SetActive( true );
				tmp.Add( bottomList[x]);
				bottomList[x].SetActive( true );
			}

			if( !isCorrect  )
				SwitchTwoColours( tmp );

			ShuffleListPosition( tmp );

			tmp.Clear();
		}

		private void PositionSymbolContainer( int symbolCount )
		{
			Vector3 pos = Vector3.zero;
			int x = 0;

			if( symbolCount <= 2 )
				x = 180;
			else if( symbolCount == 3 )
				x = 100;
			else if( symbolCount == 4 )
				x = 50;
			else if( symbolCount == 5 )
				x = 0;

			pos = new Vector3( x, -200f, 0f );
			sameDifferentContainer.transform.localPosition = pos;
		}


		private static System.Random rng = new System.Random();  
		private void ShuffleListPosition( List<GameObject> list )
		{
			int n = list.Count; 
			
			while (n > 1) 
			{  
				n--;  
				int k = rng.Next(n + 1);  
				
				Vector3 value = list[k].transform.position; 
		
				list[k].transform.position = list[n].transform.position;  
				list[n].transform.position = value;
			}  
		}

		public void CheckForWinCondition( bool b )
		{
			if( isCorrect == b )
			{
				//Enable Correct
				Messenger.Broadcast( "Success" );
			}
			else 
			{
				//enable failure
				Messenger.Broadcast( "Failure" );
			}

			Messenger.Broadcast( "ChangeLevel" );
		}
	}
}
