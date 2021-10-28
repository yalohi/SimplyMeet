using System.Collections.Generic;
using SimplyMeetApi.Enums;
using SimplyMeetApi.Models;

namespace SimplyMeetApi.Configuration
{
	public class ThrottleConfiguration
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Dictionary<EThrottleGroup, ThrottleLimitModel> ThrottleLimitDict { get; private set; }
		#endregion
	}
}