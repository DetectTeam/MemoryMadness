using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json; //Json Library
using System.IO;
using UnityEngine.Networking;

public class FileUploadHandler : MonoBehaviour 
{
	private string path = "";
	//private string destinationPath;

	private string jsonString;

	private bool webAccessStatus;


	//private static readonly string PutSessionURL = "https://murmuring-fortress-76588.herokuapp.com/memorymadness/session";
	private static readonly string PutSessionURL = "http://localhost:5000/memorymadness/session";

	private void OnEnable()
	{
		Messenger.AddListener<string>( "PUT" , PUT  );
		Messenger.AddListener<bool>( "WebAccessStatus" , WebAccessStatus );

		Messenger.MarkAsPermanent( "PUT" );
		Messenger.MarkAsPermanent( "WebAccessStatus" );
	}

	private void OnDisable()
	{
		Messenger.RemoveListener<string>( "PUT" , PUT  );
		Messenger.RemoveListener<bool>( "WebAccessStatus" , WebAccessStatus );
	}

	private IEnumerator Start()
	{
		path = GetPath() + "upload/";
		//destinationPath = GetPath() + "sent/";
		Debug.Log( path );

		CheckDirectoryExists( path );	

		yield return new WaitForSeconds( 1.0f );

		if( webAccessStatus )
		{
			Debug.Log( "Uploading Files left in upload directory...." );
			UploadFile();
		}	
	}

	//Upload Current Session File to Server
	public void UploadFile()
	{
		//Search  directory for files
		DirectoryInfo dir = new DirectoryInfo( path );
		FileInfo[] info = dir.GetFiles( "*.dat" );

		if( info.Length <= 0 )
		{
			Debug.Log( "No files found...." );
			return;
		}

		Debug.Log( "Shouldnt see me >:)" );
		//Find all .dat files in the upload directory
		foreach( FileInfo f in info )
		{
			Debug.Log( f.Name );
			//Load the file 
			System.Object obj = PersistenceManager.Instance.Load( f.Name );
			
			//Convert the file to JSON
			jsonString = JsonConvert.SerializeObject( obj );

			//jsonString = JsonUtility.ToJson( obj );

			//Display the file
			//Debug.Log( jsonString );

			PUT( jsonString );
			//TODO

			//Move Uploaded Files to sent directory
			//Only if they were successfully uploaded..
			//File.Move( path + f.Name, destinationPath + f.Name );
		}			
	}

	public void PUT( string jsonStr )
	{
		StartCoroutine( IEPUT( jsonStr ) );
	}


	//Send Put Request to the web server
	//Send the session data as a json string.
	private IEnumerator IEPUT( string jsonStr )
	{

		Debug.Log( "Uploading...Data" );
		Debug.Log( jsonStr);

		if( jsonStr.Length <= 0 )
		{
			Debug.Log( "Json not set..." );
			yield break;
		}	

		UnityWebRequest www = UnityWebRequest.Put( PutSessionURL, jsonStr );
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();

		Debug.Log( "Got this far...." + www.downloadHandler.text );

		//Move Uploaded File
	}


	IEnumerator WaitForRequest( WWW data )
	{
		Debug.Log( "Uploading Json...." );
		yield return data;

		Debug.Log( "Got this far ..." );

		if( data.error != null )
		{
			Debug.Log( data.error );
		}
		else
		{
			Debug.Log( "WWW Request : " + data.text );
		}
	}

		//Return a valid filepath for various devices...
	private static string GetPath()
	{
		#if UNITY_EDITOR
			return Application.dataPath + "/";
 		#elif UNITY_ANDROID
			return Application.persistentDataPath;
		#elif UNITY_IPHONE
			return Application.persistentDataPath + "/";
		#else
			return Application.dataPath + "/";
		#endif
	}

	private void CheckDirectoryExists( string path )
	{
		//check if directory doesn't exit
 		if(!Directory.Exists(path))
 		{    
     		//if it doesn't, create it
			Debug.Log("Directory Path does not exist. So im creating it for you."); 
     		Directory.CreateDirectory(path);
 		}
		else
		{
			Debug.Log( "Directory exists . We are good to go :)" );
		}
	}

	private void WebAccessStatus( bool b )
	{
		webAccessStatus = b;
	}
}
