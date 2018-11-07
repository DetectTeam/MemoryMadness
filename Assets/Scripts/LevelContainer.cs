using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryMadness
{
	public class LevelContainer : MonoBehaviour 
	{
		[SerializeField] private GameObject[] rows;
	//	[SerializeField] private GameObject gameManager;

	//	[SerializeField] private BoardManager boardManager;

		[SerializeField] private int heartCount;

		private const string currentStageStr = "CurrentStage";

		void OnEnable()
		{
			StartCoroutine( Sequence() );

			
		}

		void OnDisable()
		{
			gameObject.SetActive( false );
		}

		
		private IEnumerator Sequence()
		{
			int count = 0;
			yield return null;

			while( count < rows.Length )
			{
				yield return new WaitForSeconds( 0.2f );
			
				rows[ count ].SetActive( true );
				count ++;
			}

			
		}

		
		
	}
}
