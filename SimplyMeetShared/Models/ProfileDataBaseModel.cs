using System;

namespace SimplyMeetShared.Models
{
	public abstract class ProfileDataBaseModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 Id { get; set; }
		public String Name { get; set; }
		#endregion
	}
}