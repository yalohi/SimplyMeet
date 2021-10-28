using System;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Constants;

namespace SimplyMeetShared.RequestModels
{
	public class ProfileGetRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		[MinLength(AccountConstants.PUBLIC_ID_LENGTH)]
		[MaxLength(AccountConstants.PUBLIC_ID_LENGTH)]
		public String AccountPublicId { get; set; }
		#endregion
	}
}