namespace SimplyMeetApi.Services;

public class MainHubService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Const Fields
	private const String USER_GROUP = "USER_";
	private const String CHAT_GROUP = "CHAT_";
	#endregion
	#region Fields
	private readonly AuthorizationService _AuthorizationService;
	private readonly DatabaseService _DatabaseService;
	private readonly ProfileCompactService _ProfileCompactService;

	private readonly IHubContext<MainHub> _MainHubContext;
	private readonly ConcurrentDictionary<Int32, ConcurrentHashSet<String>> _ConnectionIdDict;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public MainHubService(AuthorizationService InAuthorizationService, DatabaseService InDatabaseService, ProfileCompactService InProfileCompactService, IHubContext<MainHub> InMainHubContext)
	{
		_AuthorizationService = InAuthorizationService;
		_DatabaseService = InDatabaseService;
		_ProfileCompactService = InProfileCompactService;
		_MainHubContext = InMainHubContext;
		_ConnectionIdDict = new ();
	}

	// Events
	public async Task OnConnectedAsync(HubCallerContext InContext)
	{
		if (InContext == null) throw new ArgumentNullException(nameof(InContext));

		var Auth = await _AuthorizationService.GetHubAuthAsync(InContext, true);
		if (Auth.AccountId != -1)
		{
			if (!_ConnectionIdDict.TryGetValue(Auth.AccountId, out var ConnectionIdList)) _ConnectionIdDict.TryAdd(Auth.AccountId, ConnectionIdList = new ());
			ConnectionIdList.Add(Auth.ConnectionId);

			await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				await AddToGroupsAsync(Auth, InConnection);
				await SendLocalUserAsync(Auth, InConnection);
				await SendAllMatchUsersAsync(Auth, InConnection);
			});
		}
	}
	public async Task OnDisconnectedAsync(HubCallerContext InContext)
	{
		if (InContext == null) throw new ArgumentNullException(nameof(InContext));

		var Auth = await _AuthorizationService.GetHubAuthAsync(InContext);
		if (Auth.AccountId != -1 && _ConnectionIdDict.TryGetValue(Auth.AccountId, out var ConnectionIdSet))
		{
			ConnectionIdSet.TryRemove(Auth.ConnectionId);
			if (ConnectionIdSet.Count <= 0) _ConnectionIdDict.TryRemove(Auth.AccountId, out var Value);
		}
	}
	public async Task OnLocalUserChangedAsync(Int32 InAccountId, IDbConnection InConnection)
	{
		if (InAccountId < 0) throw new ArgumentOutOfRangeException(nameof(InAccountId));
		if (InConnection == null) throw new ArgumentNullException(nameof(InConnection));

		if (!_ConnectionIdDict.TryGetValue(InAccountId, out var ConnectionIdList)) return;

		foreach (var ConnectionId in ConnectionIdList)
		{
			var Auth = await _AuthorizationService.GetHubAuthAsync(ConnectionId, InAccountId, InConnection, true);
			await SendLocalUserAsync(Auth, InConnection);
		}
	}
	public async Task OnMatchAsync(MatchModel InMatch, IDbConnection InConnection)
	{
		if (InMatch == null) throw new ArgumentNullException(nameof(InMatch));
		if (InConnection == null) throw new ArgumentNullException(nameof(InConnection));

		if (_ConnectionIdDict.TryGetValue(InMatch.AccountId, out var ConnectionIdList))
		{
			foreach (var ConnectionId in ConnectionIdList)
				await AddToGroupsAsync(new AuthHubModel(ConnectionId, InMatch.AccountId), InConnection);
		}

		if (_ConnectionIdDict.TryGetValue(InMatch.MatchAccountId, out var MatchConnectionIdList))
		{
			foreach (var ConnectionId in MatchConnectionIdList)
				await AddToGroupsAsync(new AuthHubModel(ConnectionId, InMatch.MatchAccountId), InConnection);
		}

		var FirstMatchUser = await GetMatchUserAsync(InMatch.MatchAccountId, InMatch.Id, InConnection);
		var SecondMatchUser = await GetMatchUserAsync(InMatch.AccountId, InMatch.Id, InConnection);

		await SendMatchUsersAsync(InMatch.AccountId, new [] { FirstMatchUser }, InConnection);
		await SendMatchUsersAsync(InMatch.MatchAccountId, new [] { SecondMatchUser }, InConnection);
	}
	public async Task OnUnmatchAsync(Int32 InAccountId, Int32 InMatchId)
	{
		if (InAccountId < 0) throw new ArgumentOutOfRangeException(nameof(InAccountId));
		if (InMatchId < 0) throw new ArgumentOutOfRangeException(nameof(InMatchId));

		if (!_ConnectionIdDict.TryGetValue(InAccountId, out var ConnectionIdList)) return;

		foreach (var ConnectionId in ConnectionIdList)
		{
			var Auth = new AuthHubModel(ConnectionId, InAccountId);
			await RemoveFromGroupAsync(Auth, $"{CHAT_GROUP}{InMatchId}");
			await _MainHubContext.Clients.Client(ConnectionId).SendAsync(MainHubConstants.RECEIVE_UNMATCH, InMatchId);
		}
	}

	// Messages
	public async Task ChatGetHistoryAsync(ServiceHubModel<ChatGetHistoryRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		var SendModel = await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			var Match = await _DatabaseService.GetModelByIdAsync(new MatchModel { Id = InModel.Request.MatchId }, InConnection);
			if (Match == null || (InModel.Auth.AccountId != Match.AccountId && InModel.Auth.AccountId != Match.MatchAccountId)) return null;

			var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
			if (Account == null || await _DatabaseService.UpdateAccountActiveAsync(Account, InConnection) <= 0) return null;

			var Messages = Enumerable.Empty<MessageModel>();
			if (InModel.Request.Forward) Messages = await _DatabaseService.GetFollowingMessagesAsync(InModel.Request, InConnection);
			else Messages = await _DatabaseService.GetPreviousMessagesAsync(InModel.Request, InConnection);

			var LastMessage = Messages.LastOrDefault();
			var RemainingMessageCount = 0;

			if (LastMessage != null)
			{
				InModel.Request.StartingMessageId = LastMessage.Id;
				if (InModel.Request.Forward) RemainingMessageCount = await _DatabaseService.GetFollowingMessageCountAsync(InModel.Request, InConnection);
				else RemainingMessageCount = await _DatabaseService.GetPreviousMessageCountAsync(InModel.Request, InConnection);
			}

			return new ChatHistorySendModel
			{
				Messages = Messages,
				MatchId = InModel.Request.MatchId,
				RemainingMessageCount = RemainingMessageCount,
				Forward = InModel.Request.Forward,
			};
		});

		if (SendModel != null) await _MainHubContext.Clients.Client(InModel.Auth.ConnectionId).SendAsync(MainHubConstants.RECEIVE_CHAT_HISTORY, SendModel);
	}
	public async Task ChatSendAsync(ServiceHubModel<ChatSendRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));
		if (String.IsNullOrEmpty(InModel.Request.ClientDataJson_Encrypted_Base64)) throw new ArgumentException(nameof(InModel.Request.ClientDataJson_Encrypted_Base64));

		var MessageInfo = await GetMessageInfoAsync(InModel.Auth, InModel.Request.MatchId);
		if (MessageInfo == null) return;

		var ServerData = new MessageServerDataModel { DateTime = DateTime.UtcNow };
		var ServerDataJson = JsonSerializer.Serialize(ServerData, ApiJsonSerializerContext.Default.MessageServerDataModel);

		var Message = new MessageModel
		{
			MatchId = InModel.Request.MatchId,
			FromPublicId = MessageInfo.FromPublicId,
			// ToPublicKeyId = MessageInfo.PublicKey.Id,
			ServerDataJson = ServerDataJson,
			ClientDataJson_Encrypted_Base64 = InModel.Request.ClientDataJson_Encrypted_Base64,
			ClientDataJson_Nonce_Base64 = InModel.Request.ClientDataJson_Nonce_Base64,
		};

		await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			Message = Message with { Id = await _DatabaseService.InsertModelReturnIdAsync(Message, InConnection) };
		});

		await _MainHubContext.Clients.Group($"{CHAT_GROUP}{Message.MatchId}").SendAsync(MainHubConstants.RECEIVE_CHAT_MESSAGE, Message);
	}

	// Notifications
	public async Task SendLocalUserAsync(AuthHubModel InAuth, IDbConnection InConnection)
	{
		if (InAuth == null) throw new ArgumentNullException(nameof(InAuth));
		if (InConnection == null) throw new ArgumentNullException(nameof(InConnection));

		var CompactProfile = await _ProfileCompactService.GetAsync(InAuth.AccountId, EMatchChoice.None, InConnection);
		var LocalUser = new MainHubLocalUserModel { CompactProfile = CompactProfile, Roles = InAuth.Roles.Select(X => X.Name) };
		await _MainHubContext.Clients.Client(InAuth.ConnectionId).SendAsync(MainHubConstants.RECEIVE_LOCAL_USER, LocalUser);
	}
	public async Task SendAllMatchUsersAsync(AuthHubModel InAuth, IDbConnection InConnection)
	{
		if (InAuth == null) throw new ArgumentNullException(nameof(InAuth));
		if (InConnection == null) throw new ArgumentNullException(nameof(InConnection));

		var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InAuth.AccountId }, InConnection);
		if (Account == null) return;

		var Matches = await _DatabaseService.GetAllMatchesAsync(Account, InConnection);
		if (Matches == null) return;

		var ChatUserList = new List<MainHubMatchUserModel>();
		foreach (var Match in Matches)
		{
			var ActualMatchAccountId = Match.AccountId == Account.Id ? Match.MatchAccountId : Match.AccountId;
			var MatchUser = await GetMatchUserAsync(ActualMatchAccountId, Match.Id, InConnection);
			ChatUserList.Add(MatchUser);
		}

		await SendMatchUsersAsync(Account.Id, ChatUserList, null);
	}
	public async Task SendUserUpdateAsync(AccountModel InAccount, IDbConnection InConnection)
	{
		if (InAccount == null) throw new ArgumentNullException(nameof(InAccount));
		if (InConnection == null) throw new ArgumentNullException(nameof(InConnection));

		var CompactProfile = await _ProfileCompactService.GetFromAccountAsync(InAccount, EMatchChoice.None, InConnection);
		var Matches = await _DatabaseService.GetAllMatchesAsync(InAccount, InConnection);

		if (Matches.Count() <= 0) await OnLocalUserChangedAsync(InAccount.Id, InConnection);
		else foreach (var Match in Matches) await _MainHubContext.Clients.Group($"{CHAT_GROUP}{Match.Id}").SendAsync(MainHubConstants.RECEIVE_USER_UPDATE, CompactProfile);
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	// Groups
	private async Task<IEnumerable<String>> GetGroupNamesAsync(AuthHubModel InAuth, IDbConnection InConnection)
	{
		var GroupNameList = new List<String> { $"{USER_GROUP}{InAuth.AccountId}" };
		var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InAuth.AccountId }, InConnection);
		if (Account == null) return GroupNameList;

		var Matches = await _DatabaseService.GetAllMatchesAsync(Account, InConnection);
		if (Matches == null) return GroupNameList;

		GroupNameList.AddRange(Matches.Select(X => $"{CHAT_GROUP}{X.Id}"));
		return GroupNameList;
	}
	private async Task AddToGroupsAsync(AuthHubModel InAuth, IDbConnection InConnection)
	{
		var GroupNames = await GetGroupNamesAsync(InAuth, InConnection);
		if (GroupNames != null) foreach (var GroupName in GroupNames) await _MainHubContext.Groups.AddToGroupAsync(InAuth.ConnectionId, GroupName);
	}
	private async Task RemoveFromGroupAsync(AuthHubModel InAuth, String InGroupName)
	{
		await _MainHubContext.Groups.RemoveFromGroupAsync(InAuth.ConnectionId, InGroupName);
	}

	// Messages
	private async Task<MessageInfoModel> GetMessageInfoAsync(AuthHubModel InAuth, Int32 InMatchId)
	{
		return await _DatabaseService.PerformTransactionAsync<MessageInfoModel>(async InConnection =>
		{
			var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InAuth.AccountId }, InConnection);
			if (Account == null) return null;

			var Match = new MatchModel { Id = InMatchId };
			Match = await _DatabaseService.GetModelByIdAsync<MatchModel>(Match, InConnection);
			if (Match == null || (Match.AccountId != Account.Id && Match.MatchAccountId != Account.Id)) return null;

			var MatchAccount = new AccountModel { Id = Match.MatchAccountId };
			MatchAccount = await _DatabaseService.GetModelByIdAsync<AccountModel>(MatchAccount, InConnection);
			if (MatchAccount == null) return null;

			return new MessageInfoModel(Account.PublicId);
		});
	}

	// Notifications
	private async Task SendMatchUsersAsync(Int32 InAccountId, IEnumerable<MainHubMatchUserModel> InMatchUsers, IDbConnection InConnection)
	{
		await _MainHubContext.Clients.Group($"{USER_GROUP}{InAccountId}").SendAsync(MainHubConstants.RECEIVE_MATCH_USERS, InMatchUsers);
	}

	// Helper
	private async Task<MainHubMatchUserModel> GetMatchUserAsync(Int32 InAccountId, Int32 InMatchId, IDbConnection InConnection)
	{
		var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InAccountId }, InConnection);
		if (Account == null) return null;

		var CompactProfile = await _ProfileCompactService.GetFromAccountAsync(Account, EMatchChoice.None, InConnection);

		return new MainHubMatchUserModel
		{
			CompactProfile = CompactProfile,
			PublicKey = Convert.FromBase64String(Account.PublicKey_Base64),
			MatchId = InMatchId
		};
	}
}