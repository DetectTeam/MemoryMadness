using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryMadness
{
	public class TutorialManager : Singleton<TutorialManager> 
	{
		[SerializeField] private GameObject gameSymbolContainer;

		[SerializeField] private GameObject[] highlights;

		[SerializeField] private int dialogCount = 0;

		[SerializeField] private GameObject continueButton;

		[SerializeField] private GameObject[] memorySymbolsSets;

		[SerializeField] private GameObject[] gameSymbolsSets;

		private bool isButtonNeeded = true;
		public bool IsButtonNeeded { get{ return isButtonNeeded; } set{ isButtonNeeded = value; } }
		
		private void OnEnable()
		{
			Messenger.AddListener( "ToggleGameSymbols", ToggleGameSymbols );
			Messenger.AddListener( "IncrementDialogCount", IncrementDialogCount );	
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( "ToggleGameSymbols", ToggleGameSymbols );	
			Messenger.RemoveListener( "IncrementDialogCount", IncrementDialogCount );
		}

		private void Start()
		{
			if( !gameSymbolContainer )
			{
				Debug.Log( "Game Symbol Container Not set" );
				return;
			}

			if( highlights.Length <= 0 )
			{
				Debug.Log( "HighLights Not set" );
				return;
			}
		}

		public void ToggleGameSymbols()
		{
			if( !gameSymbolContainer.activeSelf )
				gameSymbolContainer.SetActive( true );
			else
				gameSymbolContainer.SetActive( false );
		}

		public void ToggleSymbolHighlights( int index )
		{
			IncrementDialogCount();
	
			if( dialogCount == 4 || dialogCount == 5 )
			{
				isButtonNeeded = false;
				continueButton.SetActive( false );
				highlights[ index ].SetActive( true );
			}
				
		}

		public void IncrementDialogCount()
		{
			dialogCount ++;

			if( dialogCount == 6 )
				isButtonNeeded = true;
		}

		public void EnableContinueButton()
		{
			if( !isButtonNeeded )
				return;
		
			continueButton.SetActive( true );
		}
		public void DisableContinueButton()
		{
			if( !isButtonNeeded )
				return;
			
			continueButton.SetActive( false );
		}

		public void EnableMemorySymbols( int index  )
		{
			memorySymbolsSets[index].SetActive( true );
		}
		public void DisableMemorySymbols( int index )
		{
			memorySymbolsSets[index].SetActive( false );
		}

		public void EnableGameSymbols( int index  )
		{
			gameSymbolsSets[ index ].SetActive( true );
		}
		public void DisableGameSymbols( int index )
		{
			gameSymbolsSets[ index ].SetActive( false );
		}
	}
}
