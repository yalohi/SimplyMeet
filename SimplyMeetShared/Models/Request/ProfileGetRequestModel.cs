namespace SimplyMeetShared.RequestModels;

public record ProfileGetRequestModel : RequestModelBase
{
	[Required]
	[MinLength(AccountConstants.PUBLIC_ID_LENGTH)]
	[MaxLength(AccountConstants.PUBLIC_ID_LENGTH)]
	public String AccountPublicId { get; set; }
}