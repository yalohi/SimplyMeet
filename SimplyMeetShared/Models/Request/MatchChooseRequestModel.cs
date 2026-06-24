namespace SimplyMeetShared.RequestModels;

public record MatchChooseRequestModel : RequestModelBase
{
	[Required]
	[StringLength(AccountConstants.PUBLIC_ID_LENGTH, MinimumLength = AccountConstants.PUBLIC_ID_LENGTH)]
	public String PublicId { get; set; }

	[Required]
	public EMatchChoice Choice { get; set; }
}
