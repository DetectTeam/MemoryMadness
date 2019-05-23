using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionTimer : MonoBehaviour
{
    public static SessionTimer Instance;
    private float timer;
    public float Timer { get{ return timer; } }
    
    // Start is called before the first frame update
    private void Awake()
    {
        	
		//DontDestroyOnLoad(gameObject);
		//Check if instance already exists
        if (Instance == null)
                
                //if not, set instance to this
                 Instance = this;
            
            //If instance already exists and it's not this:
             else if (Instance != this)
                
                 //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                 Destroy(gameObject);    
            
        //     //Sets this to not be destroyed when reloading scene
            // DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
     
    }
}
