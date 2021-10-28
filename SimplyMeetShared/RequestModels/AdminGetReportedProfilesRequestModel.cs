using System;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Constants;

namespace SimplyMeetShared.RequestModels
{
	public class AdminGetReportedProfilesRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		[Range(0, Int32.MaxValue)]
		public Int32 Offset { get; set; }

		[Required]
		[Range(1, ApiRequestConstants.ADMIN_GET_REPORTED_PROFILES_MAX_LOAD_COUNT)]
		public Int32 Count { get; set; }
		#endregion
	}
}