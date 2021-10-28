using System;

namespace SimplyMeetShared.ResponseModels
{
	public class MatchGetNewResponseModel : ResponseModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public ProfileFullModel FullProfile { get; set; }
		#endregion
	}
}