using System;
using System.Collections.Generic;
using SimplyMeetShared.Models;

namespace SimplyMeetShared.ResponseModels
{
	public class AdminGetReportedProfilesResponseModel : ResponseModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public IEnumerable<ReportedProfileModel> ReportedProfiles { get; set; }
		public Int32 TotalReportedAccounts { get; set; }
		#endregion
	}
}