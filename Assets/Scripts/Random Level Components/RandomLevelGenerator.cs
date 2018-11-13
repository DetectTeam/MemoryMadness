using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MemoryMadness
{
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

		[SerializeField] private List<GameObject> memoryPhaseSymbols;
		[SerializeField] private GameObject symbolPrefab;

	     
		
		// Use this for initialization
		
		private void OnEnable()
		{
			
			Debug.Log( "OnEnable Function Called..." );
			//LoadLists();
			//CreateSymbols();
			SetupSymbols();
			
		}

		private void OnDisable()
		{
			Debug.Log( "OnDisable Function Called..." );
			//DisableSymbols();
		}
		
		
		private void Start () 
		{
			Debug.Log( "Start Function Called..." );
			LoadLists();
			InitSymbols();
			
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

		private void InitSymbols()
		{
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
			}
		}

		private void UpdateSymbols()
		{
			for( int i = 0; i < cloneSymbols.Count; i++ )
			{
				cloneSymbols[i].transform.Find( "BackgroundColor" ).GetComponent<Image>().color = colorPicker.ColourList[ i ];
				
				cloneSymbols[i].transform.Find( "Rune" ).GetComponent<Image>().sprite = unamedShapes[i];

				cloneSymbols[i].SetActive( true );

				cloneSymbols[i].transform.parent = anchors[ i ].transform.parent;
			}
		}

		private void GenerateMemoryPhaseSymbols()
		{
			GameObject randomSymbol;
			int memPhaseSymbolCount = 3;
			//Randomly select 2 - 5 ( depending on which stage we are on ) symbols from the clone list
			//ie the list of symbols that will be displayed on the game screen
			if( memoryPhaseSymbols.Count > 0 )
				memoryPhaseSymbols.Clear();

			for( int i = 0; i < memPhaseSymbolCount; i++ )
			{
				randomSymbol = cloneSymbols[ Random.Range( 0, 19 ) ] ;
				randomSymbol.GetComponent<MemorySymbols>().IsCorrect = true;
				
				memoryPhaseSymbols.Add( Instantiate( randomSymbol, randomSymbol.transform.position, Quaternion.identity ) );
				memoryPhaseSymbols[i].GetComponent<MemorySymbols>().DisableButton();
			}

			

			//Store these symbols in a list. 

			//Use that list to populate the memory phase page.
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

