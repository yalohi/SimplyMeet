namespace SimplyMeetWasm.Components;

public partial class SectionComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Parameter] public RenderFragment ChildContent { get; set; }
	[Parameter] public String BackgroundClass { get; set; }
	[Parameter] public Uri BackgroundUri { get; set; }
	#endregion
	#region Fields
	private String _Style;
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		if (BackgroundUri != null) _Style = $"background-image: url('{BackgroundUri}');";
	}
}
