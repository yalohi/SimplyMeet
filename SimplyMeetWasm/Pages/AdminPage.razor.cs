namespace SimplyMeetWasm.Pages;

public partial class AdminPage : PageBase
{
	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task<Boolean> OnCheckSetupAsync()
	{
		NavigationService.NavigateTo(NavigationConstants.NAV_ADMIN_REPORTED_PROFILES);
		return await Task.FromResult(false);
	}
}
