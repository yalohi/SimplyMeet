namespace SimplyMeetShared.RequestModels;

public record AccountGetChallengeRequestModel : RequestModelBase
{
	[Required]
	[ArrayLength(AccountConstants.PUBLIC_KEY_LENGTH, MinimumLength = AccountConstants.PUBLIC_KEY_LENGTH)]
	public Byte[] UserPublicKey { get; set; }
}
