using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebStatusChecker : Singleton<WebStatusChecker>
{
	
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
            //Change the Text
            Debug.Log( "Not Reachable." );
        }
        //Check if the device can reach the internet via a carrier data network
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            Debug.Log( "Reachable via carrier data network." );
        }
        //Check if the device can reach the internet via a LAN
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Debug.Log( "Reachable via Local Area Network." );
        }
	}
}
