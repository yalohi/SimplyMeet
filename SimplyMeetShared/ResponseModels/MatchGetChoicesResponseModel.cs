using System;
using System.Collections.Generic;
using SimplyMeetShared.Models;

namespace SimplyMeetShared.ResponseModels
{
	public class MatchGetChoicesResponseModel : ResponseModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public IEnumerable<ProfileCompactModel> CompactProfiles { get; set; }
		public Int32 TotalProfiles { get; set; }
		#endregion
	}
}