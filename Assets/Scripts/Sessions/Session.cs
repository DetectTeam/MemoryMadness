﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryMadness
{
	[System.Serializable]
	public class StudyItem 
	{
		public int ShapeCode { get; set; }
		public int ColourCode{ get; set; }
	}

	[System.Serializable]
	public class TestSlot : StudyItem
	{
		public int Type { get; set; }
		public string StudyOrder { get; set; }
	}

	[System.Serializable]
	public class Session  
	{
		public int SessionID { get; set; }
		public string SessionName{ get; set; }
		public string SessionTimeStamp { get; set; }
		public int Level { get; set; } // level 1 - 8
		public string UserID { get; set; } 
		public int Stage { get; set; } // Stage 1 - 4
		public int SymbolArraySize{ get; set; } //Num of symbols to memorize 2 - 5
		public int TrialNumber { get; set; } // a total of 32 trials
		public string Condition { get; set; } //Binding or Shape
		public string ShapeType { get; set; } //Abstract or Nameable
		public int DistractorCount { get; set; } //18 - 15 
		public int SumAccuracy { get; set; }
		public int NormalErrors { get; set; } 
		public int LureErrors { get; set; }
		public int TotalLives { get; set; } //2 - 5 based on stage
		public int LivesLost { get; set; } 
		public List<StudyItem> StudyItems; //Items in MemoryPhase
		public List<TestSlot> TestSlots; //Record of items in the test phase
		public int[] AccuracySlots; // List of player decisions for each slot
		public int[] OrderSlot;
		public float[] TimeSlot;

		public Session()
		{
			StudyItems = new List<StudyItem>();
			TestSlots = new List<TestSlot>();
			AccuracySlots = new int[ 20 ];
			OrderSlot = new int[ 20 ];
			TimeSlot = new float[ 20 ];
		}
	}
}