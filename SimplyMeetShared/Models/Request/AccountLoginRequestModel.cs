namespace SimplyMeetShared.RequestModels;

public record AccountLoginRequestModel : RequestModelBase
{
	[Required]
	[MinLength(AccountConstants.PUBLIC_KEY_LENGTH)]
	[MaxLength(AccountConstants.PUBLIC_KEY_LENGTH)]
	public Byte[] UserPublicKey { get; set; }

	[Required]
	[MinLength(AccountConstants.SOLVED_CHALLENGE_LENGTH)]
	[MaxLength(AccountConstants.SOLVED_CHALLENGE_LENGTH)]
	public Byte[] SolvedChallenge { get; set; }
}