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
			
			Debug.Log( "OnEnable Function Called..." );
			LoadLists();
			
			InitSymbols();
			
			//1 Setup symbols
			SetupSymbols();
			
			//2. display the memory phase page.
			if(  memoryPhaseContainer != null )
				memoryPhaseContainer.SetActive( true );
		
		}

		private void OnDisable()
		{
			Debug.Log( "OnDisable Function Called..." );
			//DisableSymbols();
		}
		
		
		private void Start () 
		{
			//Debug.Log( "Start Function Called..." );
			//LoadLists();
			//InitSymbols();
			
			//SetupSymbols();
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

		private void InitSymbols()
		{
			
			if( cloneSymbols.Count > 0 )
				cloneSymbols.Clear();
			
			for( int i = 0; i < anchors.Count; i++ )
			{
				var clone  = Instantiate( symbolPrefab, anchors[ i ].transform.position, Quaternion.identity );
				clone.transform.parent = anchors[ i ].transform.parent;
				cloneSymbols.Add( clone );
				clone.SetActive( false );
				
			}
		}

		private void SetupSymbols()
		{
			if( cloneSymbols.Count > 0 )
			{
				UpdateSymbols();
				GenerateMemoryPhaseSymbols();
				//ColourSwitchSymbols();
			}
		}

		private void UpdateSymbols()
		{
			for( int i = 0; i < cloneSymbols.Count; i++ )
			{
				cloneSymbols[i].transform.Find( "BackgroundColor" ).GetComponent<Image>().color = colorPicker.ColourList[ i ];
				
				cloneSymbols[i].transform.Find( "Rune" ).GetComponent<Image>().sprite = unamedShapes[i];

				cloneSymbols[i].transform.gameObject.GetComponent<MemorySymbols>().SlotNumber = ( i+1 );

				cloneSymbols[i].SetActive( true );

				cloneSymbols[i].transform.parent = anchors[ i ].transform.parent;
			}
		}

		private void GenerateMemoryPhaseSymbols()
		{
			GameObject randomSymbol;
			int memPhaseSymbolCount = 3;
			int max = 19;
			//Randomly select and copy 2 - 5 ( depending on which stage we are on ) symbols from the clone list
			//ie the list of symbols that will be displayed on the game screen
			
			
			if( memoryPhaseSymbols.Count > 0 )
				memoryPhaseSymbols.Clear();

			for( int i = 0; i < memPhaseSymbolCount; i++ )
			{
				
				int rand = Random.Range( 0, max );
			

				randomSymbol = cloneSymbols[ rand ] ;
				var memorySymbolsScript = randomSymbol.GetComponent<MemorySymbols>();
				//cloneSymbols.RemoveAt( rand );
				//max--;

				Debug.Log( "New Symbol..." );
				Symbol symbol = new Symbol();

				symbol.Name = "Symbol_" + ( i + 1 );
				symbol.BackgroundColor = memorySymbolsScript.BackgroundColor.GetComponent<Image>().color;
				symbol.Rune = memorySymbolsScript.Rune.GetComponent<Image>();

				memorySymbolsScript.IsCorrect = true;

				memoryPhaseSymbols.Add( symbol );

				cloneSymbols.ShuffleList();
				
				
			
			}

		}

		private void ColourSwitchSymbols()
		{
			Debug.Log( "MPS " + memoryPhaseSymbols.Count );
			for( int i = 0; i < memoryPhaseSymbols.Count; i++ )
			{
				 
			}
		}

		private void DisableSymbols()
		{
			foreach( GameObject symbol in cloneSymbols )
			{
				symbol.SetActive( false );	
			}
		}

		

	}

}

