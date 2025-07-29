namespace SimplyMeetWasm.Components;

public partial class ChooseServerComponent : InputBase<String>
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject]
	public IConfiguration Config { get; set; }
	[Inject]
	public HttpService HttpService { get; set; }
	[Inject]
	public SettingsService SettingsService { get; set; }
	#endregion
	#region Fields
	private ContainerComponent<TableListComponent> _ServerListContainer;
	private TableListComponent _OfficialServerTable;
	private TableListComponent _CustomServerTable;
	private List<String> _HeaderColumnList;

	private List<List<String>> _OfficialServerList;
	private List<List<String>> _CustomServerList;

	private HomeGetServerInfoResponseModel _ServerInfoResponse;
	private String _NewCustomApiServer;
	private Boolean _IsRequestingServerInfo;
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		_HeaderColumnList = [ Loc["Server"] ];
		_OfficialServerList = [];
		_CustomServerList = [];
	}
	protected override async Task OnAfterRenderAsync(Boolean InFirstRender)
	{
		await base.OnAfterRenderAsync(InFirstRender);
		if (!InFirstRender) return;

		await RequestOfficialServerListAsync();
		await FillCustomServerListAsync();
		await SelectCurrentApiServerAsync();
	}
	protected override Boolean TryParseValueFromString(String InValue, out String OutResult, out String OutValidationErrorMessage)
	{
		OutResult = InValue;
		OutValidationErrorMessage = null;
		return true;
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task RequestOfficialServerListAsync()
	{
		_OfficialServerList.Clear();

		var OfficialServerListUri = Config["OfficialServerListUri"];
		using var Stream = await HttpService.GetFileAsync(OfficialServerListUri);
		if (Stream == null) return;

		var ApiServerListModel = JsonSerializer.Deserialize<ApiServerListModel>(Stream);

		foreach (var ApiServer in ApiServerListModel.List)
			if (Uri.TryCreate(ApiServer, UriKind.Absolute, out var ApiServerUri))
				_OfficialServerList.Add([ ApiServerUri.ToString() ]);

		StateHasChanged();
	}
	private async Task RequestServerInfoAsync(String InApiServer)
	{
		if (!Uri.TryCreate(InApiServer, UriKind.Absolute, out var ApiServerUri)) return;

		var RequestModel = new HomeGetServerInfoRequestModel();
		_IsRequestingServerInfo = true;
		_ServerInfoResponse = await HttpService.PostJsonRequestAsync<HomeGetServerInfoRequestModel, HomeGetServerInfoResponseModel>(ApiServerUri, ApiRequestConstants.HOME_GET_SERVER_INFO, RequestModel);
		_IsRequestingServerInfo = false;

		StateHasChanged();
	}
	private async Task FillCustomServerListAsync()
	{
		var UriList = await SettingsService.GetCustomApiServerListAsync();
		foreach (var Uri in UriList) _CustomServerList.Add([ Uri.ToString() ]);
		StateHasChanged();
	}
	private async Task SelectCurrentApiServerAsync()
	{
		var ApiServerUri = await SettingsService.GetApiServerAsync();
		if (ApiServerUri == null) return;

		var ServerIndex = _OfficialServerList.FindIndex(X => X.Contains(ApiServerUri.AbsoluteUri));
		var Table = _OfficialServerTable;

		if (ServerIndex == -1)
		{
			ServerIndex = _CustomServerList.FindIndex(X => X.Contains(ApiServerUri.AbsoluteUri));
			Table = _CustomServerTable;
		}

		if (ServerIndex == -1) return;

		Table.SelectedIndex = ServerIndex;
		OnListSelectedIndexChanged(Table);
	}
	private async Task AddCustomApiServerAsync(String InApiServer)
	{
		ArgumentException.ThrowIfNullOrEmpty(InApiServer);
		if (!Uri.TryCreate(InApiServer, UriKind.Absolute, out var ApiServerUri)) return;

		var ServerIndex = _CustomServerList.FindIndex(X => X.Contains(ApiServerUri.AbsoluteUri));
		if (ServerIndex != -1) _CustomServerTable.SelectedIndex = ServerIndex;

		else
		{
			await SettingsService.AddCustomApiServerAsync(ApiServerUri);
			_CustomServerList.Add([ ApiServerUri.ToString() ]);
			_CustomServerTable.SelectedIndex = _CustomServerList.Count - 1;
		}

		await _CustomServerTable.SelectedIndexChanged.InvokeAsync(_CustomServerTable);
	}

	private async void OnListSelectedIndexChanged(TableListComponent InTableList)
	{
		foreach (var Component in _ServerListContainer.ChildComponentList)
			if (Component != InTableList)
				Component.SelectedIndex = -1;

		var ApiServer = InTableList.SelectedIndex != -1 ? InTableList.RowList[InTableList.SelectedIndex][0] : String.Empty;
		await ValueChanged.InvokeAsync(ApiServer);
		EditContext?.NotifyFieldChanged(FieldIdentifier);
		_ServerInfoResponse = null;
		if (String.IsNullOrEmpty(ApiServer)) return;

		await RequestServerInfoAsync(ApiServer);
	}
	private async void OnListRemoveRowClick((TableListComponent TableList, Int32 RowIndex) InArgs)
	{
		var ApiServer = InArgs.TableList.RowList[InArgs.RowIndex][0];
		if (!Uri.TryCreate(ApiServer, UriKind.Absolute, out var ApiServerUri)) return;

		await SettingsService.RemoveCustomApiServerAsync(ApiServerUri);
		_CustomServerList.RemoveAt(InArgs.RowIndex);
		StateHasChanged();
	}
	private async void OnAddCustomApiServerClick()
	{
		await AddCustomApiServerAsync(_NewCustomApiServer);
	}
}
