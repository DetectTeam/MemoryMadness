using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryMadness
{
	
	[System.Serializable]
	public class ShapeInfo
	{
		private string shapeCode = "NA";
		public string ShapeCode { get{ return shapeCode; } set{ shapeCode = value; } } 

		private string colourCode = "NA";
		public string ColourCode { get{ return colourCode; } set{ colourCode = value; } } 
	}
	
	[System.Serializable]
	public class StudyItem : ShapeInfo
	{
		private string studyCellNumber = "NA";
		public string StudyCellNumber { get{ return studyCellNumber; } set{ studyCellNumber = value; } }
	}

	[System.Serializable]
	public class TestSlot : ShapeInfo
	{
		public string CellNumber { get; set; }
	}

	[System.Serializable]
	public class Session  
	{
		public string SessionID { get; set; }
		public string SessionName{ get; set; }
		public string SessionTimeStamp { get; set; }
		public string UserID { get; set; } 
		public string Date { get; set; }
		public string AbsoluteTimeOfResponse { get; set; }
		public int Stage { get; set; } // Stage 1 - 4
		//public int Level { get; set; } // level 1 - 8
		public int StudyCellSize { get; set; }
		public List<StudyItem> StudyItems; //Items in MemoryPhase
		public List<TestSlot> TestSlots; //Record of items in the test phase
		public List<PlayerSelection> PlayerSelections;
		public int SymbolArraySize{ get; set; } //Num of symbols to memorize 2 - 5
		public int TrialNumber { get; set; } // a total of 32 trials
		public string Condition { get; set; } //Binding or Shape
		public string Nameability { get; set; } //Abstract or Nameable
		public string ApplicationQuit { get; set; } //Set when user quits app
	
		public Session()
		{
			StudyItems = new List<StudyItem>();
			TestSlots = new List<TestSlot>();
			PlayerSelections = new List<PlayerSelection>();	
		}
	}
}
