namespace SimplyMeetWasm.Components;

public partial class AccountComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	[Inject] public AppState AppState { get; set; } = default!;
	[Inject] public AccountService AccountService { get; set; } = default!;
	[Inject] public HttpService HttpService { get; set; } = default!;
	[Inject] public NavigationService NavigationService { get; set; } = default!;
	[Inject] public NotificationService NotificationService { get; set; } = default!;

	public Boolean IsLoginDisabled => _IsSubmittingLogin || _IsSubmittingGenerateAccount || String.IsNullOrEmpty(_PrivateKey) || _PrivateKey.Length != AccountConstants.PRIVATE_KEY_TEXT_LENGTH;
	public Boolean IsGenerateAccountDisabled => _IsSubmittingLogin || _IsSubmittingGenerateAccount;

	[Parameter] public String Title { get; set; }
	#endregion
	#region Fields
	private String _PrivateKey;
	private Boolean _HasLogin;
	private Boolean _IsSubmittingLogin;
	private Boolean _IsSubmittingGenerateAccount;
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		_HasLogin = await AppState.HasLoginAsync();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task OnPrivateKeyKeyDown(KeyboardEventArgs InArgs)
	{
		if ((InArgs.Code == KeyCodeConstants.ENTER || InArgs.Code == KeyCodeConstants.NUMPAD_ENTER) && !String.IsNullOrEmpty(_PrivateKey)) await OnLoginClick();
	}
	private async Task OnLoginClick()
	{
		_IsSubmittingLogin = true;
		await AccountService.RequestLoginAsync(_PrivateKey);
		_HasLogin = await AppState.HasLoginAsync();
		_IsSubmittingLogin = false;
	}
	private async Task OnGenerateAccountClick()
	{
		_IsSubmittingGenerateAccount = true;
		await AccountService.RequestGenerateAccountAsync();
		_HasLogin = await AppState.HasLoginAsync();
		_IsSubmittingGenerateAccount = false;
	}
	private async Task OnLogoutClick()
	{
		await AccountService.LogoutAsync();
		_HasLogin = await AppState.HasLoginAsync();
	}

	private void OnProfileClick()
	{
		NavigationService.NavigateTo(NavigationConstants.NAV_PROFILE);
	}
	private void OnMatchClick()
	{
		NavigationService.NavigateTo(NavigationConstants.NAV_MATCH);
	}
}
