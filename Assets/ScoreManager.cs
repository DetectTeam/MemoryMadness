using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour 
{

	[SerializeField] private TextMeshProUGUI scorePopUpText;
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private int score;
	[SerializeField] private Color successColour;
	[SerializeField] private Color failureColour;

	
	private void OnEnable()
	{
		Messenger.AddListener<int>( "IncreaseScore" , IncrementScore );
		Messenger.AddListener<int>( "DecreaseScore" , DecrementScore );
		Messenger.AddListener<GameObject>( "FloatScore" , FloatingScore );
	}

	private void OnDisable()
	{
		Messenger.RemoveListener<int>( "IncreaseScore" , IncrementScore );
		Messenger.RemoveListener<int>( "DecreaseScore" , DecrementScore );
		Messenger.RemoveListener<GameObject>( "FloatScore" , FloatingScore );
	}

	
	private void IncrementScore( int amt )
	{	
		score = score + amt;
		UpdateScore( score );
		ScorePop();	
	}

	private void DecrementScore( int amt )
	{
		score = score - amt;
		UpdateScore( score );
		
	}

	private void UpdateScore( int score )
	{
		scoreText.text = "000" + score.ToString();
	}

	private void ScorePop()
	{
		iTween.PunchScale( scoreText.gameObject, iTween.Hash( "x",+0.5f, "y",+0.5f, "time",0.75f));
		
	}


	private void FloatingScore( GameObject pos )
	{
		StartCoroutine( IEFloatingScore( pos ) );
	}

	private IEnumerator IEFloatingScore( GameObject pos )
	{
		yield return null;
		var clone  = Instantiate( scorePopUpText.gameObject, pos.transform.position, Quaternion.identity  );
		clone.transform.parent = pos.gameObject.transform;
		clone.GetComponent<TextMeshProUGUI>().color = Color.red;
		clone.SetActive( true );

		iTween.MoveBy( clone, iTween.Hash(
     		"y"   , 100f,
     		"time", 0.65f
 		));

		yield return new WaitForSeconds( 0.75f );
		Destroy( clone );

		
 
	}
		
	

}
