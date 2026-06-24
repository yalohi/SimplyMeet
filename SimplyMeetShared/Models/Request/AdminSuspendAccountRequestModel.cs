namespace SimplyMeetShared.RequestModels;

public record AdminSuspendAccountRequestModel : RequestModelBase
{
	[Required]
	[StringLength(AccountConstants.PUBLIC_ID_LENGTH, MinimumLength = AccountConstants.PUBLIC_ID_LENGTH)]
	public String AccountPublicId { get; set; }
}
