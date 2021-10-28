using System;

namespace SimplyMeetShared.Extensions
{
	public static class DateTimeExtensions
	{
		//===========================================================================================
		// Public Static Methods
		//===========================================================================================
		public static Int32 GetAge(this DateTime InBirthDate)
		{
			var Today = DateTime.UtcNow;
			return (Today.Year - InBirthDate.Year - 1) + (((Today.Month > InBirthDate.Month) || (Today.Month == InBirthDate.Month && Today.Day >= InBirthDate.Day)) ? 1 : 0);
		}
	}
}