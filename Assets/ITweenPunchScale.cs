using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITweenPunchScale : MonoBehaviour , IEffect
{

	[SerializeField] private GameObject target;

	public GameObject Target { get; set; }
	
	[SerializeField] private float time;


	[Range( 0.25f, 2.0f ) ]
	[SerializeField] private float x,y;
	
	// Use this for initialization

	private void Start()
	{
		if( !target )
		{
			Debug.Log( "target not set..." ); 
			return;
		} 
	}
	public void Play(  GameObject obj )
	{
		PunchScale( obj );
	}

	private void PunchScale( GameObject target)
	{
		iTween.PunchScale( target, iTween.Hash( "x", x, "y", y, "time", time));
	}
	
	// Update is called once per frame
	
}
