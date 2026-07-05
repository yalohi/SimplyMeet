namespace SimplyMeetWasm.Pages;

public partial class IndexPage : PageBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	[Inject] public HttpService HttpService { get; set; } = default!;
	#endregion
	#region Fields
	private HomeGetDataResponseModel _GetResponse;
	private Uri _BannerUri;
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
		await base.OnSetupAsync();
		await ReloadCardsAsync();
		var ApiServerUri = await SettingsService.GetApiServerAsync();
		if (!String.IsNullOrEmpty(_GetResponse?.BannerImage)) _BannerUri = new Uri(ApiServerUri, $"{ApiRequestConstants.IMAGES}/{_GetResponse.BannerImage}");
		else _BannerUri = null;
		AppState.ShowFooter = true;
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task ReloadCardsAsync()
	{
		var RequestModel = new HomeGetDataRequestModel();
		_IsRequesting = true;
		_GetResponse = await HttpService.PostJsonRequestAsync<HomeGetDataRequestModel, HomeGetDataResponseModel>(ApiRequestConstants.HOME_GET_DATA, RequestModel);
		_IsRequesting = false;
		if (!HttpService.ValidateResponse(_GetResponse)) return;
	}
}
