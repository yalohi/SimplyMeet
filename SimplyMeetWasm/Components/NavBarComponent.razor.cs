namespace SimplyMeetWasm.Components;

public partial class NavBarComponent : ComponentBase, IDisposable
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	[Inject] public AppState AppState { get; set; } = default!;
	[Inject] public AccountService AccountService { get; set; } = default!;
	[Inject] public MainHubService MainHubService { get; set; } = default!;
	#endregion
	#region Fields
	private String _LinkClasses;
	private String _LoginLinkClasses;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public void Dispose()
	{
		AccountService.Login -= OnLoginUpdate;
		AccountService.Logout -= OnLoginUpdate;
		MainHubService.LocalUserUpdate -= OnLocalUserUpdate;
		MainHubService.ChatUpdate -= OnChatUpdate;
	}

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		await UpdateBarAsync();

		AccountService.Login += OnLoginUpdate;
		AccountService.Logout += OnLoginUpdate;
		MainHubService.LocalUserUpdate += OnLocalUserUpdate;
		MainHubService.ChatUpdate += OnChatUpdate;
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async void OnLoginUpdate(Object InSender, EventArgs InArgs) => await UpdateBarAsync();
	private void OnLocalUserUpdate(Object InSender, EventArgs InArgs) => StateHasChanged();
	private void OnChatUpdate(Object InSender, EventArgs InArgs) => StateHasChanged();

	private async Task UpdateBarAsync()
	{
		_LinkClasses = "nav-link text-center px-3";
		_LoginLinkClasses = await AppState.HasLoginAsync() ? _LinkClasses : $"{_LinkClasses} text-muted disabled";
		StateHasChanged();
	}
}
