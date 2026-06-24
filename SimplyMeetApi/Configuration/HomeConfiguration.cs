namespace SimplyMeetApi.Configuration;

public class HomeConfiguration : IApiConfiguration
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	public IEnumerable<CardModel> Cards { get; private set; }
	public String ShortDescription { get; private set; }
	public String Administration { get; private set; }
	public String RootRedirectUrl { get; private set; }
	#endregion
}
