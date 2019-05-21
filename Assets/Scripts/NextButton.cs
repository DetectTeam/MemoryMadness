using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MemoryMadness
{
	public class NextButton : MonoBehaviour 
	{

		public void LoadNextLevel()
		{
			int currentLevel = 0;
			
			if( PlayerPrefs.HasKey( "CurrentLevel" ) )
			{
				currentLevel = PlayerPrefs.GetInt( "CurrentLevel" );
				currentLevel++;

				PlayerPrefs.SetInt( "CurrentLevel" , currentLevel );
			}
		}

		public void CheckStage()
		{
			if( PlayerPrefs.HasKey( "CurrentStage" ) )
			{
				int currentStage = PlayerPrefs.GetInt( "CurrentStage" );
				Debug.Log( "Current Stage : " + currentStage  );

	
				//Remove when testing is over ....!
				if( currentStage > 3 )
				{
					PlayerPrefs.DeleteAll();
					PlayerPrefs.SetInt( "CurrrentStage" , 1  );
					Debug.Log( "Resetting Game .............." );
					SceneManager.LoadSceneAsync( "Memory_Madness_Random_Level" );
				}		
			}
		}

	}

}
