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
	private readonly SettingsService _SettingsService;

	private readonly HttpClient _HttpClient;
	#endregion
	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public HttpService(AppState InAppState, LocalizationService InLocalizationService, LocalStorageService InLocalStorageService, NavigationService InNavigationService, NotificationService InNotificationService, SettingsService InSettingsService, HttpClient InHttpClient)
	{
		_AppState = InAppState;
		_LocalizationService = InLocalizationService;
		_LocalStorageService = InLocalStorageService;
		_NavigationService = InNavigationService;
		_NotificationService = InNotificationService;
		_SettingsService = InSettingsService;

		_HttpClient = InHttpClient;
	}

	public async Task<Stream> GetFileAsync(String InRequestPath)
	{
		return await GetFileAsync(await _SettingsService.GetApiServerAsync(), InRequestPath);
	}
	public async Task<Stream> GetFileAsync(Uri InApiServerUri, String InRequestPath)
	{
		try
		{
			var RequestUri = _NavigationService.ToAbsoluteUri(InRequestPath);
			return await _HttpClient.GetStreamAsync(RequestUri);
		}

		catch (Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
			return default;
		}
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
				RequestUri = new Uri(await _SettingsService.GetApiServerAsync(), InRequestUri),
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
	public async Task<TResponse> PostJsonRequestAsync<TRequest, TResponse>(String InRequestPath, TRequest InRequestModel)
		where TRequest : RequestModelBase
		where TResponse : ResponseModelBase, new()
	{
		return await PostJsonRequestAsync<TRequest, TResponse>(await _SettingsService.GetApiServerAsync(), InRequestPath, InRequestModel);
	}
	public async Task<TResponse> PostJsonRequestAsync<TRequest, TResponse>(Uri InApiServerUri, String InRequestPath, TRequest InRequestModel)
		where TRequest : RequestModelBase
		where TResponse : ResponseModelBase, new()
	{
		if (InRequestPath == null) throw new ArgumentNullException(nameof(InRequestPath));
		if (InRequestModel == null) throw new ArgumentNullException(nameof(InRequestModel));

		try
		{
			var Request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri(InApiServerUri, InRequestPath),
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
