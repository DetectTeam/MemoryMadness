using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour 
{
	[SerializeField] private GameObject particle;

	private ParticleSystem pSystem;


	private void OnEnable()
	{
		StartCoroutine( "SpawnParticle" );
	}

	private void OnDisable()
	{
		StopCoroutine( "SpawnParticle" );
		
	}

	private IEnumerator SpawnParticle()
	{
		float delay = 0;
		float x, y, z = 0;
		float r, g, b, a = 0;
		
		yield return null;

		while( true )
		{
			x = Random.Range( -2.5f, 2.5f );
			y = Random.Range( -4.5f, 4.5f );
			z = -10;

			delay = Random.Range( 0.5f, 2.0f );

			GameObject p = Instantiate( particle, new Vector3( x, y, z ) , Quaternion.identity );
		

			yield return new WaitForSeconds( delay );
		}
	}
	
	// Update is called once per frame
}
