using System.Collections.Generic;
using SimplyMeetShared.Models;

namespace SimplyMeetShared.ResponseModels
{
	public class AdminGetAccountRolesResponseModel : ResponseModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public IEnumerable<RoleModel> Roles { get; set; }
		public IEnumerable<AccountRoleModel> AccountRoles { get; set; }

		public ProfileCompactModel MainAdminCompactProfile { get; set; }
		public IEnumerable<ProfileCompactModel> CompactProfiles { get; set; }
		#endregion
	}
}