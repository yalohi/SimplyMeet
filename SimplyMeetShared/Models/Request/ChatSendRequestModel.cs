namespace SimplyMeetShared.RequestModels;

public record ChatSendRequestModel : RequestModelBase
{
	[Required]
	[Range(0, Int32.MaxValue)]
	public Int32 MatchId { get; set; }

	[Required]
	[StringLength(MainHubConstants.CHAT_MAX_LENGTH)]
	public String ClientDataJson_Encrypted_Base64 { get; set; }

	[Required]
	[StringLength(CryptoConstants.NONCE_BASE64_LENGTH, MinimumLength = CryptoConstants.NONCE_BASE64_LENGTH)]
	public String ClientDataJson_Nonce_Base64 { get; set; }
}
