namespace SimplyMeetApi.Constants;

public static class JwtConstants
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Const Fields
	public const String JWT_SECRET = nameof(JWT_SECRET);
	public const String GEN_JWT_CHOICES = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
	public const Int32 MIN_JWT_SECRET_LENGTH = 32;
	#endregion
}
