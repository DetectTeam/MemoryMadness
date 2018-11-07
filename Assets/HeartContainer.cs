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

		[SerializeField] private int ActiveHearts = 0;
		private void OnEnable()
		{
			Messenger.AddListener( "RemoveHeart", HideHeart );
			//Messenger.AddListener<int>( "DisplayHearts" , DisplayHearts );
			ResetHearts();
			DisplayHearts();
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( "RemoveHeart", HideHeart );
			//Messenger.RemoveListener<int>( "DisplayHearts" , DisplayHearts );
		}
		// Use this for initialization
		private void Start () 
		{
			
		}

		private void DisplayHearts( )
		{
			int currentStage = 1;

			if( PlayerPrefs.HasKey( currentStageStr ) )
				currentStage = PlayerPrefs.GetInt( currentStageStr );

			heartCount = currentStage + 1;
			
			if( heartCount > 5  )
				heartCount = 5;


			Debug.Log( "Current Number of Hearts To Display is: " + heartCount );

			for( int i = 0; i < heartCount; i++ )
			{
				hearts[ i ].SetActive( true );
			}

			ActiveHearts = heartCount;
			
		}

		private void HideHeart()
		{
			hearts[ ActiveHearts - 1 ].SetActive( false );
			ActiveHearts --;

			if( ActiveHearts < 1 )
				ActiveHearts = 1;
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
