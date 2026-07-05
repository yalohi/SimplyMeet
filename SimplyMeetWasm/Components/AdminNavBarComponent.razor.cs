namespace SimplyMeetWasm.Components;

public partial class AdminNavBarComponent : ComponentBase
{
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	[Inject] public MainHubService MainHubService { get; set; } = default!;
	#endregion
}
