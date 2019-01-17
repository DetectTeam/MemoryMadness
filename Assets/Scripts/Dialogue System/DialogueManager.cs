using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager> 
{
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI dialogueText;
	[SerializeField] private Queue<string> sentences; 
	[SerializeField] private Animator animator;

	private void Start () 
	{
		sentences = new Queue<string>();
	}
	
	public void StartDialogue( Dialogue dialogue )
	{
		Debug.Log( "Starting Dialogue..." );
		nameText.text = dialogue.name;

		sentences.Clear();

		foreach( string sentence in dialogue.sentences )
		{
			sentences.Enqueue( sentence );
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		Debug.Log( sentences.Count );
		
		if( sentences.Count == 0 )
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine( TypeSentence( sentence ) );
	}

	private IEnumerator TypeSentence( string sentence )
	{
		dialogueText.text = "";

		foreach( char letter in sentence.ToCharArray() )
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	private void EndDialogue()
	{
		Debug.Log( "Ending Dialogue" );
		animator.SetBool( "IsOpen", false );
	}
}
