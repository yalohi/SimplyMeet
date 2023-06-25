namespace SimplyMeetApi.Services;

public class ProfileCompactService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly DatabaseService _DatabaseService;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public ProfileCompactService(DatabaseService InDatabaseService)
	{
		_DatabaseService = InDatabaseService;
	}

	public async Task<ProfileCompactModel> GetAsync(Int32 InAccountId, EMatchChoice InChoice, IDbConnection InConnection)
	{
		if (InAccountId < 0) throw new ArgumentOutOfRangeException(nameof(InAccountId));
		if (InConnection == null) throw new ArgumentNullException(nameof(InConnection));

		var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InAccountId }, InConnection);
		return Account != null ? await GetFromAccountAsync(Account, InChoice, InConnection) : null;
	}
	public async Task<ProfileCompactModel> GetFromAccountAsync(AccountModel InAccount, EMatchChoice InChoice, IDbConnection InConnection)
	{
		if (InAccount == null) throw new ArgumentNullException(nameof(InAccount));
		if (InConnection == null) throw new ArgumentNullException(nameof(InConnection));

		var Profile = new ProfileModel { Id = InAccount.ProfileId };
		Profile = await _DatabaseService.GetModelByIdAsync(Profile, InConnection);
		if (Profile == null) return null;

		return new ProfileCompactModel
		{
			Avatar = Profile.Avatar,
			DisplayName = Profile.DisplayName,
			PublicId = InAccount.PublicId,
			Choice = InChoice,
		};
	}
}