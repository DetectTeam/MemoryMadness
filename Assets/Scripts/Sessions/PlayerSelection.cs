using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryMadness
{
	[System.Serializable]
	public class PlayerSelection 
	{
		public int Level { get; set; } 
		public int TrialNumber { get; set; }
		public string TimeOfSelection { get; set; }
		public string RelativeTime { get; set; } //Relative time from absolute time of study array presentation to absolute time of response
		public string ReactionTime { get; set; } //Time between last selection and current selection
		public string Selection { get; set; }
		public string Repeat { get; set; }
		public string Interrupt { get; set; }
		public string Correct { get; set; }
		public string Lure { get; set; }
		public string OtherMiss{ get; set; }
		//public string Condition { get; set; }
		//public string Nameability { get; set; }
		public string CorrectPosition { get; set; }
		public string SelectedTestCellShape { get; set; }
		public string SelectedTestCellColour { get; set; }
		public string SelectedTestCellPosition{ get; set; }	
		
	}
}
