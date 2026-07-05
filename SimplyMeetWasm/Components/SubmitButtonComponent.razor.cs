namespace SimplyMeetWasm.Components;

public partial class SubmitButtonComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Parameter] public String CssClass { get; set; }
	[Parameter] public String Text { get; set; }
	[Parameter] public Boolean IsSubmitting { get; set; }
	[Parameter] public Boolean IsDisabled { get; set; }
	[Parameter] public EventCallback Click { get; set; }
	#endregion
}
