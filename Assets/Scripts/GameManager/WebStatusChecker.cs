using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebStatusChecker : Singleton<WebStatusChecker>
{
	
	private bool webAccessStatus = false;

	private void Start()
	{
		StartCoroutine( "WebCheck" );
	}

	private IEnumerator WebCheck()
	{
		Debug.Log( "Starting Web Check...." );

		while( true )
		{
			yield return new WaitForSeconds( 1.0f );
			InternetReachabilityCheck();
		}
	}

	private void InternetReachabilityCheck()
	{
		 //Check if the device cannot reach the internet
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            webAccessStatus = false;
        }
        //Check if the device can reach the internet via a carrier data network
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
			webAccessStatus = true;
        }
        //Check if the device can reach the internet via a LAN
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            webAccessStatus = true;
        }

		Messenger.Broadcast<bool>( "WebAccessStatus", webAccessStatus );
	}
}
