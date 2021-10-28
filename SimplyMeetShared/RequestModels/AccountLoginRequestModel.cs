using System;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Constants;

namespace SimplyMeetShared.RequestModels
{
	public class AccountLoginRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		[MinLength(AccountConstants.PUBLIC_KEY_LENGTH)]
		[MaxLength(AccountConstants.PUBLIC_KEY_LENGTH)]
		public Byte[] UserPublicKey { get; set; }

		[Required]
		[MinLength(AccountConstants.SOLVED_CHALLENGE_LENGTH)]
		[MaxLength(AccountConstants.SOLVED_CHALLENGE_LENGTH)]
		public Byte[] SolvedChallenge { get; set; }
		#endregion
	}
}