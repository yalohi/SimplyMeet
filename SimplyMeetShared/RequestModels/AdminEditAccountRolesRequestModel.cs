using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Models;

namespace SimplyMeetShared.RequestModels
{
	public class AdminEditAccountRolesRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		public IEnumerable<Int32> RemovedIds { get; set; }

		[Required]
		public IEnumerable<AccountRoleModel> NewAccountRoles { get; set; }
		#endregion
	}
}