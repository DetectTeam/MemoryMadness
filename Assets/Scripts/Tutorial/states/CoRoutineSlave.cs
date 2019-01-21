using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoRoutineSlave : Singleton<CoRoutineSlave>
{
	public IEnumerator ExecCoroutine( IEnumerator coRoutine )
	{
		StartCoroutine( coRoutine );

		return coRoutine;
	}
}
