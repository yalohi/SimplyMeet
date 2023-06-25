namespace SimplyMeetWasm.Base;

public class Program
{
	//===========================================================================================
	// Public Static Methods
	//===========================================================================================
	public static async Task Main(String[] InArgs)
	{
		var Builder = WebAssemblyHostBuilder.CreateDefault(InArgs);
		Builder.RootComponents.Add<App>("#app");

		Builder.Services.AddLocalization();

		Builder.Services.AddScoped(InProvider => new HttpClient { BaseAddress = new Uri(ApiRequestConstants.BASE_ADDRESS) });
		Builder.Services.AddScoped<HttpService>();

		Builder.Services.AddSingleton<AppState>();
		Builder.Services.AddSingleton<AccountService>();
		Builder.Services.AddSingleton<LocalizationService>();
		Builder.Services.AddSingleton<LocalStorageService>();
		Builder.Services.AddSingleton<MainHubService>();
		Builder.Services.AddSingleton<NavigationService>();
		Builder.Services.AddSingleton<NotificationService>();
		Builder.Services.AddSingleton<ProfileService>();

		await Builder.Build().RunAsync();
	}
}