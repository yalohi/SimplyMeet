using System;
using SimplyMeetShared.Models;

namespace SimplyMeetShared.ResponseModels
{
	public class ProfileGetCompactResponseModel : ResponseModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public ProfileCompactModel CompactProfile { get; set; }
		#endregion
	}
}