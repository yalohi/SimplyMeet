using System;

namespace SimplyMeetShared.Constants
{
	public static class ProfileConstants
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Const Fields
		public const String DEFAULT_AVATAR = "Default.webp";
		public const String AVATAR_EXTENSION = "webp";
		public const Int32 AVATAR_HASH_LENGTH = 10;
		public const Int32 AVATAR_QUALITY = 50;
		public const Int32 MAX_AVATAR_SIZE = 1024 * 1024 * 1;

		public const String DEFAULT_DISPLAY_NAME = "Anonymous";
		public const Int32 MAX_DISPLAY_NAME_LENGTH = 32;

		public const Single DAYS_PER_YEAR = 365.25f;
		public const Int32 MIN_AGE = 18;
		public const Int32 MAX_AGE = 100;

		public const Int32 MAX_TAG_NAME_LENGTH = 32;
		public const Int32 MAX_TAGS = 20;

		public const Int32 MAX_SEXUALITIES = 10;

		public const Int32 MAX_ABOUT_LENGTH = 1000;
		#endregion
	}
}