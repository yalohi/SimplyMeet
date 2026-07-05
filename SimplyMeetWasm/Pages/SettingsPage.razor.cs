namespace SimplyMeetWasm.Pages;

public partial class SettingsPage : PageBase
{
	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnSetupAsync()
	{
		await base.OnSetupAsync();
		NavigationService.NavigateTo(NavigationConstants.NAV_SETTINGS_SERVER);
	}
}
