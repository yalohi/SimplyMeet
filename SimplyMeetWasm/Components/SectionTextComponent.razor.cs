namespace SimplyMeetWasm.Components;

public partial class SectionTextComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Parameter] public String BackgroundClass { get; set; }
	[Parameter] public String Title { get; set; }
	[Parameter] public String Text { get; set; }
	#endregion
}
