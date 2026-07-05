namespace SimplyMeetWasm.Pages;

public partial class SettingsServerPage
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	public Boolean IsSaveChangesDisabled => _IsSubmittingSaveChanges || !_EditContext.Validate() || !_EditContext.IsModified();
	#endregion
	#region Fields
	private ChooseServerComponent _ChooseServerComponent;

	private EditContext _EditContext;
	private M_SettingsModel _SettingsModel;
	private Boolean _IsSubmittingSaveChanges;
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnSetupAsync()
	{
		await base.OnSetupAsync();
	}
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		_SettingsModel = new M_SettingsModel
		{
			ApiServer = (await SettingsService.GetApiServerAsync())?.ToString() ?? String.Empty,
		};

		_EditContext = new EditContext(_SettingsModel);
		_EditContext.SetFieldCssClassProvider(new BootstrapFieldCssClassProvider());
		// _EditContext.OnFieldChanged += EditContext_OnFieldChanged;
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task SaveChangesAsync()
	{
		await SettingsService.SetApiServerAsync(_SettingsModel.ApiServer);
		_EditContext.MarkAsUnmodified();
	}
	private async Task OnSaveChangesClick()
	{
		_IsSubmittingSaveChanges = true;
		await SaveChangesAsync();
		_IsSubmittingSaveChanges = false;
	}
}
