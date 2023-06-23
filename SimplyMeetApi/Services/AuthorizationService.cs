namespace SimplyMeetApi.Services;

public class AuthorizationService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly DatabaseService _DatabaseService;
	private readonly AdminConfiguration _AdminConfig;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public AuthorizationService(DatabaseService InDatabaseService, IOptions<AdminConfiguration> InAdminConfig)
	{
		_DatabaseService = InDatabaseService;
		_AdminConfig = InAdminConfig.Value;
	}

	public async Task<AuthModel> GetControllerAuthAsync(HttpContext InContext, Boolean InGetRoles = false)
	{
		if (InContext == null) throw new ArgumentNullException(nameof(InContext));

		var AccountId = GetAccountId(InContext.User);
		if (!InGetRoles || AccountId == -1) return new AuthModel(InContext.Connection.RemoteIpAddress, AccountId, Enumerable.Empty<RoleModel>());

		return new AuthModel(InContext.Connection.RemoteIpAddress, AccountId, await GetRolesAsync(AccountId));
	}
	public async Task<AuthHubModel> GetHubAuthAsync(HubCallerContext InContext, Boolean InGetRoles = false)
	{
		if (InContext == null) throw new ArgumentNullException(nameof(InContext));

		var AccountId = GetAccountId(InContext.User);
		if (!InGetRoles || AccountId == -1) return new AuthHubModel(InContext.ConnectionId, AccountId);

		return new AuthHubModel(InContext.ConnectionId, AccountId, await GetRolesAsync(AccountId));
	}
	public async Task<AuthHubModel> GetHubAuthAsync(String InConnectionId, Int32 InAccountId, IDbConnection InConnection, Boolean InGetRoles = false)
	{
		if (InConnectionId == null) throw new ArgumentNullException(nameof(InConnectionId));
		if (InAccountId < 0) throw new ArgumentOutOfRangeException(nameof(InAccountId));
		if (InConnection == null) throw new ArgumentNullException(nameof (InConnection));

		if (!InGetRoles) return new AuthHubModel(InConnectionId, InAccountId);
		return new AuthHubModel(InConnectionId, InAccountId, await GetRolesAsync(InAccountId, InConnection));
	}

	public Boolean HasRole(AuthModel InAuth, EAccountRole InAccountRole)
	{
		if (InAuth == null) throw new ArgumentNullException(nameof(InAuth));

		return HasRole(InAuth.Roles, InAccountRole);
	}
	public Boolean HasRole(AuthHubModel InAuth, EAccountRole InAccountRole)
	{
		if (InAuth == null) throw new ArgumentNullException(nameof(InAuth));

		return HasRole(InAuth.Roles, InAccountRole);
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task<IEnumerable<RoleModel>> GetRolesAsync(Int32 InAccountId)
	{
		return await _DatabaseService.PerformTransactionAsync(async InConnection => await GetRolesAsync(InAccountId, InConnection));
	}
	private async Task<IEnumerable<RoleModel>> GetRolesAsync(Int32 InAccountId, IDbConnection InConnection)
	{
		var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InAccountId }, InConnection);
		if (Account == null) return Enumerable.Empty<RoleModel>();

		var Roles = await _DatabaseService.GetAccountRolesAsync(Account, InConnection);
		if (Account.PublicId == _AdminConfig.MainAdminPublicId) Roles = Roles.Append(new RoleModel { Name = nameof(EAccountRole.Admin) });
		return Roles;
	}

	private Int32 GetAccountId(ClaimsPrincipal InUser) => Int32.Parse(InUser.Claims.FirstOrDefault(X => X.Type == ClaimTypes.NameIdentifier)?.Value ?? "-1");
	private Boolean HasRole(IEnumerable<RoleModel> InRoles, EAccountRole InAccountRole) => InRoles.Any(X => X.Name == InAccountRole.ToString());
}