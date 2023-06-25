namespace SimplyMeetShared.Constants;

public static class MainHubConstants
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Const Fields
	public const String PATH = "hub/main";

	public const String RECEIVE_THROTTLE = nameof(RECEIVE_THROTTLE);

	public const String RECEIVE_LOCAL_USER = nameof(RECEIVE_LOCAL_USER);
	public const String RECEIVE_MATCH_USERS = nameof(RECEIVE_MATCH_USERS);
	public const String RECEIVE_USER_UPDATE = nameof(RECEIVE_USER_UPDATE);
	public const String RECEIVE_CHAT_HISTORY = nameof(RECEIVE_CHAT_HISTORY);
	public const String RECEIVE_CHAT_MESSAGE = nameof(RECEIVE_CHAT_MESSAGE);
	public const String RECEIVE_UNMATCH = nameof(RECEIVE_UNMATCH);

	public const String REQUEST_CHAT_GET_HISTORY = nameof(REQUEST_CHAT_GET_HISTORY);
	public const String REQUEST_CHAT_SEND = nameof(REQUEST_CHAT_SEND);

	public const Int32 CHAT_MAX_LENGTH = 1024 * 64;
	public const Int32 CHAT_MAX_MESSAGE_LENGTH = (CHAT_MAX_LENGTH / 4) - 1024; // TODO: calculate actual max length for client
	public const Int32 CHAT_MAX_LOAD_MESSAGE_COUNT = 100;
	public const Int32 CHAT_SETUP_MESSAGE_COUNT = 30;
	public const Int32 CHAT_LOAD_MESSAGE_COUNT = 10;
	#endregion
}