using System;

namespace SimplyMeetApi.Models
{
	public class ThrottleModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public DateTime FirstRequestDateUTC;
		public Int32 RequestCount;
		#endregion
	}
}