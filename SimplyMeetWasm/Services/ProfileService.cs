namespace SimplyMeetWasm.Services;

public class ProfileService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public ProfileService()
	{
	}

	public String GetLookingForIconClasses(ELookingFor InLookingFor)
	{
		if (InLookingFor.HasFlag(ELookingFor.Conversation)) return "fas fa-comments text-warning";
		if (InLookingFor.HasFlag(ELookingFor.Friendship)) return "fas fa-user-friends text-info";
		if (InLookingFor.HasFlag(ELookingFor.Love)) return "fas fa-heartbeat text-danger";
		return String.Empty;
	}
	public String GetAvatarUrl(String InAvatar)
	{
		if (InAvatar == null) throw new ArgumentNullException(nameof(InAvatar));

		return $"{ApiRequestConstants.BASE_PATH}{ApiRequestConstants.AVATARS}/{InAvatar}";
	}
}