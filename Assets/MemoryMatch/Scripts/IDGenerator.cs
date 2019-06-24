using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDGenerator : MonoBehaviour
{

    public static IDGenerator Instance;
    
    [SerializeField] private string userId; 

    public string UserID { get{ return userId; } set{ userId = value; } }

    
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)  
            //if not, set instance to this
            Instance = this; 
        //If instance already exists and it's not this:
        else if (Instance != this)   
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);   
        
        userId = System.Guid.NewGuid().ToString().Substring( 0, 8 );
    }
}
