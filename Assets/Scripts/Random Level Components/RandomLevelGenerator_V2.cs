using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Randomly Generates a game level consisting of 20 selectable symbols 
/// </summary>
namespace  MemoryMadness
{
	
	public class RandomLevelGenerator_V2 : Singleton<RandomLevelGenerator_V2>
	{

		 
		[SerializeField] private ColourPicker colorPicker;  //List of colours used in the level
		[SerializeField] private ShapePicker namedShapePicker; //nameable sprites 
		[SerializeField] private ShapePicker unamedShapePicker; //unanameable sprites
		[SerializeField] private List<Shape> namedShapes; //List of nameable shapes
		[SerializeField] private List<Shape> unamedShapes; //List of unameable shapes
		[SerializeField] private List<GameObject> levelSymbols; //List of the symbols displayed in the level
		[SerializeField] private List<Color>levelBackGroundColors = new List<Color>();
		[SerializeField] private List<Colour> backgroundColours = new List<Colour>();
		[SerializeField] private GameObject symbolPrefab;
		[SerializeField] private int currentStage = 2;
		[SerializeField] private int memPhaseSymbolCount;
		[SerializeField] private List<Symbol> memoryPhaseSymbols;
			public List<Symbol> MemoryPhaseSymbols { get{ return memoryPhaseSymbols; } }

		
		
		private void OnEnable()
		{
			//Reset the correct button count to zero
			Messenger.Broadcast( "ResetCorrectButtonCount" );

			CheckCurrentStage( StageManager.Instance.CurrentStage );

			LoadLists();

			SetBackgroundColours();
			
			SetupSymbols();
		}

		
		//Load all colour and shape lists
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

	
		//Select the symbol backgroudn colours for current level
		private void SetBackgroundColours()
		{
			List<Colour> tmpList = colorPicker.GetColourList();

			if( backgroundColours.Count > 0 )
				backgroundColours.Clear();
			
			for( int i = 0; i < 4; i++ )
			{
				backgroundColours.Add( tmpList[i] );
			}
		}

		//Generate the memory phase symbols 
		private void SetupSymbols()
		{
			bool isNoColour = false;
			
			if( levelSymbols.Count > 0 )
			{
				if( StageManager.Instance.CurrentLevelType == LevelType.UnNameableNonColour || StageManager.Instance.CurrentLevelType == LevelType.NameableNonColour )
				{
					CreateSymbols( true );	
					GenerateMemoryPhaseSymbolsNonColoured();
				}
				else
				{
					CreateSymbols( false );
					GenerateMemorySymbolsColoured();
				}

				//Only perform a colour switch on levels with coloured symbols
				if( StageManager.Instance.CurrentLevelType == LevelType.NameableColour || StageManager.Instance.CurrentLevelType == LevelType.UnNameableColour )
				{
					if( memoryPhaseSymbols.Count <= 3 )
						ColourSwitchSymbols();
				}
			}
		}


		
		//Create the symbols for the level , Assigns colour and sprites...
		private void CreateSymbols( bool isNoColour )
		{
			List<Shape> levelSprites;

			if( levelBackGroundColors.Count > 0 )
				levelBackGroundColors.Clear();

			//Create a list of size 20 with 10 of each background colour
			for( int i = 0; i < 20; i++ )
			{
				if( i >= 0 && i <= 9  )
					levelBackGroundColors.Add( 	backgroundColours[0].Color );
				else
					levelBackGroundColors.Add( backgroundColours[1].Color );
			}

			levelBackGroundColors.ShuffleList();
	
			if( isNoColour )
			{
				levelSprites = unamedShapes;
			}
			else
			{
				levelSprites = namedShapes;
			}
			
			for( int i = 0; i < levelSymbols.Count; i++  )
			{
				var memSymbolsScript = levelSymbols[i].transform.gameObject.GetComponent<MemorySymbols>();
				
				memSymbolsScript.Reset();

				memSymbolsScript.Name = "MemorySymbol_" + i;

			    memSymbolsScript.SlotNumber = i+1;

				memSymbolsScript.IsCorrect = false;
				memSymbolsScript.IsColourSwitched = false;

				int colourPickerIndex = 0;
				int shapeIndex = 0;
					
				if( !isNoColour  )
					levelSymbols[i].transform.Find( "BackgroundColor" ).GetComponent<Image>().color = levelBackGroundColors[i];
				else
					levelSymbols[i].transform.Find( "BackgroundColor" ).GetComponent<Image>().color = backgroundColours[0].Color;
	
				
				levelSymbols[i].transform.Find( "Rune" ).GetComponent<Image>().sprite = RandomShapePicker( i , levelSprites ).Image;
				//memSymbolsScript.ShapeCode = RandomShapePicker( i, levelSprites ).ShapeCode;

				levelSymbols[i].SetActive( true );
			}
		}


		[SerializeField] private List<GameObject> sameColourSymbols;
		private void GenerateMemorySymbolsColoured( )
		{
			GameObject memorySymbol = null;
			int count = 0;
			
			if( memoryPhaseSymbols.Count > 0 )
				memoryPhaseSymbols.Clear();

			//Pick symbol of different colour from symbol list.
			for( int x = 0; x <  memPhaseSymbolCount; x++ )
			{

				sameColourSymbols = SearchByColour( backgroundColours[x].Color );
				memorySymbol = sameColourSymbols[ Random.Range( 0, sameColourSymbols.Count - 1 ) ];

				var memorySymbolsScript = memorySymbol.GetComponent<MemorySymbols>();
		
				Symbol symbol = new Symbol();
				Colour c = new Colour();
				Shape s = new Shape();

				symbol.Index = count;
				symbol.Name = memorySymbolsScript.Name;
				
				c.Color = memorySymbolsScript.BackgroundColor.GetComponent<Image>().color;
				c.ColourName = "";
				c.ColourCode = memorySymbolsScript.ColourCode;

				s.ShapeCode = memorySymbolsScript.ShapeCode;

				symbol.BackgroundColor = c;
				symbol.CurrentShape = s;
				symbol.Rune = memorySymbolsScript.Rune.GetComponent<Image>();
					
				memorySymbolsScript.IsCorrect = true;
				memorySymbolsScript.IsColourSwitched = true;
					
				memoryPhaseSymbols.Add( symbol );

				count ++;
			}		
		}

		private void GenerateMemoryPhaseSymbolsNonColoured()
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
					//Debug.Log( rand );
					pickedNumberList.Add( rand );
					PickColourAndShape( rand, count );
					count ++;
				}
				else if( !pickedNumberList.Contains( rand )  )
				{	
					pickedNumberList.Add( rand );
					PickColourAndShape( rand, count );
					count++;	
				}
			}
			
			count = 0;
			//levelSymbols.ShuffleList();
		}

			private void PickColourAndShape( int rand,  int index )
		{
			var randomSymbol = levelSymbols[ rand ] ;
			var memorySymbolsScript = randomSymbol.GetComponent<MemorySymbols>();
			
			Symbol symbol = new Symbol();
			Colour c = new Colour();
			Shape s = new Shape();

			symbol.Index = index;
			symbol.Name = memorySymbolsScript.Name;
		
			c.Color = memorySymbolsScript.BackgroundColor.GetComponent<Image>().color;
			c.ColourName = "";
			c.ColourCode = memorySymbolsScript.ColourCode;

			s.ShapeCode = memorySymbolsScript.ShapeCode;

			symbol.BackgroundColor = c;
			symbol.CurrentShape = s;
			symbol.Rune = memorySymbolsScript.Rune.GetComponent<Image>();
			
			memorySymbolsScript.IsCorrect = true;
			memorySymbolsScript.IsColourSwitched = true;
			
			memoryPhaseSymbols.Add( symbol );
		
		}

		private Shape RandomShapePicker( int currentIndex, List<Shape> shapes )
		{
			if( currentIndex >= shapes.Count )
			{
				currentIndex = Random.Range( 0, shapes.Count - 1 );
			}

			return shapes[ currentIndex ];
		}

	

		private void CheckCurrentStage( int currentStage )
		{
			if( currentStage <= 2 )
				memPhaseSymbolCount = 2;
			else if( currentStage == 3  )
				memPhaseSymbolCount = 3;
			else if( currentStage >=  4 )
				memPhaseSymbolCount = 4;
			//else if( currentStage > 4 )
				//memPhaseSymbolCount = 5;

			//Set the win count value in the game manager
			Messenger.Broadcast<int>( "SetWinCount" , memPhaseSymbolCount );
			//Messenger.Broadcast( "ResetHearts" );					
		}

		private List<GameObject> SearchByColour( Color color )
		{
			List<GameObject> list = new List<GameObject>();
			
			foreach( GameObject obj in levelSymbols )
			{
				
				if( color == obj.transform.Find( "BackgroundColor" ).GetComponent<Image>().color )
					list.Add( obj );
			}

			return list;
		}

		private void ColourSwitchSymbols()
		{
			Debug.Log( "Colour Switching Symbols...." );
			
			bool  isSelectable = false;
			
			for( int i = 0; i < memoryPhaseSymbols.Count; i++ )
			{
				for( int x = 0; x < memoryPhaseSymbols.Count; x++ )
				{
					if( memoryPhaseSymbols[i].Name != memoryPhaseSymbols[x].Name )
					{
						isSelectable = true;
						
						while( isSelectable )
						{
							//Pick a random symbol from the list.
							GameObject selectedMemorySymbol = levelSymbols[ Random.Range( 0, levelSymbols.Count ) ];
							//Debug.Log( "SELECTED SYMBOL : " + selectedMemorySymbol );
							MemorySymbols memorySymbolScript = selectedMemorySymbol.GetComponent<MemorySymbols>();
							
							//If the random symbol isnt a winning symbol 
							//and it hasnt already been colour switched
							//And the background colours of each symbol are different.
							if( !memorySymbolScript.IsCorrect 
								&& !memorySymbolScript.IsColourSwitched 
								&& memoryPhaseSymbols[i].BackgroundColor.Color != memorySymbolScript.BackgroundColor.GetComponent<Image>().color )
							{
								//Set the symbol
								memorySymbolScript.Rune.GetComponent<Image>().sprite = memoryPhaseSymbols[i].Rune.sprite;
								
								//Mark this memory symbol as colour switched
								memorySymbolScript.IsColourSwitched = true;
								
								isSelectable = false;
							}		
						}
					}
				}
			}
		}	
	}
}
