namespace SimplyMeetWasm.Components;

public partial class MatchNavBarComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	#endregion
}
