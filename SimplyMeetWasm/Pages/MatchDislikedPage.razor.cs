namespace SimplyMeetWasm.Pages;

public partial class MatchDislikedPage : PageBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Parameter] public Int32 PageIndex { get; set; }
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnSetupAsync()
	{
		if (MainHubService.FirstMatchUser != null)
		{
			NavigationService.NavigateTo(NavigationConstants.NAV_MATCH);
			return;
		}

		await base.OnSetupAsync();
	}
}
