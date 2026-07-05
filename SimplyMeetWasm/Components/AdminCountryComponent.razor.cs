namespace SimplyMeetWasm.Components;

public partial class AdminCountryComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;

	[Parameter] public RenderFragment ChildContent { get; set; }
	[Parameter] public RenderFragment RegionsContent { get; set; }
	[Parameter] public EventCallback<(String, String, Int32)> AddClick { get; set; }
	[Parameter] public EventCallback<String> RemoveClick { get; set; }

	private Boolean IsAddDisabled => _NewRegionId == -1 || String.IsNullOrEmpty(_NewIcon) || String.IsNullOrEmpty(_NewName);
	#endregion
	#region Fields
	private String _SelectedName = String.Empty;
	private String _NewName = String.Empty;
	private String _NewIcon = String.Empty;
	private Int32 _NewRegionId = -1;
	#endregion
}
