using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffectsHandler : MonoBehaviour 
{
	[SerializeField] private GameObject effect;
	[SerializeField] private IEffect iEffect;
	[SerializeField] private GameObject target;

	private void Awake()
	{
	
		if( effect == null ) 
		{
			Debug.Log( "Special Effect not set..." );
			return;
		}

		iEffect = effect.GetComponent<IEffect>();

		if( iEffect == null ) 
		{
			Debug.Log( "Interface ie effect not found..." );
			return;
		}
	}

	private void OnEnable()
	{
		Messenger.AddListener( "TriggerEffect", TriggerEffect );
		Messenger.MarkAsPermanent( "TriggerEffect" );
		TriggerEffect();
	}

	private void OnDisable()
	{
		Messenger.RemoveListener( "TriggerEffect",  TriggerEffect );
	}

	private void TriggerEffect()
	{	
		iEffect.Play( target );
	}	
}
