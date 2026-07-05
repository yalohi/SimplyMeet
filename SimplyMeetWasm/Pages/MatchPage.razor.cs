namespace SimplyMeetWasm.Pages;

public partial class MatchPage : PageBase
{
	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnSetupAsync()
	{
		await base.OnSetupAsync();

		if (MainHubService.FirstMatchUser == null) NavigationService.NavigateTo(NavigationConstants.NAV_MATCH_NEW);
		else NavigationService.NavigateTo(NavigationConstants.NAV_MATCH_CHAT);
	}
}
