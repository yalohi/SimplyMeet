using System;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Constants;

namespace SimplyMeetShared.RequestModels
{
	public class ChatSendRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		[Range(0, Int32.MaxValue)]
		public Int32 MatchId { get; set; }

		[Required]
		[MaxLength(MainHubConstants.CHAT_MAX_LENGTH)]
		public String ClientDataJson_Encrypted_Base64 { get; set; }

		[Required]
		[MinLength(CryptoConstants.NONCE_BASE64_LENGTH)]
		[MaxLength(CryptoConstants.NONCE_BASE64_LENGTH)]
		public String ClientDataJson_Nonce_Base64 { get; set; }
		#endregion
	}
}