namespace SimplyMeetApi.Base;

public class JwtBearerOptionsConfigurator : IConfigureNamedOptions<JwtBearerOptions>
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly EnvironmentService _EnvironmentService;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public JwtBearerOptionsConfigurator(EnvironmentService InEnvironmentService)
	{
		_EnvironmentService = InEnvironmentService;
	}

	public void Configure(String InName, JwtBearerOptions InOptions)
	{
		Configure(InOptions);
	}
	public void Configure(JwtBearerOptions InOptions)
	{
		InOptions.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = false,
			ValidateIssuerSigningKey = true,
			ValidIssuer = nameof(SimplyMeetApi),
			ValidAudience = nameof(SimplyMeetApi),
			IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_EnvironmentService.JwtSecret))
		};

		InOptions.Events = new JwtBearerEvents
		{
			OnMessageReceived = InContext =>
			{
				var Token = InContext.Request.Query["access_token"];
				var Path = InContext.HttpContext.Request.Path;
				if (!String.IsNullOrEmpty(Token) && Path.StartsWithSegments($"/{MainHubConstants.PATH}")) InContext.Token = Token;
				return Task.CompletedTask;
			}
		};
	}
}
