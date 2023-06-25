namespace SimplyMeetShared.RequestModels;

public record AdminEditProfileDataRequestModel : RequestModelBase
{
	[Required]
	public M_ProfileDataModel Data { get; set; }
}