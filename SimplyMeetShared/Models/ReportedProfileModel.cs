using System;

namespace SimplyMeetShared.Models
{
	public class ReportedProfileModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public ProfileCompactModel CompactProfile { get; set; }
		public Int32 ReportCount { get; set; }
		#endregion
	}
}