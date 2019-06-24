using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryMadness
{
	public class ParticleContoller : MonoBehaviour 
	{

		[SerializeField] private ParticleSystem pSystem;

		[SerializeField] private Gradient particleColorGradient;

		private void OnEnable()
		{
			pSystem = GetComponent<ParticleSystem>();
			ConfigureParticle();
		}

		// Use this for initialization
		private void Start () 
		{
			//pSystem = GetComponent<ParticleSystem>();
		}
		
		private void ConfigureParticle()
		{
			var main = pSystem.main;

			main.startColor = particleColorGradient.Evaluate (Random.Range (0f, 1f));
			main.startSizeMultiplier = Random.Range( 0.25f, 1.25f  );
			//main.duration = Random.Range( 0.5f, 2.5f );
			main.gravityModifier  = Random.Range( 0.1f, 0.4f );
			
			

		}

		private Color RandomColor()
		{
			float r,g,b,a = 0;
			
			r = Random.Range( 0.2f , 1.0f );
			g = Random.Range( 0.1f , 0.7f );
			b = Random.Range( 0.1f , 0.3f );
			a = 1;
			
			return new Color( r, g, b, a );
		}
	}

}
