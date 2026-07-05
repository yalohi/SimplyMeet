namespace SimplyMeetWasm.Components;

public partial class NotificationComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public NotificationService NotificationService { get; set; } = default!;
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		NotificationService.PropertyChanged += (InSender, InArgs) => StateHasChanged();
	}
}
