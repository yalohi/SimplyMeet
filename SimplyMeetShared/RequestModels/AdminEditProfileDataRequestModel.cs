using System;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Models;

namespace SimplyMeetShared.RequestModels
{
	public class AdminEditProfileDataRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		public ProfileDataModel Data { get; set; }
		#endregion
	}
}