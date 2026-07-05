namespace SimplyMeetWasm.Components;

public partial class ProfileComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	[Inject] public AccountService AccountService { get; set; } = default!;
	[Inject] public HttpService HttpService { get; set; } = default!;
	[Inject] public MainHubService MainHubService { get; set; } = default!;
	[Inject] public NotificationService NotificationService { get; set; } = default!;
	[Inject] public ProfileService ProfileService { get; set; } = default!;

	[Parameter] public ProfileFullModel FullProfile { get; set; }
	[Parameter] public String PublicId { get; set; }
	[Parameter] public String CssClass { get; set; }
	[Parameter] public EMatchChoice Choice { get; set; }
	[Parameter] public Boolean ShowReportButton { get; set; }
	[Parameter] public Boolean ShowSuspendButton { get; set; }
	[Parameter] public Boolean ShowUnmatchButton { get; set; }
	[Parameter] public Boolean ShowRateButtons { get; set; }

	[Parameter] public EventCallback ReportClick { get; set; }
	[Parameter] public EventCallback SuspendClick { get; set; }
	[Parameter] public EventCallback RateClick { get; set; }
	#endregion
	#region Fields
	private String _AvatarUri;

	private PronounsModel _Pronouns;
	private SexModel _Sex;
	private GenderModel _Gender;
	private RegionModel _Region;
	private CountryModel _Country;
	private Boolean _IsRequesting;
	private Boolean _ShowDetails;

	private ConfirmModalComponent _ReportConfirmModal;
	private ConfirmModalComponent _SuspendAccountConfirmModal;
	private ConfirmModalComponent _UnmatchConfirmModal;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public async Task ReloadAsync()
	{
		if (!String.IsNullOrEmpty(PublicId))
		{
			_IsRequesting = true;
			var RequestModel = new ProfileGetRequestModel { AccountPublicId = PublicId };
			var Response = await HttpService.PostJsonRequestAsync<ProfileGetRequestModel, ProfileGetResponseModel>(ApiRequestConstants.PROFILE_GET, RequestModel);
			_IsRequesting = false;
			if (!HttpService.ValidateResponse(Response)) return;

			FullProfile = Response.FullProfile;
		}

		_AvatarUri = (await ProfileService.GetAvatarUriAsync(FullProfile.Profile.Avatar)).ToString();

		_Pronouns = FullProfile.Profile.PronounsId != null ? FullProfile.Data.AllPronouns.FirstOrDefault(X => X.Id == FullProfile.Profile.PronounsId) : null;
		_Sex = FullProfile.Profile.SexId != null ? FullProfile.Data.AllSexes.FirstOrDefault(X => X.Id == FullProfile.Profile.SexId) : null;
		_Gender = FullProfile.Profile.GenderId != null ? FullProfile.Data.AllGenders.FirstOrDefault(X => X.Id == FullProfile.Profile.GenderId) : null;
		_Region = FullProfile.Profile.RegionId != null ? FullProfile.Data.AllRegions.FirstOrDefault(X => X.Id == FullProfile.Profile.RegionId) : null;
		_Country = FullProfile.Profile.CountryId != null ? FullProfile.Data.AllCountries.FirstOrDefault(X => X.Id == FullProfile.Profile.CountryId) : null;

		_ShowDetails =
			FullProfile.Profile.BirthDate != null ||
			FullProfile.Profile.PronounsId != null ||
			FullProfile.Profile.SexId != null ||
			FullProfile.Profile.GenderId != null ||
			FullProfile.Profile.RegionId != null ||
			FullProfile.Profile.CountryId != null ||
			FullProfile.Tags.Any() ||
			FullProfile.Sexualities.Any() ||
			FullProfile.Profile.LookingFor != ELookingFor.NotSpecified ||
			!String.IsNullOrEmpty(FullProfile.Profile.AboutMe) ||
			!String.IsNullOrEmpty(FullProfile.Profile.AboutYou);
	}

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		await ReloadAsync();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task OnReportClick()
	{
		var ReportReason = FullProfile.ReportReasons.FirstOrDefault();
		var RequestModel = new ProfileReportRequestModel { AccountPublicId = FullProfile.Account.PublicId, ReportReasonId = ReportReason?.Id ?? -1 };
		var Response = await HttpService.PostJsonRequestAsync<ProfileReportRequestModel, ProfileReportResponseModel>(ApiRequestConstants.PROFILE_REPORT, RequestModel);
		HttpService.ValidateResponse(Response);

		// if (ShowRateButtons && Choice != EMatchChoice.Dislike) await ChooseAsync(EMatchChoice.Dislike);
		await ReportClick.InvokeAsync();
		await ReloadAsync();
	}
	private async Task OnSuspendAccountClick()
	{
		var RequestModel = new AdminSuspendAccountRequestModel { AccountPublicId = FullProfile.Account.PublicId };
		var Response = await HttpService.PostJsonRequestAsync<AdminSuspendAccountRequestModel, AdminSuspendAccountResponseModel>(ApiRequestConstants.ADMIN_SUSPEND_ACCOUNT, RequestModel);
		HttpService.ValidateResponse(Response);

		await SuspendClick.InvokeAsync();
	}
	private async Task OnUnmatchClick()
	{
		var RequestModel = new MatchUnmatchRequestModel { MatchId = MainHubService.FirstMatchUser.MatchId };
		var Response = await HttpService.PostJsonRequestAsync<MatchUnmatchRequestModel, MatchUnmatchResponseModel>(ApiRequestConstants.MATCH_UNMATCH, RequestModel);
		HttpService.ValidateResponse(Response);
	}
	private async Task OnLikeClick() => await ChooseAsync(EMatchChoice.Like);
	private async Task OnDislikeClick() => await ChooseAsync(EMatchChoice.Dislike);

	private async Task ChooseAsync(EMatchChoice InChoice)
	{
		var RequestModel = new MatchChooseRequestModel { PublicId = FullProfile.Account.PublicId, Choice = InChoice };
		var Response = await HttpService.PostJsonRequestAsync<MatchChooseRequestModel, MatchChooseResponseModel>(ApiRequestConstants.MATCH_CHOOSE, RequestModel);
		HttpService.ValidateResponse(Response);

		Choice = InChoice;
		await RateClick.InvokeAsync();
	}
}
