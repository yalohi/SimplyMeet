namespace SimplyMeetApi.Extensions;

public static class IHostExtensions
{
	//===========================================================================================
	// Public Static Methods
	//===========================================================================================
	public static IHost RunMigrations(this IHost InHost)
	{
		using (var Scope = InHost.Services.CreateScope())
		{
			var MigrationRunner = Scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
			MigrationRunner.ListMigrations();
			MigrationRunner.MigrateUp();
		}

		return InHost;
	}
}