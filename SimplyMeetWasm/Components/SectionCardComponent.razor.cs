namespace SimplyMeetWasm.Components;

public partial class SectionCardComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Parameter] public RenderFragment ChildContent { get; set; }
	[Parameter] public String BackgroundClass { get; set; }
	#endregion
}
