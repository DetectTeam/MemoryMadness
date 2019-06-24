using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour 
{
	[SerializeField] private Vector3 startPosition;
	public Vector3 StartPosition { get{ return startPosition; } set{ startPosition = value; } }

	[SerializeField] private Vector3 target;
	public Vector3 Target { get{ return target; } set{ target = value; } }

	[SerializeField] private float delay = 0.3f;
	public float Delay { get{ return delay; } set{ delay = value; } }

	// Use this for initialization
	
	private void OnEnable()
	{
		StartCoroutine( IEMove() );
	}

	private void OnDisable()
	{
		transform.localPosition = startPosition;
	}

	public void Move()
	{
		StartCoroutine( IEMove() );
	}

	public void Move( float delay, Vector3 startPosition, Vector3 target  )
	{
		this.delay = delay;
		this.startPosition = startPosition;
		this.target = target;

		StartCoroutine( IEMove() );
	}
	private IEnumerator IEMove()
	{
		yield return null;
		
		iTween.MoveTo( gameObject,iTween.Hash( 
			"position", target,
			"islocal" , true,
			"easetype",iTween.EaseType.easeInOutSine,
			"time", delay ));    
	}
	
	
}
