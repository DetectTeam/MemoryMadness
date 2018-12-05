using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryMadness
{
	public class HeartContainer : MonoBehaviour 
	{

		[SerializeField] private GameObject[] hearts;

		private const string currentStageStr = "CurrentStage";
		[SerializeField] private int heartCount = 0;

		[SerializeField] private int activeHearts = 0;
		private void OnEnable()
		{
			Messenger.AddListener( "RemoveHeart", HideHeart );
			Messenger.AddListener( "ResetHearts", DisplayHearts );
			//Messenger.AddListener<int>( "DisplayHearts" , DisplayHearts );
			ResetHearts();
			DisplayHearts();
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( "RemoveHeart", HideHeart );
			Messenger.RemoveListener( "ResetHearts", DisplayHearts );
			//Messenger.RemoveListener<int>( "DisplayHearts" , DisplayHearts );
		}
		
	
		private void DisplayHearts( )
		{
			 int currentStage = 2;

			if( PlayerPrefs.HasKey( currentStageStr ) )
				currentStage = PlayerPrefs.GetInt( currentStageStr );


			if( currentStage <= 2 )
				heartCount = 2;
			else if( currentStage > 5 )
				heartCount = 5;
			else
				heartCount = currentStage;

			for( int i = 0; i < heartCount; i++ )
			{
				hearts[ i ].SetActive( true );

			}

			activeHearts = heartCount;
			
		}

		private void HideHeart()
		{
			hearts[ activeHearts - 1 ].SetActive( false );
			activeHearts --;

			if( activeHearts < 1 )
				activeHearts = 1;
		}

		private void ResetHearts()
		{
			foreach( GameObject heart in hearts )
			{
				heart.SetActive( false );
			}
		}



		
		
		
	}

}
