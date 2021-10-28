using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimplyMeetApi.Configuration;
using SimplyMeetApi.Hubs;
using SimplyMeetApi.Middleware;
using SimplyMeetApi.Services;
using SimplyMeetShared.Constants;

namespace SimplyMeetApi
{
	public class Startup
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Fields
		private IConfiguration _Configuration;
		private DatabaseConfiguration _DatabaseConfig;
		private StaticFilesConfiguration _StaticFilesConfig;
		private TokenConfiguration _TokenConfig;
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
			ConfigurationHelper.Configure<AdminConfiguration>(_Configuration, InServices);
			_DatabaseConfig = ConfigurationHelper.Configure<DatabaseConfiguration>(_Configuration, InServices);
			_StaticFilesConfig = ConfigurationHelper.Configure<StaticFilesConfiguration>(_Configuration, InServices);
			ConfigurationHelper.Configure<ThrottleConfiguration>(_Configuration, InServices);
			_TokenConfig = ConfigurationHelper.Configure<TokenConfiguration>(_Configuration, InServices);

			// Services
			InServices.AddSingleton<AccountService>();
			InServices.AddSingleton<AdminService>();
			InServices.AddSingleton<AuthorizationService>();
			InServices.AddSingleton<DatabaseService>();
			InServices.AddSingleton<HomeService>();
			InServices.AddSingleton<MainHubService>();
			InServices.AddSingleton<MatchService>();
			InServices.AddSingleton<ProfileCompactService>();
			InServices.AddSingleton<ProfileService>();
			InServices.AddSingleton<ThrottleService>();
			InServices.AddSingleton<TokenService>();
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
				InOptions.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
			});

			InServices.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(InOptions =>
			{
				InOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = false,
					ValidateIssuerSigningKey = true,
					ValidIssuer = _TokenConfig.Issuer,
					ValidAudience = _TokenConfig.Issuer,
					IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_TokenConfig.SecretKey))
				};

				InOptions.Events = new JwtBearerEvents
				{
					OnMessageReceived = InContext =>
					{
						var Token = InContext.Request.Query["access_token"];
						var Path = InContext.HttpContext.Request.Path;
						if (!String.IsNullOrEmpty(Token) && (Path.StartsWithSegments($"/{MainHubConstants.PATH}"))) InContext.Token = Token;
						return Task.CompletedTask;
					}
				};
			});

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
		public void Configure(IApplicationBuilder InApp, IWebHostEnvironment InEnv)
		{
			if (InEnv.IsDevelopment())
			{
				InApp.UseDeveloperExceptionPage();
				InApp.UseSwagger();
				InApp.UseSwaggerUI(C => C.SwaggerEndpoint("/swagger/v1/swagger.json", "SimplyMeetApi v1"));
			}

			InApp.UseResponseCompression();
			InApp.UseRouting();
			InApp.UseAuthentication();
			InApp.UseAuthorization();
			InApp.UseMiddleware<ExceptionHandlerMiddleware>();

			InApp.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(InEnv.ContentRootPath, _StaticFilesConfig.AvatarsPath)),
				RequestPath = $"/{ApiRequestConstants.AVATARS}"
			});

			InApp.UseEndpoints(InEndpoints =>
			{
				InEndpoints.MapHub<MainHub>($"/{MainHubConstants.PATH}");
				InEndpoints.MapControllers();
			});
		}
	}
}
