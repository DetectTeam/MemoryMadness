using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : MonoBehaviour//Singleton<DialogueBox>
{
    public static DialogueBox Instance = null;
    [SerializeField] private GameObject dialogueBox;
    public GameObject Dialogue{ get{ return dialogueBox; } set{ dialogueBox = value; } }

    private void Awake()
    {
        if (Instance == null)
             Instance = this;
        else if (Instance != this)
                Destroy(gameObject);   
    }
}
