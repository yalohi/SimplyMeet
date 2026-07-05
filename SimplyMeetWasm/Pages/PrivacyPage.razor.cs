namespace SimplyMeetWasm.Pages;

public partial class PrivacyPage : PageBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnSetupAsync()
	{
		await base.OnSetupAsync();
		AppState.ShowFooter = true;
	}
}
