using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MemoryMadness;

public class Home : MonoBehaviour 
{
	
	//[SerializeField] private GameObject animMachine;
	
	public void GoToMainMenu(  )
	{
		
		//TutorialManager.Instance.IsExit = true;
		
		//var animator = animMachine.GetComponent<Animator>();
		//animator.SetInteger( "Tutorial" , 99 );
		//DialogueManager.Instance.IsSectionComplete = true;

		//animator.enabled = false;
		Debug.Log( "Heading Home...." );
		SceneManager.LoadScene( "Memory_Madness_Random_Level" );

		
	}

	private IEnumerator ieLeave( )
	{
		yield return new WaitForSeconds( 1.0f );
		

		
	}

	
}
