namespace SimplyMeetWasm.Pages;

public partial class MatchNewPage : PageBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	[Inject] public HttpService HttpService { get; set; } = default!;
	#endregion
	#region Fields
	private MatchGetNewResponseModel _GetResponse;
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
	protected override async Task OnSetupAsync()
	{
		if (MainHubService.FirstMatchUser != null)
		{
			NavigationService.NavigateTo(NavigationConstants.NAV_MATCH);
			return;
		}

		await base.OnSetupAsync();
		await GetNextAsync();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task GetNextAsync()
	{
		var RequestModel = new MatchGetNewRequestModel();
		_IsRequesting = true;
		_GetResponse = await HttpService.PostJsonRequestAsync<MatchGetNewRequestModel, MatchGetNewResponseModel>(ApiRequestConstants.MATCH_GET_NEW, RequestModel);
		_IsRequesting = false;

		if (!HttpService.ValidateResponse(_GetResponse)) return;
	}

	private void OnReviewDislikedProfilesClick()
	{
		NavigationService.NavigateTo(NavigationConstants.NAV_MATCH_DISLIKED);
	}
	private void OnAdjustFilterClick()
	{
		NavigationService.NavigateTo(NavigationConstants.NAV_MATCH_FILTER);
	}
}
