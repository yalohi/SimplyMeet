using System;

namespace SimplyMeetShared.Models
{
	public class AccountRoleModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 Id { get; set; }
		public Int32 RoleId { get; set; }
		public String AccountPublicId { get; set; }
		#endregion
	}
}