namespace SimplyMeetApi;

public class Program
{
	//===========================================================================================
	// Public Static Methods
	//===========================================================================================
	public static void Main(String[] InArgs)
	{
		CreateHostBuilder(InArgs)
			.Build()
			.RunMigrations()
			.Run();
	}

	public static IHostBuilder CreateHostBuilder(String[] InArgs)
	{
		return Host.CreateDefaultBuilder(InArgs)
			.ConfigureLogging(InLogging =>
			{
				InLogging.AddFile("Logs/Error-{Date}.log", LogLevel.Error);
			})
			.ConfigureWebHostDefaults(InBuilder =>
			{
				InBuilder.UseStartup<Startup>();
			});
	}
}