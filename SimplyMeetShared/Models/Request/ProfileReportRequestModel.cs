namespace SimplyMeetShared.RequestModels;

public record ProfileReportRequestModel : RequestModelBase
{
	[Required]
	[StringLength(AccountConstants.PUBLIC_ID_LENGTH, MinimumLength = AccountConstants.PUBLIC_ID_LENGTH)]
	public String AccountPublicId { get; set; }

	[Required]
	[Range(0, Int32.MaxValue)]
	public Int32 ReportReasonId { get; set; }
}
