namespace SimplyMeetWasm.Components;

public partial class CopyComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IJSRuntime JS { get; set; } = default!;
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	[Inject] public NotificationService NotificationService { get; set; } = default!;
	[Parameter] public String Text { get; set; }
	#endregion

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task OnCopyClick()
	{
		if (String.IsNullOrEmpty(Text)) return;

		await JS.InvokeVoidAsync(JSHelperConstants.WRITE_CLIPBOARD, Text);
		NotificationService.SetMainNotification(Loc["Copied"], ENotificationType.Info);
	}
}
