namespace SimplyMeetShared.RequestModels;

public record AccountCreateRequestModel : RequestModelBase
{
	[Required]
	[StringLength(AccountConstants.PUBLIC_KEY_LENGTH, MinimumLength = AccountConstants.PUBLIC_KEY_LENGTH)]
	public Byte[] UserPublicKey { get; set; }
}
