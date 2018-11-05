using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	}

}
