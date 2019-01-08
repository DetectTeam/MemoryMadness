using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryMadness
{
	[System.Serializable]
	public class PlayerSelection 
	{
		public float RelativeTime { get; set; } //Relative time from absolute time of study array presentation to absolute time of response
		public float ReactionTime { get; set; } //Time between last selection and current selection
		public int Selection { get; set; }
		public int Repeat { get; set; }
		public int Interrupt { get; set; }
		public int Correct { get; set; }
		public int Lure { get; set; }
		public int OtherMiss{ get; set; }
		//public string Condition { get; set; }
		//public string Nameability { get; set; }
		public int CorrectPosition { get; set; }
		public int SelectedTestCellShape { get; set; }
		public int SelectedTestCellColour { get; set; }
		public int SelectedTestCellPosition{ get; set; }	
	}
}
