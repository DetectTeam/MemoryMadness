using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour 
{

	[SerializeField] private string deviceType;

	[SerializeField] private string deviceName;
	[SerializeField] private string deviceModel;


	#region Getters Setters

	public string DeviceType { get{  return deviceType;} set{ deviceType = value; } }
	public string DeviceName { get{  return deviceName; } set{ deviceName = value; } }
	public string DeviceModel { get{  return deviceModel; } set{ deviceModel = value; } }


	#endregion   


	// Use this for initialization
	void Start () 
	{
		deviceType = SystemInfo.deviceType.ToString();
		deviceName = SystemInfo.deviceName.ToString();
		deviceModel = SystemInfo.deviceModel.ToString();

		Debug.Log( deviceType + " " + deviceName + " " + deviceModel );
	}
	
	
}
