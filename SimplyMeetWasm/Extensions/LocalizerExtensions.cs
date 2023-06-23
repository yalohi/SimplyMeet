namespace SimplyMeetShared.Extensions;

public static class LocalizerExtensions
{
	//===========================================================================================
	// Public Static Methods
	//===========================================================================================
	public static String GetIconText(this IStringLocalizer<SharedResource> InLoc, ProfileDataIconModelBase InModel)
	{
		return $"{(String.IsNullOrEmpty(InModel.Icon) ? String.Empty : $"{InModel.Icon} ")}{InLoc[InModel.Name]}";
	}
}