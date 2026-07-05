namespace SimplyMeetWasm.Pages;

public partial class AdminProfileDataPage : PageBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	[Inject] public HttpService HttpService { get; set; } = default!;

	public M_ProfileDataModel Data => _EditRequest.Data;
	public Boolean IsSaveDisabled => _IsSubmitting || !_DataChanged;
	#endregion
	#region Fields
	private AdminGetProfileDataResponseModel _GetResponse;
	private AdminEditProfileDataRequestModel _EditRequest;
	private Boolean _IsRequesting;
	private Boolean _IsSubmitting;
	private Boolean _DataChanged;
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnInitializedAsync()
	{
		_IsRequesting = true;
		await base.OnInitializedAsync();
	}
	protected override async Task OnSetupAsync()
	{
		await base.OnSetupAsync();
		await ReloadProfileDataAsync();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task ReloadProfileDataAsync()
	{
		NotificationService.ClearMainNotification();

		var RequestModel = new AdminGetProfileDataRequestModel();
		_IsRequesting = true;
		_GetResponse = await HttpService.PostJsonRequestAsync<AdminGetProfileDataRequestModel, AdminGetProfileDataResponseModel>(ApiRequestConstants.ADMIN_GET_PROFILE_DATA, RequestModel);
		_IsRequesting = false;
		if (!HttpService.ValidateResponse(_GetResponse)) return;

		_EditRequest = new AdminEditProfileDataRequestModel { Data = _GetResponse.ProfileData };

	}
	private async Task SaveChangesAsync()
	{
		var Response = await HttpService.PostJsonRequestAsync<AdminEditProfileDataRequestModel, AdminEditProfileDataResponseModel>(ApiRequestConstants.ADMIN_EDIT_PROFILE_DATA, _EditRequest);
		if (!HttpService.ValidateResponse(Response)) return;

		NotificationService.ClearMainNotification();
		_DataChanged = false;
	}

	private async Task OnSaveChangesClick()
	{
		_IsSubmitting = true;
		await SaveChangesAsync();
		_IsSubmitting = false;
	}

	private void OnAddPronounsClick(String InName) => Data.AllPronouns = AddProfileData(Data.AllPronouns, new PronounsModel { Name = InName });
	private void OnRemovePronounsClick(String InName) => Data.AllPronouns = RemoveProfileData(Data.AllPronouns, InName);
	private void OnAddSexClick((String Name, String Icon) InParams) => Data.AllSexes = AddProfileData(Data.AllSexes, new SexModel { Name = InParams.Name, Icon = InParams.Icon });
	private void OnRemoveSexClick(String InName) => Data.AllSexes = RemoveProfileData(Data.AllSexes, InName);
	private void OnAddGenderClick(String InName) => Data.AllGenders = AddProfileData(Data.AllGenders, new GenderModel { Name = InName });
	private void OnRemoveGenderClick(String InName) => Data.AllGenders = RemoveProfileData(Data.AllGenders, InName);
	private void OnAddRegionClick((String Name, String Icon) InParams) => Data.AllRegions = AddProfileData(Data.AllRegions, new RegionModel { Name = InParams.Name, Icon = InParams.Icon });
	private void OnRemoveRegionClick(String InName) => Data.AllRegions = RemoveProfileData(Data.AllRegions, InName);
	private void OnAddCountryClick((String Name, String Icon, Int32 RegionId) InParams) => Data.AllCountries = AddProfileData(Data.AllCountries, new CountryModel { Name = InParams.Name, Icon = InParams.Icon, RegionId = InParams.RegionId });
	private void OnRemoveCountryClick(String InName) => Data.AllCountries = RemoveProfileData(Data.AllCountries, InName);
	private void OnAddSexualityClick(String InName) => Data.AllSexualities = AddProfileData(Data.AllSexualities, new SexualityModel { Name = InName });
	private void OnRemoveSexualityClick(String InName) => Data.AllSexualities = RemoveProfileData(Data.AllSexualities, InName);

	private IEnumerable<T> AddProfileData<T>(IEnumerable<T> InItems, T InModel) where T : ProfileDataModelBase
	{
		NotificationService.ClearMainNotification();
		if (InItems.Any(X => X.Name == InModel.Name))
		{
			NotificationService.SetMainNotification(@Loc["NameExists"], ENotificationType.Warning);
			return InItems;
		}

		_DataChanged = true;
		return InItems.Append(InModel);
	}
	private IEnumerable<T> RemoveProfileData<T>(IEnumerable<T> InItems, String InName) where T : ProfileDataModelBase
	{
		_DataChanged = true;
		return InItems.Where(X => X.Name != InName);
	}
}
