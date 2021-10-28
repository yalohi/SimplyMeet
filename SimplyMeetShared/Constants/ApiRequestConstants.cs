using System;

namespace SimplyMeetShared.Constants
{
	public static class ApiRequestConstants
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Const Fields
		public const String BASE_ADDRESS = "https://127.0.0.1/api/";
		public const String BASE_PATH = "/api/";

		public const String AVATARS = "Avatars";

		public const String HOME_GET_DATA = "Home/GetData";

		public const String ACCOUNT_CREATE = "Account/Create";
		public const String ACCOUNT_DELETE = "Account/Delete";
		public const String ACCOUNT_GET_CHALLENGE = "Account/GetChallenge";
		public const String ACCOUNT_LOGIN = "Account/Login";

		public const String PROFILE_GET = "Profile/Get";
		public const String PROFILE_GET_COMPACT = "Profile/GetCompact";
		public const String PROFILE_GET_EDIT_DATA = "Profile/GetEditData";
		public const String PROFILE_EDIT = "Profile/Edit";
		public const String PROFILE_EDIT_AVATAR = "Profile/EditAvatar";
		public const String PROFILE_RESET_AVATAR = "Profile/ResetAvatar";
		public const String PROFILE_REPORT = "Profile/Report";

		public const String MATCH_GET_NEW = "Match/GetNew";
		public const String MATCH_GET_CHOICES = "Match/GetChoices";
		public const String MATCH_GET_FILTER = "Match/GetFilter";
		public const String MATCH_EDIT_FILTER = "Match/EditFilter";
		public const String MATCH_CHOOSE = "Match/Choose";
		public const String MATCH_UNMATCH = "Match/Unmatch";

		public const String ADMIN_GET_REPORTED_PROFILES = "Admin/GetReportedProfiles";
		public const String ADMIN_GET_PROFILE_DATA = "Admin/GetProfileData";
		public const String ADMIN_GET_ACCOUNT_ROLES = "Admin/GetAccountRoles";
		public const String ADMIN_SUSPEND_ACCOUNT = "Admin/SuspendAccount";
		public const String ADMIN_EDIT_PROFILE_DATA = "Admin/EditProfileData";
		public const String ADMIN_EDIT_ACCOUNT_ROLES = "Admin/EditAccountRoles";

		public const Int32 MATCH_GET_CHOICES_LOAD_COUNT = 50;
		public const Int32 MATCH_GET_CHOICES_MAX_LOAD_COUNT = 100;

		public const Int32 ADMIN_GET_REPORTED_PROFILES_LOAD_COUNT = 50;
		public const Int32 ADMIN_GET_REPORTED_PROFILES_MAX_LOAD_COUNT = 100;
		#endregion
	}
}