namespace SimplyMeetWasm.Components;

public partial class AdminIconComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;

	[Parameter] public RenderFragment ChildContent { get; set; }
	[Parameter] public String Name { get; set; }
	[Parameter] public String NewPlaceholder { get; set; }
	[Parameter] public String SelectPlaceholder { get; set; }
	[Parameter] public Int32 NewMaxLength { get; set; }

	[Parameter] public EventCallback<(String, String)> AddClick { get; set; }
	[Parameter] public EventCallback<String> RemoveClick { get; set; }

	private Boolean IsAddDisabled => String.IsNullOrEmpty(_NewIcon) || String.IsNullOrEmpty(_NewName);
	#endregion
	#region Fields
	private String _SelectedName = String.Empty;
	private String _NewName = String.Empty;
	private String _NewIcon = String.Empty;
	#endregion
}
