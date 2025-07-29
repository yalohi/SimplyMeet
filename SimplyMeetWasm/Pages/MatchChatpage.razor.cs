namespace SimplyMeetWasm.Pages;

public partial class MatchChatPage
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Const Fields
	private const String CHAT_ID = "chat";
	#endregion
	#region Properties
	public String Message
	{
		get { return _Message; }
		set
		{
			if (_Message == value) return;

			_Message = value;
			OnChatInput();
		}
	}
	#endregion
	#region Fields
	// private ProfileCompactComponent _CompactProfileComponent;

	private List<MessageBlockModel> _MessageBlockList;
	private String _Message;
	private String _YourPublicKey;
	private String _TheirPublicKey;
	private Boolean _NotificationsPermitted;
	private Boolean _ShowProfile;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public void Dispose()
	{
		MainHubService.UserUpdate -= OnUserUpdate;
		MainHubService.ChatUpdate -= OnChatUpdate;
		MainHubService.ChatLoad -= OnChatLoad;
	}

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnSetupAsync()
	{
		if (MainHubService.FirstMatchUser == null)
		{
			NavigationService.NavigateTo(NavigationConstants.NAV_MATCH);
			return;
		}

		await base.OnSetupAsync();
		AppState.ShowNavBar = false;

		_YourPublicKey = Convert.ToBase64String(await AccountService.GetLocalPublicKeyAsync());
		_TheirPublicKey = Convert.ToBase64String(MainHubService.FirstMatchUser.PublicKey);

		try { _NotificationsPermitted = await JS.InvokeAsync<Boolean>(JSHelperConstants.REQUEST_NOTIFICATION_PERMISSION); }
		catch (JSException Ex) { Console.WriteLine(Ex.ToString()); }

		MainHubService.UserUpdate += OnUserUpdate;
		MainHubService.ChatUpdate += OnChatUpdate;
		MainHubService.ChatLoad += OnChatLoad;

		await MainHubService.RequestChatHistoryAsync(MainHubService.FirstMatchUser.MatchId, false);
		await UpdateMessageBlockListAsync();

		try { await JS.InvokeVoidAsync(JSHelperConstants.SCROLL_ELEMENT_TO_BOTTOM, CHAT_ID); }
		catch (JSException) { }

		MainHubService.ResetUnreadMessageCount(MainHubService.FirstMatchUserState);
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async void OnProfileClick(ProfileCompactComponent InCompactProfile)
	{
		_ShowProfile = !_ShowProfile;
		if (_ShowProfile) return;

		StateHasChanged();
		await Task.Delay(1); // HACK
		OnChatInput();
	}
	private async Task OnSendClick()
	{
		if (MainHubService.FirstMatchUser != null && !String.IsNullOrEmpty(_Message))
		{
			await MainHubService.SendMessageAsync(MainHubService.FirstMatchUser.MatchId, _Message);
			_Message = String.Empty;
			StateHasChanged();
			OnChatInput();
		}
	}
	private async Task OnChatScroll()
	{
		try
		{
			var ScrollTop = await JS.InvokeAsync<Int32>(JSHelperConstants.GET_ELEMENT_SCROLL_TOP, CHAT_ID);
			if (ScrollTop <= 0) await MainHubService.RequestChatHistoryAsync(MainHubService.FirstMatchUser.MatchId, false);
		}

		catch (JSException) { }
	}
	private async Task OnMessageKeyDown(KeyboardEventArgs InArgs)
	{
		if (InArgs.CtrlKey && (InArgs.Code == KeyCodeConstants.ENTER || InArgs.Code == KeyCodeConstants.NUMPAD_ENTER))
			await OnSendClick();
	}

	private void OnNavBarClick()
	{
		AppState.ShowNavBar = !AppState.ShowNavBar;
	}
	private async void OnUserUpdate(Object InSender, EventArgs InArgs)
	{
		await UpdateMessageBlockListAsync();
		StateHasChanged();
	}
	private async void OnChatInput()
	{
		await JS.InvokeVoidAsync(JSHelperConstants.UPDATE_GROWING_TEXT_AREA, IdConstants.SEND_MESSAGE_ID);
	}
	private async void OnChatUpdate(Object InSender, EventArgs InArgs)
	{
		var LastScrollTop = 0;
		var LastScrollMax = 0;

		try
		{
			LastScrollTop = await JS.InvokeAsync<Int32>(JSHelperConstants.GET_ELEMENT_SCROLL_TOP, CHAT_ID);
			LastScrollMax = await JS.InvokeAsync<Int32>(JSHelperConstants.GET_ELEMENT_SCROLL_MAX, CHAT_ID);
		}

		catch (JSException) { }

		var PreviousFirstMessageId = GetFirstMessageId();

		await UpdateMessageBlockListAsync();
		if (!_ShowProfile) StateHasChanged();

		var FirstMessageId = GetFirstMessageId();
		var IsHistory = FirstMessageId != PreviousFirstMessageId;

		try
		{
			if (LastScrollTop < LastScrollMax)
			{
				var ScrollMax = await JS.InvokeAsync<Int32>(JSHelperConstants.GET_ELEMENT_SCROLL_MAX, CHAT_ID);
				var NewScrollTop = IsHistory ? LastScrollTop + (ScrollMax - LastScrollMax) : LastScrollTop;
				await JS.InvokeVoidAsync(JSHelperConstants.SCROLL_ELEMENT_TO, CHAT_ID, NewScrollTop);
			}

			else await JS.InvokeVoidAsync(JSHelperConstants.SCROLL_ELEMENT_TO_BOTTOM, CHAT_ID);
		}

		catch (JSException) { }
	}
	private void OnChatLoad(Object InSender, EventArgs InArgs)
	{
		StateHasChanged();
	}

	private Int32 GetFirstMessageId()
	{
		return _MessageBlockList != null && _MessageBlockList.Count > 0 ? _MessageBlockList[0].MessageList[0].Id : -1;
	}

	private async Task UpdateMessageBlockListAsync()
	{
		_MessageBlockList = new List<MessageBlockModel>();
		var MessageList = MainHubService.GetMessageListReadOnly(MainHubService.FirstMatchUser.MatchId);
		if (MessageList == null) return;

		DateTime? LastLocalDateTime = null;
		ProfileCompactModel LastSender = null;
		var TempMessageList = new List<DecryptedMessageModel>();
		var IsBlockNewDate = false;

		foreach (var Message in MessageList)
		{
			var LocalDateTime = Message.ServerData?.DateTime.ToLocalTime();
			var IsNewDate = LastLocalDateTime == null || LocalDateTime == null || LastLocalDateTime.Value.Date != LocalDateTime.Value.Date;
			var IsNewBlock = IsNewDate || Message.Sender != LastSender;

			if (IsNewBlock && TempMessageList.Count > 0)
			{
				await AddMessageBlockAsync(TempMessageList, IsBlockNewDate);
				TempMessageList = new List<DecryptedMessageModel>();
				IsBlockNewDate = false;
			}

			if (IsNewDate) IsBlockNewDate = true;
			TempMessageList.Add(Message);

			LastLocalDateTime = LocalDateTime;
			LastSender = Message.Sender;
		}

		if (TempMessageList.Count > 0) await AddMessageBlockAsync(TempMessageList, IsBlockNewDate);
		if (!_ShowProfile) StateHasChanged();
	}
	private async Task AddMessageBlockAsync(List<DecryptedMessageModel> InMessageList, Boolean InIsBlockNewDate)
	{
		var FirstMessage = InMessageList[0];
		var AvatarUri = FirstMessage.Sender != null ? (await ProfileService.GetAvatarUriAsync(FirstMessage.Sender.Avatar)).ToString() : String.Empty;
		_MessageBlockList.Add(new MessageBlockModel(InMessageList, AvatarUri, InIsBlockNewDate));
	}
}
