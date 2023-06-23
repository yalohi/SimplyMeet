namespace SimplyMeetShared.RequestModels;

public record AdminEditAccountRolesRequestModel : RequestModelBase
{
	[Required]
	public IEnumerable<Int32> RemovedIds { get; set; }

	[Required]
	public IEnumerable<AccountRoleModel> NewAccountRoles { get; set; }
}