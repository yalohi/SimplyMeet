using System;
using SimplyMeetShared.Enums;

namespace SimplyMeetShared.Models
{
	public class ProfileCompactModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public String Avatar { get; set; }
		public String DisplayName { get; set; }
		public String PublicId { get; set; }
		public EMatchChoice Choice { get; set; }
		#endregion
	}
}