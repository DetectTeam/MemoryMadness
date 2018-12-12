using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;


public class PersistenceManager : MonoBehaviour 
{
	public static PersistenceManager Instance;

	private string fileName;
	public string FileName { get{ return fileName; } set{ fileName = value; Debug.Log( fileName ); } }

	private string path;

	void Awake()
	{
		Environment.SetEnvironmentVariable( "MONO_REFLECTION_SERIALIZER", "yes" );
		Instance = this;
	}

	void Start()
	{
		
		path = GetPath() + "upload/";
		
		System.Object obj = Load( fileName );
		
		string jsonString = JsonConvert.SerializeObject( obj );

		Debug.Log( jsonString );

		// foreach( Transition t in obj.transistions )
		// {
		// 	Debug.Log( t.TransistionName );
		// 	Debug.Log( t.TransitionTime );
		// }
	}

	public void Test()
	{
		System.Object obj = Load( "Session_Name.dat" );
		
		string jsonString = JsonConvert.SerializeObject( obj );

		Debug.Log( jsonString );
	}

	public void Save( System.Object objectToSave ) 
	{
    	Debug.Log("Saving " +  GetPath() + "upload/" + fileName );

		string directoryPath = GetPath() + "upload/";

		 //check if directory doesn't exit
 		if(!Directory.Exists(directoryPath))
 		{    
     		//if it doesn't, create it
			Debug.Log("Directory Path does not exist. So im creating it for you."); 
     		Directory.CreateDirectory(directoryPath);
 		}
		else
		{
			Debug.Log( "Directory exists . We are good to go :)" );
		}

		BinaryFormatter formatter = new BinaryFormatter();
    	
		FileStream file = File.Open(  path + fileName, FileMode.OpenOrCreate );
    	formatter.Serialize( file, objectToSave );
    	file.Close();
	}

	public System.Object Load( string nameOfFile ) 
	{
    	var serializedObject = new System.Object();

		
    	if( File.Exists( path + nameOfFile ) ) 
		{
        	BinaryFormatter formatter = new BinaryFormatter();
        	FileStream file = File.Open( path + nameOfFile, FileMode.Open );
        	serializedObject = formatter.Deserialize( file );
        	file.Close();
    	}
		else
		{
			Debug.Log( "File not Found..." );
		}
    	return serializedObject;
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

	
	

	
	
}
