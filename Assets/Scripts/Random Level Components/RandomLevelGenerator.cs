using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MemoryMadness
{
	
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

	public class RandomLevelGenerator : MonoBehaviour 
	{
   
		[SerializeField] private ColourPicker colorPicker;
		[SerializeField] private ShapePicker namedShapePicker;
		[SerializeField] private ShapePicker unamedShapePicker;
		
		[SerializeField] private List<GameObject> anchors;
		//[SerializeField] private List<Color> colourList;
		[SerializeField] private List<Sprite> namedShapes;
		[SerializeField] private List<Sprite> unamedShapes;
		[SerializeField] private List<GameObject> cloneSymbols;

		[SerializeField] private List<GameObject> colourSwitchedSymbols;

		[SerializeField] private List<Symbol> memoryPhaseSymbols;
		public List<Symbol> MemoryPhaseSymbols { get{ return memoryPhaseSymbols; } }

		[SerializeField] private GameObject symbolPrefab;

		[SerializeField] private GameObject memoryPhaseContainer;
		[SerializeField] private GameObject memorySymbolContainer;

		// Use this for initialization
		
		private void OnEnable()
		{
			//Debug.Log( "Random Level Generator Enabled....." );
			Messenger.Broadcast( "ResetButtonCount" );
			
			LoadLists();
			//1 Setup symbols
			SetupSymbols();
		}

		private void LoadLists()
		{
			
			if ( namedShapePicker != null )
				namedShapes = namedShapePicker.GetShapeList();
			else
				Debug.Log( "Named Shape List not set..." );

			if ( unamedShapePicker != null )
				unamedShapes = unamedShapePicker.GetShapeList();
			else
				Debug.Log( "UNamed Shape List not set..." );

		}

		private void SetupSymbols()
		{
			if( cloneSymbols.Count > 0 )
			{
				UpdateSymbols();
				GenerateMemoryPhaseSymbols();
				ColourSwitchSymbols();
			}
		}

		private void UpdateSymbols()
		{
			for( int i = 0; i < cloneSymbols.Count; i++ )
			{
				
				var memSymbolsScript = cloneSymbols[i].transform.gameObject.GetComponent<MemorySymbols>();
				
				memSymbolsScript.Reset();

				memSymbolsScript.Name = "MemorySymbol_" + i;

			    memSymbolsScript.SlotNumber = i+1;

				memSymbolsScript.IsCorrect = false;
				memSymbolsScript.IsColourSwitched = false;

				int colourPickerIndex = 0;
				int shapeIndex = 0;

				cloneSymbols[i].transform.Find( "BackgroundColor" ).GetComponent<Image>().color = ColourPicker( i ); //colorPicker.ColourList[ colourPickerIndex ];
				
				cloneSymbols[i].transform.Find( "Rune" ).GetComponent<Image>().sprite = RandomShapePicker( i , unamedShapes );

				cloneSymbols[i].SetActive( true );

				//cloneSymbols[i].transform.parent = anchors[ i ].transform.parent;
			}
		}

		//Returns a colour from a list of colours
		private Color ColourPicker( int currentIndex )
		{
			
			if( currentIndex >= colorPicker.ColourList.Count )
			{
				currentIndex = Random.Range( 0, colorPicker.ColourList.Count - 1 );
			}
	
			return colorPicker.ColourList[ currentIndex ];
		}

		private Sprite RandomShapePicker( int currentIndex, List<Sprite> shapes )
		{
			
			if( currentIndex >= shapes.Count )
			{
				currentIndex = Random.Range( 0, shapes.Count - 1 );
			}

			return shapes[ currentIndex ];
		}

		private void GenerateMemoryPhaseSymbols()
		{
			GameObject randomSymbol;
			int memPhaseSymbolCount = 4;
		    int currentSymbolCount = 0;

			//Randomly select and copy 2 - 5 ( depending on which stage we are on ) symbols from the clone list
			//ie the list of symbols that will be displayed on the game screen
			if( memoryPhaseSymbols.Count > 0 )
				memoryPhaseSymbols.Clear();

			int count = 0;

			//Set our current Symbol Count
			//between 20 and 25 , 3,4 = 20 , 5 = 25

			if( memPhaseSymbolCount == 5 )
			{
				currentSymbolCount = 25;
			}
			else if( memPhaseSymbolCount <= 4 )
			{
				currentSymbolCount = 20;
			}

			List<int> pickedNumberList = new List<int>();

			while( count < memPhaseSymbolCount )
			{
				
				int rand = Random.Range( 0,  currentSymbolCount );

				if( pickedNumberList.Count == 0 )
				{
				 	//Debug.Log( "First Pick" );
					pickedNumberList.Add( rand );
					PickColourAndShape( rand );
					count ++;
				}
				else if( !pickedNumberList.Contains( rand )  )
				{	
					//Debug.Log( "Pick Colour and Shape... " + rand );
					pickedNumberList.Add( rand );
					PickColourAndShape( rand );
					count++;	
				}
				else if( pickedNumberList.Contains( rand )  )
				{
					Debug.Log( "Match Found Picking again ...." + rand  );
				}

			}
			
			count = 0;
			cloneSymbols.ShuffleList();

		}

		private void PickColourAndShape( int rand )
		{
			var randomSymbol = cloneSymbols[ rand ] ;
			var memorySymbolsScript = randomSymbol.GetComponent<MemorySymbols>();
			//cloneSymbols.RemoveAt( rand );
			 	//max--;

			Symbol symbol = new Symbol();

			Debug.Log( "Pick Colour and Shape: " + memorySymbolsScript.Name );

			symbol.Name = memorySymbolsScript.Name;
			symbol.BackgroundColor = memorySymbolsScript.BackgroundColor.GetComponent<Image>().color;
			symbol.Rune = memorySymbolsScript.Rune.GetComponent<Image>();
			
			memorySymbolsScript.IsCorrect = true;
			memorySymbolsScript.IsColourSwitched = true;
			
			memoryPhaseSymbols.Add( symbol );
		
		}

		private void ColourSwitchSymbols()
		{	
			bool isSelectable = true;
		
			for( int i = 0; i < memoryPhaseSymbols.Count; i++ )
			{
				for( int x = 0; x < memoryPhaseSymbols.Count; x++ )
				{
					//Debug.Log( "Names : " + memoryPhaseSymbols[i].Name  + " " + memoryPhaseSymbols[x].Name );
					if( memoryPhaseSymbols[i].Name != memoryPhaseSymbols[x].Name )
					{
						isSelectable = true;
						//Debug.Log( "No Match Found So im going to do my thing...." );
						
						while( isSelectable )
						{

							//Pick a random symbol from the list.
							GameObject selectedMemorySymbol = cloneSymbols[ Random.Range( 0, cloneSymbols.Count ) ];
							//Debug.Log( "SELECTED SYMBOL : " + selectedMemorySymbol );
							MemorySymbols memorySymbolsScript = selectedMemorySymbol.GetComponent<MemorySymbols>();
							
							//If the random symbol isnt a winning symbol 
							//and it hasnt already been colour switched
							if( !memorySymbolsScript.IsCorrect && !memorySymbolsScript.IsColourSwitched )
							{
								
								memorySymbolsScript.BackgroundColor.GetComponent<Image>().color = memoryPhaseSymbols[x].BackgroundColor;
								memorySymbolsScript.Rune.GetComponent<Image>().sprite = memoryPhaseSymbols[i].Rune.sprite;
								memorySymbolsScript.IsColourSwitched = true;
								
								isSelectable = false;
								
							}
					
						}
					}
				}
			}
		}

	}

}


