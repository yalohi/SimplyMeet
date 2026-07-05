namespace SimplyMeetWasm.Pages;

public partial class AdminReportedProfilesPage : PageBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	[Inject] public HttpService HttpService { get; set; } = default!;

	[Parameter] public Int32 PageIndex { get; set; }
	#endregion
	#region Fields
	private AdminGetReportedProfilesResponseModel _GetResponse;
	private ProfileCompactComponent _ActiveProfile;
	private Boolean _IsRequesting;
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnInitializedAsync()
	{
		_IsRequesting = true;
		await base.OnInitializedAsync();
	}
	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		await ReloadReportedProfilesAsync();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task ReloadReportedProfilesAsync()
	{
		NotificationService.ClearMainNotification();

		var RequestModel = new AdminGetReportedProfilesRequestModel
		{
			Offset = ApiRequestConstants.ADMIN_GET_REPORTED_PROFILES_LOAD_COUNT * PageIndex,
			Count = ApiRequestConstants.ADMIN_GET_REPORTED_PROFILES_LOAD_COUNT,
		};

		_IsRequesting = true;
		_GetResponse = await HttpService.PostJsonRequestAsync<AdminGetReportedProfilesRequestModel, AdminGetReportedProfilesResponseModel>(ApiRequestConstants.ADMIN_GET_REPORTED_PROFILES, RequestModel);
		_IsRequesting = false;
		if (!HttpService.ValidateResponse(_GetResponse)) return;
	}

	private void OnProfileClick(ProfileCompactComponent InCompactProfile)
	{
		if (_ActiveProfile != InCompactProfile) _ActiveProfile?.SetIsActive(false);
		_ActiveProfile = InCompactProfile;
	}
	private async Task OnSuspendClick()
	{
		await ReloadReportedProfilesAsync();
	}
}
