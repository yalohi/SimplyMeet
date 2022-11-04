namespace SimplyMeetShared.Constants;

public static class AccountConstants
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Const Fields
	public const String CHALLENGE_CHARSET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
	public const Int32 CHALLENGE_TIMEOUT_SECONDS = 60;

	public const String PUBLIC_ID_CHARSET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

	public const Int32 MAX_GENERATE_ACCOUNT_ATTEMPTS = 100;
	public const Int32 SOLVED_CHALLENGE_LENGTH = 32;
	public const Int32 PUBLIC_KEY_LENGTH = 32;
	public const Int32 PUBLIC_KEY_TEXT_LENGTH = 44;
	public const Int32 PRIVATE_KEY_TEXT_LENGTH = 44;
	public const Int32 PUBLIC_ID_LENGTH = 10;
	public const Int32 FLAG_NEW_DAYS = 7;
	public const Int32 FLAG_ACTIVE_DAYS = 7;
	#endregion
}