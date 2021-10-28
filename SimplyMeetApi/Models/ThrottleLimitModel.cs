using System;

namespace SimplyMeetApi.Models
{
	public class ThrottleLimitModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 Limit { get; set; }
		public Int32 ResetMinutes { get; set; }
		#endregion
	}
}