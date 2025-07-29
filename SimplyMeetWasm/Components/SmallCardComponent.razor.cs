namespace SimplyMeetWasm.Components;

public partial class SmallCardComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Parameter]
	public required String Title { get; init; }
	[Parameter]
	public String Content { get; set; }
	[Parameter]
	public String IconClasses { get; set; }
	[Parameter]
	public String ContentClasses { get; set; }
	#endregion
}
