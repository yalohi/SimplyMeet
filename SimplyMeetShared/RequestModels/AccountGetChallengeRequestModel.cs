using System;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Constants;

namespace SimplyMeetShared.RequestModels
{
	public class AccountGetChallengeRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		[MinLength(AccountConstants.PUBLIC_KEY_LENGTH)]
		[MaxLength(AccountConstants.PUBLIC_KEY_LENGTH)]
		public Byte[] UserPublicKey { get; set; }
		#endregion
	}
}