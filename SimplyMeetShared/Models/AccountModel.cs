using System;
using SimplyMeetShared.Enums;

namespace SimplyMeetShared.Models
{
	public class AccountModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 Id { get; set; }

		public String PublicId { get; set; }
		public String PublicKey_Base64 { get; set; }
		public DateTime Creation { get; set; }
		public DateTime? LastLogin { get; set; }
		public EAccountStatus Status { get; set; }

		public Int32 ProfileId { get; set; }
		public Int32 FilterId { get; set; }
		#endregion
	}
}