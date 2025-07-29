namespace SimplyMeetWasm.Components;

public partial class ContainerComponent<T> : ComponentBase where T : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Parameter]
	public RenderFragment ChildContent { get; set; }

	public List<T> ChildComponentList { get; private set; }
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		ChildComponentList = new List<T>();
	}
}
