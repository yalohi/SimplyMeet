namespace SimplyMeetShared.RequestModels;

public record AccountGetChallengeRequestModel : RequestModelBase
{
	[Required]
	[MinLength(AccountConstants.PUBLIC_KEY_LENGTH)]
	[MaxLength(AccountConstants.PUBLIC_KEY_LENGTH)]
	public Byte[] UserPublicKey { get; set; }
}