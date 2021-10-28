using System;
using System.Collections.Generic;
using System.Net;
using SimplyMeetShared.Models;

namespace SimplyMeetApi.Models
{
	public class AuthModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public IPAddress RemoteIpAddress { get; set; }
		public Int32 AccountId { get; set; }
		public IEnumerable<RoleModel> Roles { get; set; }
		#endregion
	}
}