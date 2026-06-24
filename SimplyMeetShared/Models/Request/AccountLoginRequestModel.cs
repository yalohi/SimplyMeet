namespace SimplyMeetShared.RequestModels;

public record AccountLoginRequestModel : RequestModelBase
{
	[Required]
	[StringLength(AccountConstants.PUBLIC_KEY_LENGTH, MinimumLength = AccountConstants.PUBLIC_KEY_LENGTH)]
	public Byte[] UserPublicKey { get; set; }

	[Required]
	[StringLength(AccountConstants.SOLVED_CHALLENGE_LENGTH, MinimumLength = AccountConstants.SOLVED_CHALLENGE_LENGTH)]
	public Byte[] SolvedChallenge { get; set; }
}
