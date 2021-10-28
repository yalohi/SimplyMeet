using System;
using SimplyMeetShared.Models;

namespace SimplyMeetWasm.Models
{
	public class MatchUserStateModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public MainHubMatchUserModel User { get; set; }
		public Int32 UnreadMessageCount { get; set; }
		public Int32 RemainingPreviousMessageCount { get; set; }
		public Int32 RemainingFollowingMessageCount { get; set; }
		public Boolean IsLoadingPreviousMessages { get; set; }
		public Boolean IsLoadingFollowingMessages { get; set; }
		public Boolean IsChatSetup { get; set; }
		#endregion
	}
}