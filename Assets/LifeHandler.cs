using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MemoryMadness
{

	public class LifeHandler : MonoBehaviour 
	{

		[SerializeField] private int lifeCount = 3;


		private void OnEnable()
		{
			Messenger.AddListener( "DecrementLife" , DecrementLifeCount );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener( "DecrementLife" , DecrementLifeCount );
		}
		
		// Use this for initialization
		void Start () 
		{
			
		}
		
		public void DecrementLifeCount()
		{
			lifeCount --;

			if( lifeCount == 0 )
			{
				Messenger.Broadcast( "Failure" ); //Request failure message
				Messenger.Broadcast( "ChangeLevel" ); //Request level change
				lifeCount = 3; //Reset life count for next level
			}
		}
	}

}
