using static SimplyMeetApi.Constants.JwtConstants;

namespace SimplyMeetApi.Configuration;

public class EnvironmentService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	public String JwtSecret { get; }
	#endregion
	#region Fields
	private readonly ILogger _Logger;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public EnvironmentService(IWebHostEnvironment InEnv, ILogger<EnvironmentService> InLogger)
	{
		_Logger = InLogger;
		JwtSecret = Environment.GetEnvironmentVariable(JWT_SECRET);

		if (JwtSecret != null)
		{
			if (JwtSecret.Length >= MIN_JWT_SECRET_LENGTH) return;

			_Logger.LogCritical($"{JWT_SECRET} is required to have a minimum length of {MIN_JWT_SECRET_LENGTH}!");
			Environment.Exit(1);
		}

		_Logger.LogWarning($"{JWT_SECRET} environment variable is not configured.");
		var  BecauseText = $"Because we are running in {InEnv.EnvironmentName}";

		if (!InEnv.IsDevelopment())
		{
			_Logger.LogCritical($"{BecauseText}, {JWT_SECRET} is required to be set.");
			Environment.Exit(1);
		}

		JwtSecret = RandomNumberGenerator.GetString(GEN_JWT_CHOICES, MIN_JWT_SECRET_LENGTH);
		_Logger.LogWarning($"{BecauseText}, a temporary {JWT_SECRET} has been generated: {JwtSecret}");
	}
}
