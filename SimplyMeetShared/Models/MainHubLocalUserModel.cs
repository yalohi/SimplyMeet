using System;
using System.Collections.Generic;

namespace SimplyMeetShared.Models
{
	public class MainHubLocalUserModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public ProfileCompactModel CompactProfile { get; set; }
		public IEnumerable<String> Roles { get; set; }
		#endregion
	}
}