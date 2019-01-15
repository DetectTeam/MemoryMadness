using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Linq;
using System;


public static class ListExtensionMethods
{

	private static System.Random rng = new System.Random();  

	public static void ShuffleList<T>(this List<T> list)  
	{	  
	
		int n = list.Count;  
		while (n > 1) 
		{  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	public static void Resize<T>( this List<T> list, int newCount ) 
	{
     	if (newCount <= 0) 
		{
         	list.Clear();
     	} 
		else 
		{
        	while ( list.Count > newCount ) list.RemoveAt( list.Count-1 );
         	while ( list.Count < newCount ) list.Add(default( T ) );
     	}
 	}

	
	public static List<T> Clone<T>(this List<T> listToClone) where T: ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }

}

