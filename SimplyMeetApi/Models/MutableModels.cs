public record M_ThrottleModel
{
	public DateTime FirstRequestDateUTC { get; set; } = default;
	public Int32 RequestCount { get; set; } = default;
};