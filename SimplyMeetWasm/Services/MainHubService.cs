
namespace SimplyMeetWasm.Services;

public class MainHubService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Const Fields
	private const String URL_REPLACE = "<a href=\"$&\">$&</a>";
	#endregion
	#region Const Fields
	private static readonly Regex URL_REGEX = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", RegexOptions.Compiled);
	#endregion
	#region Properties
	public Int32 TotalUnreadMessageCount => _MatchUserStateList.Sum(X => X.UnreadMessageCount);

	public MainHubLocalUserModel LocalUser { get; private set; }

	public HubConnectionState State => _Connection?.State ?? HubConnectionState.Connecting;
	public MatchUserStateModel FirstMatchUserState => _MatchUserStateList.FirstOrDefault();
	public MainHubMatchUserModel FirstMatchUser => FirstMatchUserState?.User;
	public Boolean IsAdmin => LocalUser != null && LocalUser.Roles.Any(X => X == EAccountRole.Admin.ToString());
	public Boolean IsModerator => LocalUser != null && LocalUser.Roles.Any(X => X == EAccountRole.Moderator.ToString());
	#endregion
	#region Fields
	private readonly IJSRuntime _JS;
	private readonly AppState _AppState;
	private readonly AccountService _AccountService;
	private readonly LocalizationService _LocalizationService;
	private readonly NavigationService _NavigationService;
	private readonly NotificationService _NotificationService;

	private HubConnection _Connection;
	private List<MatchUserStateModel> _MatchUserStateList;
	private Dictionary<Int32, List<DecryptedMessageModel>> _MessageDict;
	private List<String> _UnsentMessageList;
	private Boolean _IsSetup;
	#endregion
	#region Events
	public event EventHandler LocalUserUpdate;
	public event EventHandler UserUpdate;
	public event EventHandler ChatUpdate;
	public event EventHandler ChatLoad;
	public event EventHandler<String> Connected;
	public event EventHandler<Exception> Disconnected;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public MainHubService(IJSRuntime InJS, AppState InAppState, AccountService InAccountService, LocalizationService InLocalizationService, NavigationService InNavigationService, NotificationService InNotificationService)
	{
		_JS = InJS;
		_AppState = InAppState;
		_AccountService = InAccountService;
		_LocalizationService = InLocalizationService;
		_NavigationService = InNavigationService;
		_NotificationService = InNotificationService;

		_AccountService.Login += async (InSender, InArgs) => await ReconnectAsync();
		_AccountService.Logout += async (InSender, InArgs) => await ReconnectAsync();

		_MessageDict = new ();
		_UnsentMessageList = new ();
		_MatchUserStateList = new ();
	}

	public async Task SetupLazyAsync()
	{
		if (_IsSetup) return;

		await ReconnectAsync();
		_IsSetup = true;
	}
	public async Task ReconnectAsync()
	{
		ResetConnection();

		var Token = await _AppState.GetLoginTokenAsync();

		_Connection = new HubConnectionBuilder()
			.WithUrl(_NavigationService.ToAbsoluteUri($"{ApiRequestConstants.BASE_PATH}{MainHubConstants.PATH}"), InOptions =>
			{
				InOptions.AccessTokenProvider = () => Task.FromResult(Token);
				InOptions.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
				InOptions.SkipNegotiation = true;
			})
			.WithAutomaticReconnect()
			.Build();

		_Connection.Reconnected += async (InConnectionId) =>
		{
			_MatchUserStateList.Clear();
			_MessageDict.Clear();
			Connected?.Invoke(this, InConnectionId);
			await Task.Delay(1); // TEMP
		};

		_Connection.Closed += async (InEx) =>
		{
			_MatchUserStateList.Clear();
			_MessageDict.Clear();
			Disconnected?.Invoke(this, InEx);
			await Task.Delay(1); // TEMP
		};

		_Connection.On(MainHubConstants.RECEIVE_THROTTLE, OnReceiveThrottle);
		_Connection.On<MainHubLocalUserModel>(MainHubConstants.RECEIVE_LOCAL_USER, OnReceiveLocalUser);
		_Connection.On<IEnumerable<MainHubMatchUserModel>>(MainHubConstants.RECEIVE_MATCH_USERS, OnReceiveMatchUsers);
		_Connection.On<ProfileCompactModel>(MainHubConstants.RECEIVE_USER_UPDATE, OnReceiveUserUpdate);
		_Connection.On<ChatHistorySendModel>(MainHubConstants.RECEIVE_CHAT_HISTORY, OnReceiveChatHistory);
		_Connection.On<MessageModel>(MainHubConstants.RECEIVE_CHAT_MESSAGE, OnReceiveChatMessage);
		_Connection.On<Int32>(MainHubConstants.RECEIVE_UNMATCH, OnReceiveUnmatch);

		try { await _Connection.StartAsync(); }
		catch { _NavigationService.NavigateTo(NavigationConstants.NAV_HOME); }
	}

	public async Task RequestChatHistoryAsync(Int32 InMatchId, Boolean InForward)
	{
		if (InMatchId < 0) throw new ArgumentOutOfRangeException(nameof(InMatchId));

		var MatchUserState = _MatchUserStateList.FirstOrDefault(X => X.User.MatchId == InMatchId);
		if (MatchUserState == null) return;

		if (!InForward && MatchUserState.IsLoadingPreviousMessages) return;
		if (InForward && MatchUserState.IsLoadingFollowingMessages) return;
		if (!InForward && MatchUserState.RemainingPreviousMessageCount <= 0) return;
		if (InForward && MatchUserState.RemainingFollowingMessageCount <= 0) return;

		if (!InForward) MatchUserState.IsLoadingPreviousMessages = true;
		else MatchUserState.IsLoadingFollowingMessages = true;

		ChatLoad?.Invoke(this, EventArgs.Empty);

		var MessageList = GetMessageListReadOnly(InMatchId);
		var StartingMessageId = MessageList.Count > 0 ? (InForward ? MessageList[MessageList.Count - 1].Id : MessageList[0].Id) : Int32.MaxValue;
		var MessageCount = !MatchUserState.IsChatSetup ? MainHubConstants.CHAT_SETUP_MESSAGE_COUNT : MainHubConstants.CHAT_LOAD_MESSAGE_COUNT;

		var RequestModel = new ChatGetHistoryRequestModel
		{
			MatchId = InMatchId,
			StartingMessageId = StartingMessageId,
			MessageCount = MessageCount,
			Forward = InForward,
		};

		await _Connection.SendAsync(MainHubConstants.REQUEST_CHAT_GET_HISTORY, RequestModel);
		MatchUserState.IsChatSetup = true;
	}
	public async Task SendMessageAsync(Int32 InMatchId, String InMessage)
	{
		if (InMatchId == -1) throw new ArgumentException(nameof(InMatchId));
		if (String.IsNullOrEmpty(InMessage)) throw new ArgumentException(nameof(InMessage));

		try
		{
			var ClientData = new MessageClientDataModel { Message = InMessage };
			var ClientDataJson = JsonSerializer.Serialize(ClientData);
			var CryptoBox = await GetCryptoBoxAsync(InMatchId);
			if (CryptoBox == null) return;

			var ClientDataJson_Nonce = new Byte[NaCl.Curve25519XSalsa20Poly1305.NonceLength];
			RandomStatics.RANDOM_CRYPTO.GetBytes(ClientDataJson_Nonce);

			var ClientDataJson_Encrypted = new Byte[ClientDataJson.Length + NaCl.Curve25519XSalsa20Poly1305.TagLength];
			CryptoBox.Encrypt(ClientDataJson_Encrypted, Encoding.UTF8.GetBytes(ClientDataJson), ClientDataJson_Nonce);

			var RequestModel = new ChatSendRequestModel
			{
				MatchId = InMatchId,
				ClientDataJson_Encrypted_Base64 = Convert.ToBase64String(ClientDataJson_Encrypted),
				ClientDataJson_Nonce_Base64 = Convert.ToBase64String(ClientDataJson_Nonce),
			};

			await _Connection.SendAsync(MainHubConstants.REQUEST_CHAT_SEND, RequestModel);
		}

		catch (Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
			_UnsentMessageList.Add(InMessage); // TODO: handle unsent messages
		}
	}

	public ReadOnlyCollection<DecryptedMessageModel> GetMessageListReadOnly(Int32 InMatchId)
	{
		if (InMatchId < 0) throw new ArgumentOutOfRangeException(nameof(InMatchId));

		return GetMessageList(InMatchId).AsReadOnly();
	}
	public void ResetUnreadMessageCount(MatchUserStateModel InMatchUserState)
	{
		if (InMatchUserState == null) throw new ArgumentNullException(nameof(InMatchUserState));

		InMatchUserState.UnreadMessageCount = 0;
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task OnReceiveChatHistory(ChatHistorySendModel InModel)
	{
		ChatLoad?.Invoke(this, EventArgs.Empty);

		var MatchUserState = _MatchUserStateList.FirstOrDefault(X => X.User.MatchId == InModel.MatchId);
		if (MatchUserState == null) return;

		if (!InModel.Forward) MatchUserState.IsLoadingPreviousMessages = false;
		else MatchUserState.IsLoadingFollowingMessages = false;

		if (!InModel.Forward) MatchUserState.RemainingPreviousMessageCount = InModel.RemainingMessageCount;
		else MatchUserState.RemainingFollowingMessageCount = InModel.RemainingMessageCount;

		foreach (var Message in InModel.Messages) await AddMessageAsync(Message);
		var MessageList = GetMessageList(InModel.MatchId);

		if (!InModel.Forward && InModel.RemainingMessageCount <= 0)
		{
			MessageList.Add(new DecryptedMessageModel
			{
				Id = -2,
				ClientData = new MessageClientDataModel { Message = _LocalizationService["MatchStartMessage"] },
			});

			MessageList.Add(new DecryptedMessageModel
			{
				Id = -1,
				ClientData = new MessageClientDataModel { Message = _LocalizationService["MatchChatSuggestion"] },
			});
		}

		MessageList.Sort((X1, X2) => X1.Id.CompareTo(X2.Id));

		ChatUpdate?.Invoke(this, EventArgs.Empty);
	}
	private async Task OnReceiveChatMessage(MessageModel InMessage)
	{
		await AddMessageAsync(InMessage);
		HandleMessageNotifications(InMessage);
		ChatUpdate?.Invoke(this, EventArgs.Empty);
	}

	private async Task<NaCl.Curve25519XSalsa20Poly1305> GetCryptoBoxAsync(Int32 InMatchId)
	{
		var MatchUserState = _MatchUserStateList.FirstOrDefault(X => X.User.MatchId == InMatchId);
		return await GetCryptoBoxAsync(MatchUserState.User);
	}
	private async Task<NaCl.Curve25519XSalsa20Poly1305> GetCryptoBoxAsync(MainHubMatchUserModel InMatchUser)
	{
		if (InMatchUser == null) return null;

		var UserPrivateKey_Base64 = await _AppState.GetPrivateKeyAsync();
		if (String.IsNullOrEmpty(UserPrivateKey_Base64)) return null;

		try
		{
			var UserPrivateKey = Convert.FromBase64String(UserPrivateKey_Base64);
			return new NaCl.Curve25519XSalsa20Poly1305(UserPrivateKey, FirstMatchUser.PublicKey);
		}

		catch (Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
			return null;
		}
	}
	private async Task AddMessageAsync(MessageModel InMessage)
	{
		if (InMessage == null) return;
		if (String.IsNullOrEmpty(InMessage.ServerDataJson) || String.IsNullOrEmpty(InMessage.ClientDataJson_Encrypted_Base64)) return;

		try
		{
			var ServerData = JsonSerializer.Deserialize<MessageServerDataModel>(InMessage.ServerDataJson);
			var ClientData = new MessageClientDataModel();

			var ClientDataJson_Encrypted = Convert.FromBase64String(InMessage.ClientDataJson_Encrypted_Base64);
			var ClientDataJson_Bytes = new Byte[ClientDataJson_Encrypted.Length - NaCl.Curve25519XSalsa20Poly1305.TagLength];

			var CryptoBox = await GetCryptoBoxAsync(FirstMatchUser);
			if (CryptoBox != null && CryptoBox.TryDecrypt(ClientDataJson_Bytes, ClientDataJson_Encrypted, Convert.FromBase64String(InMessage.ClientDataJson_Nonce_Base64)))
				ClientData = JsonSerializer.Deserialize<MessageClientDataModel>(Encoding.UTF8.GetString(ClientDataJson_Bytes));

			if (ClientData.Message != null)
			{
				var EncodedMessage = HttpUtility.HtmlEncode(ClientData.Message);
				var EncodedMessageWithUrls = URL_REGEX.Replace(EncodedMessage, URL_REPLACE);
				ClientData.Message = EncodedMessageWithUrls;
			}

			var Sender = InMessage.FromPublicId == LocalUser.CompactProfile.PublicId ? LocalUser.CompactProfile : FirstMatchUser.CompactProfile;
			var DecryptedMessage = new DecryptedMessageModel { Id = InMessage.Id, Sender = Sender, ServerData = ServerData, ClientData = ClientData };
			var MessageList = GetMessageList(InMessage.MatchId);
			MessageList.Add(DecryptedMessage);
		}

		catch (Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
		}
	}

	private void OnReceiveThrottle()
	{
		_NotificationService.SetMainNotification(_LocalizationService[ErrorConstants.ERROR_TOO_MANY_REQUESTS], ENotificationType.Danger);
	}
	private void OnReceiveLocalUser(MainHubLocalUserModel InLocalUser)
	{
		LocalUser = InLocalUser;
		LocalUserUpdate?.Invoke(this, EventArgs.Empty);
		OnReceiveUserUpdate(LocalUser.CompactProfile);
	}
	private async Task OnReceiveMatchUsers(IEnumerable<MainHubMatchUserModel> InMatchUsers)
	{
		if (InMatchUsers == null) return;

		var LastCount = _MatchUserStateList.Count;
		foreach (var MatchUser in InMatchUsers)
		{
			var NewMatchUserState = new MatchUserStateModel
			{
				User = MatchUser,
				RemainingPreviousMessageCount = Int32.MaxValue,
				RemainingFollowingMessageCount = Int32.MaxValue,
			};

			_MatchUserStateList.Add(NewMatchUserState);
			OnReceiveUserUpdate(MatchUser.CompactProfile);
		}

		if (_MatchUserStateList.Count > LastCount)
		{
			if (IsOnMatchPage())
			{
				foreach (var MatchUserState in _MatchUserStateList)
					await RequestChatHistoryAsync(MatchUserState.User.MatchId, false);
			}

			RefreshMatchPage();
		}
	}
	private void OnReceiveUserUpdate(ProfileCompactModel InCompactProfile)
	{
		if (InCompactProfile == null) return;
		if (LocalUser == null) return;

		foreach (var Pair in _MessageDict)
		{
			var MessageList = Pair.Value;
			for (var Index = 0; Index < MessageList.Count; Index++)
			{
				var Message = MessageList[Index];
				if (Message.Sender != null && Message.Sender.PublicId == InCompactProfile.PublicId)
				{
					MessageList.RemoveAt(Index);
					MessageList.Insert(Index, new DecryptedMessageModel { Id = Message.Id, Sender = InCompactProfile, ServerData = Message.ServerData, ClientData = Message.ClientData });
				}
			}
		}

		if (InCompactProfile.PublicId == LocalUser.CompactProfile.PublicId)
		{
			LocalUser = new MainHubLocalUserModel
			{
				CompactProfile = InCompactProfile,
				Roles = LocalUser.Roles,
			};

			LocalUserUpdate?.Invoke(this, EventArgs.Empty);
		}

		else
		{
			var MatchUserStateIndex = _MatchUserStateList.FindIndex(X => X.User.CompactProfile.PublicId == InCompactProfile.PublicId);
			if (MatchUserStateIndex != -1)
			{
				var MatchUserState = _MatchUserStateList[MatchUserStateIndex];
				_MatchUserStateList.RemoveAt(MatchUserStateIndex);

				_MatchUserStateList.Add(new MatchUserStateModel
				{
					User = new MainHubMatchUserModel
					{
						CompactProfile = InCompactProfile,
						PublicKey = MatchUserState.User.PublicKey,
						MatchId = MatchUserState.User.MatchId,
					},

					RemainingPreviousMessageCount = MatchUserState.RemainingPreviousMessageCount,
					RemainingFollowingMessageCount = MatchUserState.RemainingFollowingMessageCount,
					IsChatSetup = MatchUserState.IsChatSetup,
				});
			}
		}

		UserUpdate?.Invoke(this, EventArgs.Empty);
	}
	private void OnReceiveUnmatch(Int32 InMatchId)
	{
		if (_MessageDict.ContainsKey(InMatchId)) _MessageDict.Remove(InMatchId);
		_MatchUserStateList = _MatchUserStateList.Where(X => X.User.MatchId != InMatchId).ToList();
		RefreshMatchPage();
	}

	private void RefreshMatchPage()
	{
		if (IsOnMatchPage()) _NavigationService.NavigateTo(NavigationConstants.NAV_MATCH);
	}
	private void ResetConnection()
	{
		LocalUser = null;

		_Connection?.DisposeAsync();
		_Connection = null;

		_MatchUserStateList.Clear();
		_MessageDict.Clear();
		_UnsentMessageList.Clear();
	}
	private void HandleMessageNotifications(MessageModel InMessage)
	{
		if (InMessage.FromPublicId == LocalUser.CompactProfile.PublicId) return;

		if (!IsOnMatchPage())
		{
			var Sender = _MatchUserStateList.FirstOrDefault(X => X.User.CompactProfile.PublicId == InMessage.FromPublicId);
			Sender.UnreadMessageCount++;
		}

		try { _JS.InvokeVoidAsync(JSHelperConstants.PLAY_AUDIO, IdConstants.CHAT_AUDIO_ID); }
		catch (JSException) { }
	}

	private List<DecryptedMessageModel> GetMessageList(Int32 InMatchId)
	{
		if (!_MessageDict.TryGetValue(InMatchId, out var List)) _MessageDict.Add(InMatchId, List = new ());
		return List;
	}
	private Boolean IsOnMatchPage() => $"/{_NavigationService.ToBaseRelativePath(_NavigationService.Uri)}".StartsWith(NavigationConstants.NAV_MATCH);
}