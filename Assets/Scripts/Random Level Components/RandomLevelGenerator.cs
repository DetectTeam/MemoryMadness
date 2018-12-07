using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;



namespace MemoryMadness
{
	
	public class RandomLevelGenerator : MonoBehaviour 
	{
   
		[SerializeField] private ColourPicker colorPicker;
		[SerializeField] private ShapePicker namedShapePicker;
		[SerializeField] private ShapePicker unamedShapePicker;
		
		[SerializeField] private List<LevelType> currentStageLevels;
		[SerializeField] private List<GameObject> anchors;
		//[SerializeField] private List<Color> colourList;
		[SerializeField] private List<Shape> namedShapes;
		[SerializeField] private List<Shape> unamedShapes;
		[SerializeField] private List<GameObject> cloneSymbols;

		public List<GameObject> CurrentLevelSymbols { get{ return cloneSymbols; } }
		
		[SerializeField] private List<GameObject> colourSwitchedSymbols;

		[SerializeField] private List<Symbol> memoryPhaseSymbols;
		public List<Symbol> MemoryPhaseSymbols { get{ return memoryPhaseSymbols; } }

		[SerializeField] private GameObject symbolPrefab;

		[SerializeField] private GameObject memoryPhaseContainer;
		[SerializeField] private GameObject memorySymbolContainer;

		[SerializeField] private int currentStage = 2;
		[SerializeField] private int memPhaseSymbolCount;
		
		[SerializeField] private int currentLevel = 0;
		
		// Use this for initialization

		public static RandomLevelGenerator Instance; 
		private void Awake()
		{
			if( Instance == null )
				Instance = this;
			else
				Destroy( gameObject );
		}


		private void OnEnable()
		{
			//Reset the correct button count to zero
			Messenger.Broadcast( "ResetCorrectButtonCount" );
	
			CheckCurrentStage( StageManager.Instance.CurrentStage );

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

		private void CheckCurrentStage( int currentStage )
		{
			if( currentStage <= 2 )
				memPhaseSymbolCount = 2;
			else if( currentStage == 3  )
				memPhaseSymbolCount = 3;
			else if( currentStage == 4 )
				memPhaseSymbolCount = 4;
			else if( currentStage > 4 )
				memPhaseSymbolCount = 5;

			//Set the win count value in the game manager
			Messenger.Broadcast<int>( "SetWinCount" , memPhaseSymbolCount );
			//Messenger.Broadcast( "ResetHearts" );					
		}

		private void SetupSymbols()
		{
			if( cloneSymbols.Count > 0 )
			{
				UpdateSymbols();
				GenerateMemoryPhaseSymbols();

				//Only perform a colour switch on levels with coloured symbols
				if( StageManager.Instance.CurrentLevelType == LevelType.NameableColour  || StageManager.Instance.CurrentLevelType == LevelType.UnNameableColour )
				{
					if( memoryPhaseSymbols.Count <= 3 )
						ColourSwitchSymbols();
					else
						ColourSwitchFiveSymbols();	
				}
			}
		}

	

		private void UpdateSymbols()
		{
			bool isNoColour = false;

			List<Shape> levelSprites;

			Colour bgColor = colorPicker.GetRandomColour();
			 

			if( StageManager.Instance.CurrentLevelType == LevelType.UnNameableNonColour  || StageManager.Instance.CurrentLevelType == LevelType.NameableNonColour )
			{
				isNoColour = true;
			}

			if( StageManager.Instance.CurrentLevelType == LevelType.UnNameableColour  || StageManager.Instance.CurrentLevelType == LevelType.UnNameableNonColour )
			{
				levelSprites = unamedShapes;
			}
			else
			{
				levelSprites = namedShapes;
			}

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

				
				if( !isNoColour ) 
				{
					cloneSymbols[i].transform.Find( "BackgroundColor" ).GetComponent<Image>().color = ColourPicker( i ).Color; //colorPicker.ColourList[ colourPickerIndex ];	
					memSymbolsScript.ColourCode = ColourPicker( i ).ColourCode;
				}
				else
				{
					cloneSymbols[i].transform.Find( "BackgroundColor" ).GetComponent<Image>().color = bgColor.Color; //Pick a single random Colour . Used for noncoloured symbols
					memSymbolsScript.ColourCode = bgColor.ColourCode;
				}
			
				cloneSymbols[i].transform.Find( "Rune" ).GetComponent<Image>().sprite = RandomShapePicker( i , levelSprites );

				cloneSymbols[i].SetActive( true );

				//cloneSymbols[i].transform.parent = anchors[ i ].transform.parent;
			}
		}

		//Returns a colour from a list of colours
		private Colour ColourPicker( int currentIndex )
		{
			
			if( currentIndex >= colorPicker.ColourList.Count )
			{
				currentIndex = Random.Range( 0, colorPicker.ColourList.Count - 1 );
			}
	
			return colorPicker.ColourList[ currentIndex ];
		}

		private Sprite RandomShapePicker( int currentIndex, List<Shape> shapes )
		{
			
			if( currentIndex >= shapes.Count )
			{
				currentIndex = Random.Range( 0, shapes.Count - 1 );
			}

			return shapes[ currentIndex ].Image;
		}

		private void GenerateMemoryPhaseSymbols()
		{
			GameObject randomSymbol;
			
		    int currentSymbolCount = 20;

			//Randomly select and copy 2 - 5 ( depending on which stage we are on ) symbols from the clone list
			//ie the list of symbols that will be displayed on the game screen
			if( memoryPhaseSymbols.Count > 0 )
				memoryPhaseSymbols.Clear();

			int count = 0;

			List<int> pickedNumberList = new List<int>();

			while( count < memPhaseSymbolCount )
			{
				
				int rand = Random.Range( 0,  currentSymbolCount );

				if( pickedNumberList.Count == 0 )
				{
					Debug.Log( rand );
					pickedNumberList.Add( rand );
					PickColourAndShape( rand );
					count ++;
				}
				else if( !pickedNumberList.Contains( rand )  )
				{	
					pickedNumberList.Add( rand );
					PickColourAndShape( rand );
					count++;	
				}
			}
			
			count = 0;
			//cloneSymbols.ShuffleList();
		}

		private void PickColourAndShape( int rand )
		{
			var randomSymbol = cloneSymbols[ rand ] ;
			var memorySymbolsScript = randomSymbol.GetComponent<MemorySymbols>();
			//cloneSymbols.RemoveAt( rand );
			 	//max--;
		    if( !memorySymbolsScript )
				Debug.Log( "BIG PROBLEM >>>>>>" );
			else
			{
				Debug.Log( "Memory Symbol : " + memorySymbolsScript.BackgroundColor.GetComponent<Image>().color );
			}

			Symbol symbol = new Symbol();
			Colour c = new Colour();

			symbol.Name = memorySymbolsScript.Name;
		
			c.Color = memorySymbolsScript.BackgroundColor.GetComponent<Image>().color;
			c.ColourName = "";
			c.ColourCode = memorySymbolsScript.ColourCode;

			symbol.BackgroundColor = c;
			symbol.Rune = memorySymbolsScript.Rune.GetComponent<Image>();
			
			memorySymbolsScript.IsCorrect = true;
			memorySymbolsScript.IsColourSwitched = true;
			
			memoryPhaseSymbols.Add( symbol );
		
		}


		//Alternative way of colour switching symbols 
		//when symbol count is greater than 3

		[SerializeField] private List<Symbol> symbolsToSwitch = new List<Symbol>();
		[SerializeField] private Color foundColor;

		[SerializeField] private List<Color> colourSwitchList;
		private void ColourSwitchFiveSymbols()
		{

				int rand = 0;
				Color tmp = Color.clear;
				bool foundMatch = false;
				bool isSelectable = false;

				//List<Symbol> symbolsToSwitch = new List<Symbol>();

			
			
				//Create new list of symbols from the current list of memoryphase symbols
				//My shitty way of doing a deep copy
				//Find a better way to do this !!!!!!
				for( int i = 0; i< memoryPhaseSymbols.Count; i++ )
				{
					Symbol symbol =  new Symbol();
					symbol.Name = memoryPhaseSymbols[i].Name;
					symbol.BackgroundColor = memoryPhaseSymbols[i].BackgroundColor;
					symbol.Rune = memoryPhaseSymbols[i].Rune;

				 	symbolsToSwitch.Add( symbol );
					 colourSwitchList.Add( memoryPhaseSymbols[i].BackgroundColor.Color );
				 }

				//Load List of colours
				// for( int i = 0; i< symbolsToSwitch.Count; i++ )
				// {
				// 	colourSwitchList.Add( symbolsToSwitch[i].BackgroundColor );
				// }

				//Randomize Colours for the 5 symbols
				foreach( Symbol symbol in symbolsToSwitch )
				{
					
					//Remove the colour that matches the current symbol colour
					for( int x = 0; x < colourSwitchList.Count; x++ )
					{
						if( colourSwitchList.Contains( symbol.BackgroundColor.Color) )
						{
							tmp = colourSwitchList[x];
							colourSwitchList.RemoveAt( x );
							foundMatch = true;
						}
					}

					//Pick a colour from the remaining coloours
					rand = Random.Range( 0, colourSwitchList.Count - 1 );

					symbol.BackgroundColor.Color = colourSwitchList[ rand ];
				
					//Remove the random Colour
					colourSwitchList.RemoveAt( rand );

					//Add the Colour that matched the current symbol back into the list
					if( foundMatch )
					{
						colourSwitchList.Add( tmp );
						foundMatch = false;
					}

				}

				for( int x = 0; x < symbolsToSwitch.Count; x++ )
				{
					//Debug.Log( "Names : " + memoryPhaseSymbols[i].Name  + " " + memoryPhaseSymbols[x].Name );
					
					isSelectable = true;
					//Debug.Log( "No Match Found So im going to do my thing...." );

					Debug.Log( symbolsToSwitch[x].Name + " " + symbolsToSwitch[x].BackgroundColor );
						
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
								
							//Set the background colour
							memorySymbolsScript.BackgroundColor.GetComponent<Image>().color =  symbolsToSwitch[x].BackgroundColor.Color;
								
							//Set the symbol
							memorySymbolsScript.Rune.GetComponent<Image>().sprite = symbolsToSwitch[x].Rune.sprite;
								
							//Mark this memory symbol as colour switched
							memorySymbolsScript.IsColourSwitched = true;
								
							isSelectable = false;
	
						}
					
					}
					
				}

				//Randomly insert the Symbols into the level.
			
				//ColourSwitchSymbols();

				colourSwitchList.Clear();
				symbolsToSwitch.Clear();
		}



		//Switch Colour of symbols when symbol count is 3 or less.
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
								
								//Set the background colour
								memorySymbolsScript.BackgroundColor.GetComponent<Image>().color = memoryPhaseSymbols[x].BackgroundColor.Color;
								
								//Set the symbol
								memorySymbolsScript.Rune.GetComponent<Image>().sprite = memoryPhaseSymbols[i].Rune.sprite;
								
								//Mark this memory symbol as colour switched
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


