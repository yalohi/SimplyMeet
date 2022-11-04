namespace SimplyMeetWasm.Services;

public class HttpService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly AppState _AppState;
	private readonly LocalizationService _LocalizationService;
	private readonly LocalStorageService _LocalStorageService;
	private readonly NavigationService _NavigationService;
	private readonly NotificationService _NotificationService;

	private readonly HttpClient _HttpClient;
	#endregion
	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public HttpService(AppState InAppState, LocalizationService InLocalizationService, LocalStorageService InLocalStorageService, NavigationService InNavigationService, NotificationService InNotificationService, HttpClient InHttpClient)
	{
		_AppState = InAppState;
		_LocalizationService = InLocalizationService;
		_LocalStorageService = InLocalStorageService;
		_NavigationService = InNavigationService;
		_NotificationService = InNotificationService;

		_HttpClient = InHttpClient;
	}

	public async Task<T> PostFileAsync<T>(String InRequestUri, Stream InStream, String InContentType)
	{
		if (InRequestUri == null) throw new ArgumentNullException(nameof(InRequestUri));
		if (InStream == null) throw new ArgumentNullException(nameof(InStream));
		if (InContentType == null) throw new ArgumentNullException(nameof(InContentType));

		var Content = new MultipartFormDataContent();
		var FileContent = new StreamContent(InStream);
		Content.Add(FileContent, "InFile", "InFile");

		try
		{
			var Request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri($"{_HttpClient.BaseAddress}{InRequestUri}"),
				Content = Content
			};

			Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(InContentType));
			await AddAuthHeaderAsync(Request);

			var Response = await _HttpClient.SendAsync(Request);
			return Response.StatusCode == HttpStatusCode.OK ? await Response.Content.ReadFromJsonAsync<T>() : default;
		}

		catch
		{
			return default;
		}
	}
	public async Task<TResponse> PostJsonRequestAsync<TRequest, TResponse>(String InRequestUri, TRequest InRequestModel)
		where TRequest : RequestModelBase
		where TResponse : ResponseModelBase, new()
	{
		if (InRequestUri == null) throw new ArgumentNullException(nameof(InRequestUri));
		if (InRequestModel == null) throw new ArgumentNullException(nameof(InRequestModel));

		try
		{
			var Request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri($"{_HttpClient.BaseAddress}{InRequestUri}"),
				Content = new StringContent(JsonSerializer.Serialize(InRequestModel)),
			};

			Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			await AddAuthHeaderAsync(Request);

			var Response = await _HttpClient.SendAsync(Request);
			if (Response.StatusCode == HttpStatusCode.Unauthorized) _NavigationService.NavigateTo(NavigationConstants.NAV_HOME);
			return Response.StatusCode == HttpStatusCode.OK ? await Response.Content.ReadFromJsonAsync<TResponse>() : new TResponse { ErrorCode = Response.StatusCode };
		}

		catch
		{
			return default;
		}
	}

	public Boolean ValidateResponse(ResponseModelBase InResponse)
	{
		if (InResponse == null) _NotificationService.SetMainNotification(_LocalizationService[ErrorConstants.ERROR_REQUEST_FAILED], ENotificationType.Danger);
		else if (InResponse.ErrorCode != null) _NotificationService.SetMainNotification(_LocalizationService[InResponse.ErrorCode.ToString()], ENotificationType.Danger);
		else if (!String.IsNullOrEmpty(InResponse.Error)) _NotificationService.SetMainNotification(_LocalizationService[InResponse.Error], ENotificationType.Danger);
		else return true;

		return false;
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task AddAuthHeaderAsync(HttpRequestMessage InRequest)
	{
		var Token = await _AppState.GetLoginTokenAsync();
		if (Token != null) InRequest.Headers.Authorization = new AuthenticationHeaderValue(TokenConstants.TYPE, Token);
	}
}