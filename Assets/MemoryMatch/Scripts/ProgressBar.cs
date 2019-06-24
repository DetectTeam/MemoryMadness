using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace MemoryMadness
{
	public class ProgressBar : MonoBehaviour 
	{
		[SerializeField] private Image progressBar;
		[SerializeField] private TextMeshProUGUI percentageText;

		[SerializeField] private GameManager gameManager;

		[SerializeField] private Star star1;
		[SerializeField] private Star star2;
		[SerializeField] private Star star3;

		[SerializeField] private GameObject particleContainer;

		[SerializeField] private GameObject buttonContainer;

		//private int speed = 1;

		private void OnEnable()
		{		
			Messenger.AddListener<float>( "Results", Fill );
		}

		private void OnDisable()
		{
			Messenger.RemoveListener<float>( "Results", Fill );
			
			if( particleContainer && particleContainer.activeSelf )
				particleContainer.SetActive( false );

				progressBar.fillAmount = 0;
				percentageText.text = "0%";

				if( buttonContainer )
					buttonContainer.SetActive( false );

		}

		private void Fill( float percentage )
		{
			StartCoroutine( IEFill( percentage ) );
		}

		private IEnumerator IEFill( float percentage )
		{
			float currentValue = 0;
			progressBar.fillAmount = 0f;

			yield return new WaitForSeconds( 2.0f );
			
			while( currentValue < percentage )
			{
				yield return null;
				currentValue ++; //speed; //* Time.deltaTime;
				percentageText.text = currentValue.ToString()  +  "%";
				progressBar.fillAmount = ( currentValue / 133 );

				if( currentValue == 33 )
					star1.Activate();
				
				if( currentValue == 66 )
					star2.Activate();

				if( currentValue == 99 )
					star3.Activate();

			}

			//yield return new WaitForSeconds( 0.5f );

			if( buttonContainer )
				buttonContainer.SetActive( true );

			//particleContainer.SetActive( true );
		}	
		
	}

}
