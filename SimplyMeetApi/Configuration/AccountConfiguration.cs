namespace SimplyMeetApi.Configuration;

public class AccountConfiguration : IApiConfiguration
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	public Int32 AccountExpirationCheckSeconds { get; private set; } = 3600;
	public Int32 AccountExpirationDayCount { get; private set; } = 90;
	#endregion
}