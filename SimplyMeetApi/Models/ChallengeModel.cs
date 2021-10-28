using System;

namespace SimplyMeetApi.Models
{
	public class ChallengeModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Byte[] Challenge { get; set; }
		public Byte[] SolvedChallenge { get; set; }
		public DateTime ExpireDateUTC { get; set; }
		#endregion
	}
}