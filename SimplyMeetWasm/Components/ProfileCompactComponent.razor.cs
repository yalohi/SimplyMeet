namespace SimplyMeetWasm.Components;

public partial class ProfileCompactComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public AccountService AccountService { get; set; }
	[Inject] public HttpService HttpService { get; set; }
	[Inject] public ProfileService ProfileService { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }
	[Parameter] public String ChildCssClass { get; set; }

	// Compact Profile
	[Parameter] public ProfileCompactModel CompactProfile { get; set; }
	[Parameter] public String PublicId { get; set; }
	[Parameter] public String CssClass { get; set; }
	[Parameter] public String ActiveCssClass { get; set; }
	[Parameter] public Boolean ShowProfileButton { get; set; }
	[Parameter] public Boolean ShowRemoveButton { get; set; }
	[Parameter] public Boolean ShowNavBarButton { get; set; }

	[Parameter] public EventCallback<ProfileCompactComponent> ProfileClick { get; set; }
	[Parameter] public EventCallback RemoveClick { get; set; }
	[Parameter] public EventCallback NavBarClick { get; set; }

	// Profile
	[Parameter] public String ProfileCssClass { get; set; }
	[Parameter] public Boolean ShowReportButton { get; set; }
	[Parameter] public Boolean ShowSuspendButton { get; set; }
	[Parameter] public Boolean ShowUnmatchButton { get; set; }
	[Parameter] public Boolean ShowRateButtons { get; set; }

	[Parameter] public EventCallback SuspendClick { get; set; }
	[Parameter] public EventCallback RateClick { get; set; }
	#endregion
	#region Properties
	public Boolean IsActive { get; private set; }
	#endregion
	#region Fields
	private String _AvatarUri;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public async Task ReloadAsync()
	{
		if (!String.IsNullOrEmpty(PublicId))
		{
			var RequestModel = new ProfileGetCompactRequestModel { PublicId = PublicId };
			var Response = await HttpService.PostJsonRequestAsync<ProfileGetCompactRequestModel, ProfileGetCompactResponseModel>(ApiRequestConstants.PROFILE_GET_COMPACT, RequestModel);
			if (!HttpService.ValidateResponse(Response)) return;

			CompactProfile = Response.CompactProfile;
		}

		_AvatarUri = (await ProfileService.GetAvatarUriAsync(CompactProfile.Avatar)).ToString();
	}

	public void SetIsActive(Boolean InIsActive)
	{
		IsActive = InIsActive;
	}
	public void ToggleActive()
	{
		IsActive = !IsActive;
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
	private async Task OnProfileClick()
	{
		ToggleActive();
		await ProfileClick.InvokeAsync(this);
	}
}
