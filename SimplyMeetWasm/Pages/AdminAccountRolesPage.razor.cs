namespace SimplyMeetWasm.Pages;

public partial class AdminAccountRolesPage : PageBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Inject] public IStringLocalizer<SharedResource> Loc { get; set; } = default!;
	[Inject] public HttpService HttpService { get; set; } = default!;

	public Boolean IsAddDisabled => _NewRoleId == -1 || _NewAccountPublicId == null || _NewAccountPublicId.Length != AccountConstants.PUBLIC_ID_LENGTH;
	public Boolean IsSaveDisabled => _IsSubmitting || !_DataChanged;
	#endregion
	#region Fields
	private AdminGetAccountRolesResponseModel _GetResponse;
	private ProfileCompactComponent _ActiveProfile;

	private List<AccountRoleModel> _AccountRoleList;
	private List<Int32> _RemovedIdList;

	private String _NewAccountPublicId;
	private Int32 _NewRoleId;
	private Boolean _IsRequesting;
	private Boolean _IsSubmitting;
	private Boolean _DataChanged;
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
		await ReloadAccountRolesAsync();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task ReloadAccountRolesAsync()
	{
		var RequestModel = new AdminGetAccountRolesRequestModel();
		_IsRequesting = true;
		_GetResponse = await HttpService.PostJsonRequestAsync<AdminGetAccountRolesRequestModel, AdminGetAccountRolesResponseModel>(ApiRequestConstants.ADMIN_GET_ACCOUNT_ROLES, RequestModel);
		_IsRequesting = false;
		if (!HttpService.ValidateResponse(_GetResponse)) return;

		NotificationService.ClearMainNotification();

		_AccountRoleList = _GetResponse.AccountRoles.ToList();
		_RemovedIdList = new ();
		_NewAccountPublicId = String.Empty;
		_NewRoleId = -1;
		_DataChanged = false;
	}
	private async Task SaveChangesAsync()
	{
		var EditRequest = new AdminEditAccountRolesRequestModel { RemovedIds = _RemovedIdList, NewAccountRoles = _AccountRoleList.Where(X => X.Id <= 0) };
		var Response = await HttpService.PostJsonRequestAsync<AdminEditAccountRolesRequestModel, AdminEditAccountRolesResponseModel>(ApiRequestConstants.ADMIN_EDIT_ACCOUNT_ROLES, EditRequest);
		if (!HttpService.ValidateResponse(Response)) return;

		await ReloadAccountRolesAsync();
	}

	private async Task OnSaveChangesClick()
	{
		_IsSubmitting = true;
		await SaveChangesAsync();
		_IsSubmitting = false;
	}

	private void OnAddAccountRoleClick()
	{
		var NewAccountRole = new AccountRoleModel { RoleId = _NewRoleId, AccountPublicId = _NewAccountPublicId };
		NotificationService.ClearMainNotification();

		if (_AccountRoleList.Any(X => X.RoleId == NewAccountRole.RoleId && X.AccountPublicId == NewAccountRole.AccountPublicId))
		{
			NotificationService.SetMainNotification(Loc["AccountRoleExists"], ENotificationType.Warning);
			return;
		}

		_AccountRoleList.Add(NewAccountRole);
		_DataChanged = true;
	}
	private void OnProfileClick(ProfileCompactComponent InCompactProfile)
	{
		if (_ActiveProfile != InCompactProfile) _ActiveProfile?.SetIsActive(false);
		_ActiveProfile = InCompactProfile;
	}
	private void OnRemoveAccountRoleClick(AccountRoleModel InAccountRole)
	{
		NotificationService.ClearMainNotification();

		_AccountRoleList.RemoveAll(X => X.RoleId == InAccountRole.RoleId && X.AccountPublicId == InAccountRole.AccountPublicId);
		if (InAccountRole.Id > 0) _RemovedIdList.Add(InAccountRole.Id);
		_DataChanged = true;
	}
}
