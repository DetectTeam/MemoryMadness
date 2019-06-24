using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoRoutineSlave : MonoBehaviour
{
	public static CoRoutineSlave Instance;

	private void Awake()
	{
		if (Instance == null)
       		Instance = this;
       	else if (Instance != this)
            Destroy(gameObject);  
	}
	
	public IEnumerator ExecCoroutine( IEnumerator coRoutine )
	{
		StartCoroutine( coRoutine );

		return coRoutine;
	}
}
