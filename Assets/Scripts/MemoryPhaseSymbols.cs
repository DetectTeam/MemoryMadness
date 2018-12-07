using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MemoryMadness
{
	public class MemoryPhaseSymbols : MonoBehaviour 
	{

		[SerializeField] private List<GameObject> anchors;
		[SerializeField] private List<Symbol> symbols;

		[SerializeField] private RandomLevelGenerator randomLevelGenerator;

		private void OnEnable()
		{
			//Get The Symbols data for this level;
			if( randomLevelGenerator )
			{
				symbols = randomLevelGenerator.MemoryPhaseSymbols;

				if( symbols.Count == 2 )
				{
					gameObject.transform.localPosition = new Vector3( 225, 0, 0 );
				}
				else if( symbols.Count == 3 )
				{
					gameObject.transform.localPosition = new Vector3( 150, 0, 0 );
				}
				else if( symbols.Count == 4 )
				{
					gameObject.transform.localPosition = new Vector3( 75, 0, 0 );
				}
				else if( symbols.Count == 5 )
				{
					gameObject.transform.localPosition = new Vector3( 0, 0, 0 );
				}
				
				for( int i = 0; i < symbols.Count; i++ )
				{
					anchors[i].SetActive( true );
					
					anchors[ i ].GetComponent<MemorySymbols>().BackgroundColor.GetComponent<Image>().color = symbols[i].BackgroundColor.Color;
					anchors[ i ].GetComponent<MemorySymbols>().Rune.GetComponent<Image>().sprite = symbols[i].Rune.sprite;
					anchors[ i ].GetComponent<MemorySymbols>().Name = symbols[i].Name;

				}

				symbols.Clear();
			}
		}
		
	}
}
