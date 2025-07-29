namespace SimplyMeetWasm.Services;

public class SettingsService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly LocalStorageService _LocalStorageService;
	private List<Uri> _CustomApiServerList;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public SettingsService(LocalStorageService InLocalStorageService)
	{
		_LocalStorageService = InLocalStorageService;
	}

	public async Task<Uri> GetApiServerAsync()
	{
		var ApiServer = await _LocalStorageService.GetItemAsync<String>(LocalStorageConstants.API_SERVER_STORAGE_KEY);
		return String.IsNullOrEmpty(ApiServer) ? null : new Uri(ApiServer);
	}
	public async Task<List<Uri>> GetCustomApiServerListAsync()
	{
		var CustomApiServerList = await _LocalStorageService.GetItemAsync<List<Uri>>(LocalStorageConstants.CUSTOM_API_SERVER_LIST_STORAGE_KEY);
		return CustomApiServerList ?? new List<Uri>();
	}

	public async Task SetApiServerAsync(String InApiServer)
	{
		if (String.IsNullOrEmpty(InApiServer)) await _LocalStorageService.RemoveItemAsync(LocalStorageConstants.API_SERVER_STORAGE_KEY);
		else await _LocalStorageService.SetItemAsync<String>(LocalStorageConstants.API_SERVER_STORAGE_KEY, InApiServer);
	}
	public async Task AddCustomApiServerAsync(Uri InApiServerUri)
	{
		ArgumentNullException.ThrowIfNull(InApiServerUri);

		_CustomApiServerList ??= await GetCustomApiServerListAsync();
		_CustomApiServerList.Add(InApiServerUri);
		await _LocalStorageService.SetItemAsync<List<Uri>>(LocalStorageConstants.CUSTOM_API_SERVER_LIST_STORAGE_KEY, _CustomApiServerList);
	}
	public async Task RemoveCustomApiServerAsync(Uri InApiServerUri)
	{
		ArgumentNullException.ThrowIfNull(InApiServerUri);

		_CustomApiServerList ??= await GetCustomApiServerListAsync();
		var Index = _CustomApiServerList.IndexOf(InApiServerUri);
		if (Index == -1) return;

		_CustomApiServerList.RemoveAt(Index);
		await _LocalStorageService.SetItemAsync<List<Uri>>(LocalStorageConstants.CUSTOM_API_SERVER_LIST_STORAGE_KEY, _CustomApiServerList);
	}
}
