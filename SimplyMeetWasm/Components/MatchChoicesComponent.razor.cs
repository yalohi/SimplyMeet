namespace SimplyMeetWasm.Components;

public partial class MatchChoicesComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Parameter] public String PageLink { get; set; }
	[Parameter] public Int32 PageIndex { get; set; }
	[Parameter] public EMatchChoice Choice { get; set; }
	#endregion
	#region Fields
	private MatchGetChoicesResponseModel _GetResponse;
	private ProfileCompactComponent _ActiveProfile;
	private Boolean _IsRequesting;
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		await GetChoicesAsync();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task GetChoicesAsync()
	{
		_GetResponse = null;

		var RequestModel = new MatchGetChoicesRequestModel
		{
			Offset = ApiRequestConstants.MATCH_GET_CHOICES_LOAD_COUNT * PageIndex,
			Count = ApiRequestConstants.MATCH_GET_CHOICES_LOAD_COUNT,
			Choice = Choice,
		};

		_IsRequesting = true;
		_GetResponse = await HttpService.PostJsonRequestAsync<MatchGetChoicesRequestModel, MatchGetChoicesResponseModel>(ApiRequestConstants.MATCH_GET_CHOICES, RequestModel);
		_IsRequesting = false;

		if (!HttpService.ValidateResponse(_GetResponse)) return;
	}

	private void OnProfileClick(ProfileCompactComponent InCompactProfile)
	{
		if (_ActiveProfile != InCompactProfile) _ActiveProfile?.SetIsActive(false);
		_ActiveProfile = InCompactProfile;
	}
}
