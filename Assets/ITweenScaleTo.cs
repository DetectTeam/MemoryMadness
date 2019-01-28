using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITweenScaleTo : MonoBehaviour , IEffect {

	[SerializeField] private GameObject target;

	public GameObject Target { get; set; }
	
	[SerializeField] private float time;

	[SerializeField] private iTween.EaseType easeType;



	[SerializeField] Vector3 targetSize;
	
	// Use this for initialization

	private void Start()
	{
		if( !target )
		{
			//Debug.Log( "target not set..." ); 
			return;
		} 
	}
	
	public void Play(  GameObject obj )
	{
		ScaleTo( obj );
	}

	private void ScaleTo( GameObject target)
	{
		iTween.ScaleTo( target, iTween.Hash( "time", time, "scale", targetSize, "easeType", easeType) );
	}
	
	// Update is called once per frame
}
