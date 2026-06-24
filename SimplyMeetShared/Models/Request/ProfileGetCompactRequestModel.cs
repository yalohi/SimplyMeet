namespace SimplyMeetShared.RequestModels;

public record ProfileGetCompactRequestModel : RequestModelBase
{
	[Required]
	[StringLength(AccountConstants.PUBLIC_ID_LENGTH, MinimumLength = AccountConstants.PUBLIC_ID_LENGTH)]
	public String PublicId { get; set; }
}
