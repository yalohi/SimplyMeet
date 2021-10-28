using System;
using System.Collections.Generic;
using SimplyMeetShared.Models;

namespace SimplyMeetApi.Models
{
	public class AuthHubModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public String ConnectionId { get; set; }
		public Int32 AccountId { get; set; }
		public IEnumerable<RoleModel> Roles { get; set; }
		#endregion
	}
}