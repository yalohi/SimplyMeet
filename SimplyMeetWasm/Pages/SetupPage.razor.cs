namespace SimplyMeetWasm.Pages;

public partial class SetupPage
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject]
	public SettingsService SettingsService { get; set; }

	public Boolean IsContinueDisabled => _IsSubmittingContinue || !_EditContext.Validate() || !_EditContext.IsModified();
	#endregion
	#region Fields
	private EditContext _EditContext;
	private M_SettingsModel _SettingsModel;
	private Boolean _IsSubmittingContinue;
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		await SetupFormAsync();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task SetupFormAsync()
	{
		_SettingsModel = new M_SettingsModel
		{
			ApiServer = (await SettingsService.GetApiServerAsync())?.ToString() ?? String.Empty,
		};

		_EditContext = new EditContext(_SettingsModel);
		_EditContext.SetFieldCssClassProvider(new BootstrapFieldCssClassProvider());
	}
	private async Task SaveChangesAsync()
	{
		await SettingsService.SetApiServerAsync(_SettingsModel.ApiServer);
		_EditContext.MarkAsUnmodified();
	}

	private async void OnContinueClick()
	{
		_IsSubmittingContinue = true;
		await SaveChangesAsync();
		_IsSubmittingContinue = false;

		NavigationService.NavigateTo(NavigationConstants.NAV_HOME);
	}
}
