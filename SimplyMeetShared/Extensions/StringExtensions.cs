using System;

namespace SimplyMeetShared.Extensions
{
	public static class StringExtensions
	{
		//===========================================================================================
		// Public Static Methods
		//===========================================================================================
		public static String Truncate(this String InText, Int32 InMaxLength)
		{
			if (InMaxLength < 0) throw new ArgumentOutOfRangeException(nameof(InMaxLength));
			if (String.IsNullOrEmpty(InText)) return InText;

			return InText.Substring(0, Math.Min(InText.Length, InMaxLength));
		}
	}
}