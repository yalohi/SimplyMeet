namespace SimplyMeetWasm.Pages;

public partial class MatchFilterPage : PageBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	[Inject] public HttpService HttpService { get; set; } = default!;

	public ProfileFullModel FullProfile => _GetResponse.FullProfile;
	public AccountModel Account => _GetResponse.FullProfile.Account;
	public ProfileModel Profile => _GetResponse.FullProfile.Profile;
	public Boolean IsSaveDisabled => _IsSubmitting || !_EditContext.Validate() || (!_EditContext.IsModified() && !_AgeChanged);

	public Int32 FromAge
	{
		get { return _EditRequest.FromAge; }
		set
		{
			_EditRequest.FromAge = value;
			OnAgeChanged();
		}
	}
	public Int32 ToAge
	{
		get { return _EditRequest.ToAge; }
		set
		{
			_EditRequest.ToAge = value;
			OnAgeChanged();
		}
	}
	public Boolean AgeFilterEnabled
	{
		get { return _EditRequest.AgeEnabled; }
		set
		{
			_EditRequest.AgeEnabled = value;
			OnAgeChanged();
		}
	}
	#endregion
	#region Fields
	private EditContext _EditContext;
	private MatchGetFilterResponseModel _GetResponse;
	private MatchEditFilterRequestModel _EditRequest;

	private IEnumerable<CountryModel> _Countries;

	private Boolean _AgeChanged;

	private Boolean _IsRequesting;
	private Boolean _IsSubmitting;
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
		await ReloadFiltersAsync();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task ReloadFiltersAsync()
	{
		NotificationService.ClearMainNotification();

		try
		{
			var RequestModel = new MatchGetFilterRequestModel();
			_IsRequesting = true;
			_GetResponse = await HttpService.PostJsonRequestAsync<MatchGetFilterRequestModel, MatchGetFilterResponseModel>(ApiRequestConstants.MATCH_GET_FILTER, RequestModel);
			_IsRequesting = false;

			if (!HttpService.ValidateResponse(_GetResponse)) return;
		}

		catch
		{
			NavigationService.NavigateTo(NavigationConstants.NAV_HOME);
			return;
		}

		FullProfile.Data.AllPronouns = FullProfile.Data.AllPronouns.OrderBy(X => X.Name);
		FullProfile.Data.AllSexes = FullProfile.Data.AllSexes.OrderBy(X => X.Name);
		FullProfile.Data.AllGenders = FullProfile.Data.AllGenders.OrderBy(X => X.Name);
		FullProfile.Data.AllRegions = FullProfile.Data.AllRegions.OrderBy(X => X.Name);
		FullProfile.Data.AllCountries = FullProfile.Data.AllCountries.OrderBy(X => X.Name);
		FullProfile.Data.AllSexualities = FullProfile.Data.AllSexualities.OrderBy(X => X.Name);

		_EditRequest = new MatchEditFilterRequestModel
		{
			PronounsId = FullProfile.Filter.PronounsId,
			SexId = FullProfile.Filter.SexId,
			GenderId = FullProfile.Filter.GenderId,
			RegionId = FullProfile.Filter.RegionId,
			CountryId = FullProfile.Filter.CountryId,
			FromAge = Math.Clamp(FullProfile.Filter.FromAge, ProfileConstants.MIN_AGE, ProfileConstants.MAX_AGE),
			ToAge = Math.Clamp(FullProfile.Filter.ToAge, ProfileConstants.MIN_AGE, ProfileConstants.MAX_AGE),
			AgeEnabled = FullProfile.Filter.AgeEnabled,
		};

		OnRegionChanged(_EditRequest.RegionId);
		OnCountryChanged(_EditRequest.CountryId);

		_EditContext = new EditContext(_EditRequest);
		_EditContext.SetFieldCssClassProvider(new BootstrapFieldCssClassProvider());
	}
	private async Task SaveChangesAsync()
	{
		if (_EditContext.IsModified() || _AgeChanged)
		{
			var RequestModel = GetFilterEditRequestModel();
			var Response = await HttpService.PostJsonRequestAsync<MatchEditFilterRequestModel, MatchEditFilterResponseModel>(ApiRequestConstants.MATCH_EDIT_FILTER, RequestModel);
			if (!HttpService.ValidateResponse(Response)) return;

			_EditContext.MarkAsUnmodified();
			_AgeChanged = false;
		}

		await ReloadFiltersAsync();
	}

	private MatchEditFilterRequestModel GetFilterEditRequestModel()
	{
		var Request = new MatchEditFilterRequestModel()
		{
			PronounsId = _EditRequest.PronounsId,
			SexId = _EditRequest.SexId,
			GenderId = _EditRequest.GenderId,
			RegionId = _EditRequest.RegionId,
			CountryId = _EditRequest.CountryId,
			FromAge = _EditRequest.FromAge,
			ToAge = _EditRequest.ToAge,
			AgeEnabled = _EditRequest.AgeEnabled,
		};

		return Request;
	}
	private void OnRegionChanged(Int32? InValue)
	{
		var RegionId = InValue;
		_EditRequest.RegionId = RegionId;

		if (RegionId == null) _Countries = _GetResponse.FullProfile.Data.AllCountries;
		else _Countries = _GetResponse.FullProfile.Data.AllCountries.Where(X => X.RegionId == RegionId);

		var Country = RegionId != null ? _Countries.FirstOrDefault(X => X.Id == _EditRequest.CountryId) : null;
		OnCountryChanged(Country?.Id);
	}
	private void OnCountryChanged(Int32? InValue)
	{
		var CountryId = InValue;
		_EditRequest.CountryId = CountryId;
		if (CountryId == null) return;

		var Country = _GetResponse.FullProfile.Data.AllCountries.FirstOrDefault(X => X.Id == CountryId);
		if (Country != null && _EditRequest.RegionId != Country.RegionId) OnRegionChanged(Country.RegionId);
	}
	private void OnAgeChanged()
	{
		_EditRequest.ToAge = Math.Max(_EditRequest.ToAge, _EditRequest.FromAge);
		_AgeChanged = true;
	}
	private async Task OnSaveChangesClick()
	{
		_IsSubmitting = true;
		await SaveChangesAsync();
		_IsSubmitting = false;
	}
}
