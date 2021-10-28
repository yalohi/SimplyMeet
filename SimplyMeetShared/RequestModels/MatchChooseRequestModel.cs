using System;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Constants;
using SimplyMeetShared.Enums;

namespace SimplyMeetShared.RequestModels
{
	public class MatchChooseRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		[MinLength(AccountConstants.PUBLIC_ID_LENGTH)]
		[MaxLength(AccountConstants.PUBLIC_ID_LENGTH)]
		public String PublicId { get; set; }

		[Required]
		public EMatchChoice Choice { get; set; }
		#endregion
	}
}