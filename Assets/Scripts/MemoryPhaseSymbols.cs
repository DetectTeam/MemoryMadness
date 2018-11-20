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
				
				for( int i = 0; i < anchors.Count; i++ )
				{
					anchors[ i ].GetComponent<MemorySymbols>().BackgroundColor.GetComponent<Image>().color = symbols[i].BackgroundColor;
					anchors[ i ].GetComponent<MemorySymbols>().Rune.GetComponent<Image>().sprite = symbols[i].Rune.sprite;
					anchors[ i ].GetComponent<MemorySymbols>().Name = symbols[i].Name;

				}

				symbols.Clear();
			}
		}
		
	}
}
