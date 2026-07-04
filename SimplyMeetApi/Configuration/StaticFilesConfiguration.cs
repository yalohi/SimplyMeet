namespace SimplyMeetApi.Configuration;

public class StaticFilesConfiguration : IApiConfiguration
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	public String AvatarsPath { get; private set; }
	public String ImagesPath { get; private set; }
	#endregion
}
