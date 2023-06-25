namespace SimplyMeetShared.RequestModels;

public record MatchChooseRequestModel : RequestModelBase
{
	[Required]
	[MinLength(AccountConstants.PUBLIC_ID_LENGTH)]
	[MaxLength(AccountConstants.PUBLIC_ID_LENGTH)]
	public String PublicId { get; set; }

	[Required]
	public EMatchChoice Choice { get; set; }
}