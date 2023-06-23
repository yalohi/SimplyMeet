namespace SimplyMeetShared.RequestModels;

public record MatchUnmatchRequestModel : RequestModelBase
{
	[Required]
	[Range(0, Int32.MaxValue)]
	public Int32 MatchId { get; set; }
}