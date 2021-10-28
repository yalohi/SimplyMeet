using System;
using Microsoft.Extensions.Localization;
using SimplyMeetShared.Models;
using SimplyMeetWasm.Resources;

namespace SimplyMeetShared.Extensions
{
	public static class LocalizerExtensions
	{
		//===========================================================================================
		// Public Static Methods
		//===========================================================================================
		public static String GetIconText(this IStringLocalizer<SharedResource> InLoc, ProfileDataIconBaseModel InModel)
		{
			return $"{(String.IsNullOrEmpty(InModel.Icon) ? String.Empty : $"{InModel.Icon} ")}{InLoc[InModel.Name]}";
		}
	}
}