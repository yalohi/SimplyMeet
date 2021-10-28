using System;

namespace SimplyMeetShared.Models
{
	public class MainHubMatchUserModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public ProfileCompactModel CompactProfile { get; set; }
		public Byte[] PublicKey { get; set; }
		public Int32 MatchId { get; set; }
		#endregion
	}
}