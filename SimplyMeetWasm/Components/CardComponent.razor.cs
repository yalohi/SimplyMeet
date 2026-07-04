namespace SimplyMeetWasm.Components;

public partial class CardComponent : ComponentBase
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
	public String CardClasses { get; set; }
	[Parameter]
	public String ContentClasses { get; set; }
	#endregion
}
