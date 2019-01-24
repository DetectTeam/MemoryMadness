using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MemoryMadness
{
	public class TutorialManager : Singleton<TutorialManager> 
	{
		//ints

		[SerializeField] private int currentLevel = 0;
		public int CurrentLevel { get { return currentLevel; } set{ currentLevel = value; } }
		[SerializeField] private int errorCount = 0;
		public int ErrorCount { get{ return errorCount; } set{ errorCount = value; } }
		[SerializeField] private int correctCount = 0;
		public int CorrectCount { get{ return correctCount; } set{ correctCount = value; } }
		[SerializeField] private int dialogCount = 0;
		[SerializeField] private int btnClickCount = 0;
		//End ints

		//bools
		private bool isButtonNeeded = true;
		public bool IsButtonNeeded { get{ return isButtonNeeded; } set{ isButtonNeeded = value; } }
		//End bools
		
		//GameObjects
		[SerializeField] private GameObject score;
		[SerializeField] private GameObject heart;
		[SerializeField] private GameObject gameSymbolContainer;
		[SerializeField] private GameObject[] highlights;
		[SerializeField] private GameObject[] tutSymbols;
		[SerializeField] private GameObject continueButton;
		[SerializeField] private GameObject[] memorySymbolsSets;
		[SerializeField] private GameObject[] gameSymbolsSets;

		[SerializeField] private GameObject endLevelCover;
		//Gameobjects End

		//Bools
		[SerializeField] private Button btnContinue;
		[SerializeField] private Button tutOneButtonOne;
		//Bools End
	
		private void OnEnable()
		{
			Messenger.AddListener( "IncorrectSelection" , UpdateErrorCount );
			Messenger.AddListener( "CorrectSelection" , UpdateCorrectCount );
			Messenger.AddListener( "ToggleGameSymbols", ToggleGameSymbols );
			Messenger.AddListener( "IncrementDialogCount", IncrementDialogCount );

			Messenger.MarkAsPermanent( "IncorrectSelection" );
			Messenger.MarkAsPermanent( "CorrectSelection" );	
			Messenger.MarkAsPermanent( "ToggleGameSymbols" );	
			Messenger.MarkAsPermanent(  "IncrementDialogCount" );	

			btnContinue.onClick.AddListener( TaskOnClick );
			tutOneButtonOne.onClick.AddListener( TutOneButtonOneClick );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( "IncorrectSelection" , UpdateErrorCount );
			Messenger.RemoveListener( "CorrectSelection" , UpdateCorrectCount );
			Messenger.RemoveListener( "ToggleGameSymbols", ToggleGameSymbols );	
			Messenger.RemoveListener( "IncrementDialogCount", IncrementDialogCount );

			btnContinue.onClick.RemoveListener( TaskOnClick );
			tutOneButtonOne.onClick.RemoveListener( TutOneButtonOneClick );
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
			{
				ToggleSymbolHighlights( 2 );
				isButtonNeeded = false;
			}
			
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
				heart.transform.localScale = new Vector3( 0.5f, 0.5f, 0.5f );

				Messenger.Broadcast( "RemoveHeart" );
				

				ToggleSymbolHighlights( 3 );
				isButtonNeeded = false;
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

		public void DecreaseScore()
		{
			Messenger.Broadcast<int>( "DecreaseScore" , 100 );
		}

		public void BuildTutorialLevel( int index )
		{
			Debug.Log( index );

			//Disable All Game Symbol Sets
			for( int x = 0; x < gameSymbolsSets.Length; x ++ )
			{
				gameSymbolsSets[ x ].SetActive( false );
				memorySymbolsSets[ x ].SetActive( false );
			} 

			//Set the level needed to true
			gameSymbolsSets[ index ].SetActive( true );
			memorySymbolsSets[ index ].SetActive( true );
		}

		private void CheckCorrectCount()
		{
			if( correctCount == 2 )
			{
				Debug.Log( "Trigger Success" );
				EnableBackground();
			}
		}

		private void CheckErrorCount()
		{	
			if( errorCount == 2 )
			{
				Debug.Log( "You Lose" );
				EnableBackground();
			
			}
		}

		public void UpdateCorrectCount()
		{
			correctCount ++;
			CheckCorrectCount();
		}

		public void UpdateErrorCount()
		{
			errorCount ++;
			Debug.Log( "Update Error count Called... " + errorCount );
			CheckErrorCount();
		}

		public void RemoveHeart()
		{
			Messenger.Broadcast( "RemoveHeart" );
		}

		public void EnableBackground()
		{
			endLevelCover.transform.SetSiblingIndex( 2 );
			endLevelCover.SetActive( true );
			
			if( currentLevel >= 3 )
				DialogueManager.Instance.IsSectionComplete = true;
		}

		public void DisableBackground()
		{
			endLevelCover.SetActive( false );
		}

		public void ResetCounts()
		{
			errorCount = 0;
			correctCount = 0;
		}
	}
}
