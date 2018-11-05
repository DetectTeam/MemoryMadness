using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryMadness
{
	public class ButtonContainer : MonoBehaviour 
	{

		[SerializeField] GameObject[] buttons;

		private const string m_GameType = "GameType";

		private void OnEnable()
		{
			Messenger.AddListener<GameType>( m_GameType , LoadButton );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener<GameType>( m_GameType , LoadButton );
		}

		private void LoadButton( GameType gameType )
		{
			if( gameType == GameType.SameDifferent )
			{
				buttons[1].SetActive( false );
				buttons[0].SetActive( true );
			}
			else
			{
				buttons[0].SetActive( false );
				buttons[1].SetActive( true );
			}
		}
	}

}
