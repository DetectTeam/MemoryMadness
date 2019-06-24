using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Tutorial1 
{
	public static float delay = 3.0f;

	public static IEnumerator Sequence()
	{
		delay = 4.0f;
		Debug.Log( "Starting Tutorial 1" );
		yield return new WaitForSeconds( delay );
		Debug.Log( "Finished Tutorial One" );
	}
}
