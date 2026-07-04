namespace SimplyMeetApi;

public class Startup
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly IConfiguration _Configuration;

	private DatabaseConfiguration _DatabaseConfig;
	private StaticFilesConfiguration _StaticFilesConfig;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public Startup(IConfiguration InConfiguration)
	{
		_Configuration = InConfiguration;
	}

	public void ConfigureServices(IServiceCollection InServices)
	{
		// Config
		ConfigurationHelper.Configure<AccountConfiguration>(_Configuration, InServices);
		ConfigurationHelper.Configure<AdminConfiguration>(_Configuration, InServices);
		ConfigurationHelper.Configure<HomeConfiguration>(_Configuration, InServices);
		_DatabaseConfig = ConfigurationHelper.Configure<DatabaseConfiguration>(_Configuration, InServices);
		_StaticFilesConfig = ConfigurationHelper.Configure<StaticFilesConfiguration>(_Configuration, InServices);
		ConfigurationHelper.Configure<ThrottleConfiguration>(_Configuration, InServices);

		// Services

		InServices.AddHostedService<AccountExpirationService>();
		InServices.AddSingleton<AccountService>();
		InServices.AddSingleton<AdminService>();
		InServices.AddSingleton<AuthorizationService>();
		InServices.AddSingleton<DatabaseService>();
		InServices.AddSingleton<EnvironmentService>();
		InServices.AddSingleton<HomeService>();
		InServices.AddSingleton<MainHubService>();
		InServices.AddSingleton<MatchService>();
		InServices.AddSingleton<ProfileCompactService>();
		InServices.AddSingleton<ProfileService>();
		InServices.AddSingleton<ThrottleService>();
		InServices.AddSingleton<TokenService>();
		InServices.AddCors(Options => Options.AddDefaultPolicy(Policy => Policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
		InServices.AddControllers();

		InServices.AddSingleton<ExceptionFilter>();
		InServices.AddSingleton<ThrottleFilter>();

		InServices.AddSignalR(InOptions =>
		{
			InOptions.AddFilter<ExceptionFilter>();
			InOptions.AddFilter<ThrottleFilter>();
		});

		InServices.AddResponseCompression(InOptions =>
		{
			InOptions.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat([ "application/octet-stream" ]);
		});

		InServices.ConfigureOptions<JwtBearerOptionsConfigurator>().AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

		InServices.AddSwaggerGen(InOptions =>
		{
			InOptions.SwaggerDoc("v1", new OpenApiInfo { Title = nameof(SimplyMeetApi), Version = "v1" });
		});

		InServices.AddFluentMigratorCore()
			.ConfigureRunner(InRunnerBuilder => InRunnerBuilder
				.AddSQLite()
				.WithGlobalConnectionString(_DatabaseConfig.ConnectionString)
				.ScanIn(typeof(Migrations.D1).Assembly).For.Migrations())
			.AddLogging(InLogBuilder => InLogBuilder.AddFluentMigratorConsole());
	}
	public void Configure(IApplicationBuilder InApp, IWebHostEnvironment InEnv, ILogger<Startup> InLogger)
	{
		if (InEnv.IsDevelopment())
		{
			InApp.UseDeveloperExceptionPage();
			InApp.UseSwagger();
			InApp.UseSwaggerUI(C => C.SwaggerEndpoint("/swagger/v1/swagger.json", "SimplyMeetApi v1"));
		}

		InApp.UseRouting();
		InApp.UseCors();
		InApp.UseAuthentication();
		InApp.UseAuthorization();
		InApp.UseResponseCompression();
		InApp.UseMiddleware<ExceptionHandlerMiddleware>();

		if (String.IsNullOrEmpty(_StaticFilesConfig.AvatarsPath))
		{
			InLogger.LogCritical($"{nameof(_StaticFilesConfig.AvatarsPath)} not set! Check your configuration.");
			Environment.Exit(1);
		}

		Directory.CreateDirectory(_StaticFilesConfig.AvatarsPath);
		Directory.CreateDirectory(_StaticFilesConfig.ImagesPath);

		InApp.UseStaticFiles(new StaticFileOptions
		{
			FileProvider = new PhysicalFileProvider(Path.Combine(InEnv.ContentRootPath, _StaticFilesConfig.AvatarsPath)),
			RequestPath = $"/{ApiRequestConstants.AVATARS}"
		});

		InApp.UseStaticFiles(new StaticFileOptions
		{
			FileProvider = new PhysicalFileProvider(Path.Combine(InEnv.ContentRootPath, _StaticFilesConfig.ImagesPath)),
			RequestPath = $"/{ApiRequestConstants.IMAGES}"
		});

		InApp.UseEndpoints(InEndpoints =>
		{
			InEndpoints.MapHub<MainHub>($"/{MainHubConstants.PATH}").RequireCors();
			InEndpoints.MapControllers();
		});
	}
}
