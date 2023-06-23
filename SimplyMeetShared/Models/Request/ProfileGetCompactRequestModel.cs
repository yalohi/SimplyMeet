namespace SimplyMeetShared.RequestModels;

public record ProfileGetCompactRequestModel : RequestModelBase
{
	[Required]
	[MinLength(AccountConstants.PUBLIC_ID_LENGTH)]
	[MaxLength(AccountConstants.PUBLIC_ID_LENGTH)]
	public String PublicId { get; set; }
}