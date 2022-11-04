namespace SimplyMeetApi.Configuration;

public class TokenConfiguration
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	public String SecretKey { get; private set; }
	public String Issuer { get; private set; }
	#endregion
}