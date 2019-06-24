using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MemoryMadness 
{

	public class MemorySymbolGenerator : MonoBehaviour 
	{

		[SerializeField] private GameObject memorySymbolPrefab; 

		[SerializeField] private GameObject[] memoryPhaseSymbols;

		[SerializeField] private Sprite earth;

		[SerializeField] private Color color;

		//private MemorySymbols memorySymbol;

		
		private void Start()
		{
			//memorySymbol = GetComponent<MemorySymbols>();

			//color = RandomColour();

			GenerateMemoryPhaseSymbols( 1 );

		}


		private void GenerateMemoryPhaseSymbols( int count )
		{
			for( int i = 0; i < count; i++ )
			{
				var clone = Instantiate(  memorySymbolPrefab , transform.position, Quaternion.identity );
				MemorySymbols memorySymbols = clone.GetComponent<MemorySymbols>();

				memorySymbols.BackgroundColor.GetComponent<Image>().color = RandomColour();
				memorySymbols.Rune.GetComponent<Image>().sprite = earth;
				clone.transform.parent = transform;

			}
		}

		private Color RandomColour()
		{

			float r,g,b,a = 0f;

			r = Random.Range( 0.1f , 1.0f );
			g = Random.Range( 0.1f , 1.0f );
			b = Random.Range( 0.1f , 1.0f );
			a = 1f;

			return new Color( r, g, b, a );
		}
		
		
	}

}