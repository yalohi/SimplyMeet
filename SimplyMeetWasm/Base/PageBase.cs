namespace SimplyMeetWasm.Base;

public class PageBase : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject]
	public IJSRuntime JS { get; set; }
	[Inject]
	public AppState AppState { get; set; }
	[Inject]
	public MainHubService MainHubService { get; set; }
	[Inject]
	public NavigationService NavigationService { get; set; }
	[Inject]
	public NotificationService NotificationService { get; set; }
	[Inject]
	public SettingsService SettingsService { get; set; }
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnInitializedAsync()
	{
		if (await CheckApiServerAsync())
		{
			var ShouldSetup = await OnCheckSetupAsync();
			if (ShouldSetup) await OnSetupAsync();
		}

		await base.OnInitializedAsync();
	}

	protected virtual async Task<Boolean> OnCheckSetupAsync()
	{
		return await Task.FromResult(true);
	}
	protected virtual async Task OnSetupAsync()
	{
		AppState.ShowNavBar = true;
		AppState.ShowFooter = false;

		await MainHubService.SetupLazyAsync();
		NotificationService.ClearMainNotification();

		await JS.InvokeVoidAsync(JSHelperConstants.SCROLL_ELEMENT_TO, IdConstants.SCROLL_CONTAINER_ID, 0);
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task<Boolean> CheckApiServerAsync()
	{
		if (await SettingsService.GetApiServerAsync() == null)
		{
			NavigationService.NavigateTo(NavigationConstants.NAV_SETUP);
			return false;
		}

		return true;
	}
}
