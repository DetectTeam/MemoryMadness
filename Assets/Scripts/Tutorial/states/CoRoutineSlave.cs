using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoRoutineSlave : Singleton<CoRoutineSlave>
{

	public void ExecCoroutine( IEnumerator coRoutine )
	{
		StartCoroutine( coRoutine );
	}
}
