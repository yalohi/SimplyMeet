namespace SimplyMeetShared.RequestModels;

public record MatchGetChoicesRequestModel : RequestModelBase
{
	[Required]
	public EMatchChoice Choice { get; set; }

	[Required]
	[Range(0, Int32.MaxValue)]
	public Int32 Offset { get; set; }

	[Required]
	[Range(1, ApiRequestConstants.MATCH_GET_CHOICES_MAX_LOAD_COUNT)]
	public Int32 Count { get; set; }
}