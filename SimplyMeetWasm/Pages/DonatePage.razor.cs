namespace SimplyMeetWasm.Pages;

public partial class DonatePage : PageBase
{
	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnSetupAsync()
	{
		await base.OnSetupAsync();
		AppState.ShowFooter = true;
	}
}
