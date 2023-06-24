namespace SimplyMeetApi.Configuration;

public class DatabaseConfiguration : IApiConfiguration
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	public String ConnectionString { get; private set; }
	#endregion
}