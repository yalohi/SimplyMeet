namespace SimplyMeetWasm.Services;

public class ProfileService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly SettingsService _SettingsService;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public ProfileService(SettingsService InSettingsService)
	{
		_SettingsService = InSettingsService;
	}

	public String GetLookingForIconClasses(ELookingFor InLookingFor)
	{
		if (InLookingFor.HasFlag(ELookingFor.Conversation)) return "fas fa-comments text-warning";
		if (InLookingFor.HasFlag(ELookingFor.Friendship)) return "fas fa-user-friends text-info";
		if (InLookingFor.HasFlag(ELookingFor.Love)) return "fas fa-heartbeat text-danger";
		return String.Empty;
	}

	public async Task<Uri> GetAvatarUriAsync(String InAvatar)
	{
		if (InAvatar == null) throw new ArgumentNullException(nameof(InAvatar));

		return new Uri(await _SettingsService.GetApiServerAsync(), $"{ApiRequestConstants.AVATARS}/{InAvatar}");
	}
}
