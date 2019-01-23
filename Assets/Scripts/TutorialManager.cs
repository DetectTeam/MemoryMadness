using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MemoryMadness
{
	public class TutorialManager : Singleton<TutorialManager> 
	{
		[SerializeField] private GameObject gameSymbolContainer;

		[SerializeField] private GameObject[] highlights;
		[SerializeField] private GameObject[] tutSymbols;

		[SerializeField] private int dialogCount = 0;

		[SerializeField] private GameObject continueButton;

		[SerializeField] private GameObject[] memorySymbolsSets;

		[SerializeField] private GameObject[] gameSymbolsSets;

		[SerializeField] private Button btnContinue;
		[SerializeField] private Button tutOneButtonOne;

		[SerializeField] private GameObject score;
		[SerializeField] private GameObject heart;
	
		private bool isButtonNeeded = true;
		public bool IsButtonNeeded { get{ return isButtonNeeded; } set{ isButtonNeeded = value; } }
		
		private void OnEnable()
		{
			Messenger.AddListener( "ToggleGameSymbols", ToggleGameSymbols );
			Messenger.AddListener( "IncrementDialogCount", IncrementDialogCount );	

			btnContinue.onClick.AddListener( TaskOnClick );
			tutOneButtonOne.onClick.AddListener( TutOneButtonOneClick );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( "ToggleGameSymbols", ToggleGameSymbols );	
			Messenger.RemoveListener( "IncrementDialogCount", IncrementDialogCount );

			btnContinue.onClick.RemoveListener( TaskOnClick );
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
			Debug.Log( "TOGGLE SYMBOL HIGHLIGHTS CALLED ...." );
			
			IncrementDialogCount();
	
			continueButton.SetActive( false );
				//highlights[ index ].SetActive( true );
			   
			tutSymbols[index].GetComponent<MemorySymbolsTutorial>().Interactive = true;
		
			var button = tutSymbols[index].transform.Find( "button" ).gameObject;

			button.GetComponent<Button>().interactable = true;;
			   
			var highLight = tutSymbols[index].transform.Find( "HighLight" ).gameObject;
			highLight.SetActive( true );
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

		[SerializeField] private int btnClickCount = 0;
		private void TaskOnClick()
		{
			btnClickCount++;
			Debug.Log( "Continue Button Clicked " + btnClickCount + " times." );

			if( btnClickCount == 4 )
			{
				isButtonNeeded = false;
				ToggleSymbolHighlights( 0 );
			}

			if( btnClickCount == 7 )
				ToggleSymbolHighlights( 2 );
			
			if( btnClickCount == 8 )
			{
				Messenger.Broadcast<int>( "DecreaseScore" , 100 );
				score.transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
				iTween.Stop();

				ScaleTo( heart );
			}

			if( btnClickCount == 9 )
			{
				iTween.Stop();
				heart.transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );

				Messenger.Broadcast( "RemoveHeart" );

				ToggleSymbolHighlights( 3 );
			}

		}

		private void TutOneButtonOneClick()
		{
			ToggleSymbolHighlights( 1 );
		}

		public void ScaleTo( GameObject target )
		{
			iTween.ScaleTo( target, iTween.Hash( 
			"scale", new Vector3( 1.15f, 1.15f, 1.15f ),
			"time", 0.5f,
			"loopType", "pingPong"
			 ));   
		}

		public void StopScaleTo( GameObject target )
		{
			Debug.Log( "Stopping .... " + target.name );
			iTween.Stop( target );
			target.transform.localScale = new Vector3( 1f,1f,1f );
		}

		public void IncreaseScore()
		{
			Messenger.Broadcast<int>( "IncreaseScore" , 100 );
		}

		public void BuildTutorialLevel( int index )
		{

		}

	
	}
}
