namespace SimplyMeetApi.Services;

public class AccountService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly DatabaseService _DatabaseService;
	private readonly MainHubService _MainHubService;
	private readonly TokenService _TokenService;

	// TEMP
	private readonly ProfileService _ProfileService;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public AccountService(DatabaseService InDatabaseService, MainHubService InMainHubService, TokenService InTokenService, ProfileService InProfileService)
	{
		_DatabaseService = InDatabaseService;
		_MainHubService = InMainHubService;
		_TokenService = InTokenService;

		_ProfileService = InProfileService;
	}

	public async Task<AccountGetChallengeResponseModel> GetChallengeAsync(ServiceModel<AccountGetChallengeRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		await Task.Delay(1); // TEMP
		return _TokenService.CreateChallenge(InModel.Request.UserPublicKey);
	}
	public async Task<AccountLoginResponseModel> LoginAsync(ServiceModel<AccountLoginRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			var Token = await _TokenService.AuthenticateAsync(InModel.Request.UserPublicKey, InModel.Request.SolvedChallenge, InConnection);
			if (Token == null) return new AccountLoginResponseModel { Error = ErrorConstants.ERROR_NO_SUCH_ACCOUNT };

			return new AccountLoginResponseModel { Token = Token };
		});
	}
	public async Task<AccountGetChallengeResponseModel> CreateAsync(ServiceModel<AccountCreateRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		if (InModel.Request.UserPublicKey == null) return new AccountGetChallengeResponseModel { Error = ErrorConstants.ERROR_INVALID_OPERATION };

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			// await CreateDummyAccounts(InConnection);

			var UserPublicKey_Base64 = Convert.ToBase64String(InModel.Request.UserPublicKey);
			var NewAccount = await GenerateNewAccountAsync(UserPublicKey_Base64, InConnection);
			if (NewAccount == null) return new AccountGetChallengeResponseModel { Error = ErrorConstants.ERROR_GENERATE_FAILED };
			if ((NewAccount.Id = await _DatabaseService.InsertModelReturnIdAsync(NewAccount, InConnection)) <= 0) return new AccountGetChallengeResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			var NewProfile = new ProfileModel
			{
				Avatar = ProfileConstants.DEFAULT_AVATAR,
				DisplayName = ProfileConstants.DEFAULT_DISPLAY_NAME,
			};

			var NewFilter = new FilterModel
			{
			};

			if ((NewProfile.Id = await _DatabaseService.InsertModelReturnIdAsync(NewProfile, InConnection)) <= 0) return new AccountGetChallengeResponseModel { Error = ErrorConstants.ERROR_DATABASE };
			if ((NewFilter.Id = await _DatabaseService.InsertModelReturnIdAsync(NewFilter, InConnection)) <= 0) return new AccountGetChallengeResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			NewAccount.ProfileId = NewProfile.Id;
			NewAccount.FilterId = NewFilter.Id;
			if (await _DatabaseService.UpdateModelByIdAsync(NewAccount, InConnection) <= 0) return new AccountGetChallengeResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			var Model = new ServiceModel<AccountGetChallengeRequestModel>
			{
				Auth = InModel.Auth,
				Request = new AccountGetChallengeRequestModel { UserPublicKey = Convert.FromBase64String(NewAccount.PublicKey_Base64) }
			};

			return await GetChallengeAsync(Model);
		});
	}
	public async Task<AccountDeleteResponseModel> DeleteAsync(ServiceModel<AccountDeleteRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
			if (Account == null) return new AccountDeleteResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			var Matches = await _DatabaseService.GetAllMatchesAsync(Account, InConnection);
			foreach (var Match in Matches)
			{
				var MatchedAccountId = Match.AccountId == Account.Id ? Match.MatchAccountId : Match.AccountId;
				await _MainHubService.OnUnmatchAsync(MatchedAccountId, Match.Id);
			}

			if (await _DatabaseService.DeleteAccountAsync(Account, InConnection) <= 0) return new AccountDeleteResponseModel { Error = ErrorConstants.ERROR_DATABASE };
			return new AccountDeleteResponseModel();
		});
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task<AccountModel> GenerateNewAccountAsync(String InPublicKey, IDbConnection InConnection)
	{
		var PublicId = await AttemptGenerateAsync(GeneratePublicId, _DatabaseService.CheckAccountPublicIdAsync, InConnection);
		if (InPublicKey == null || PublicId == null) return null;

		return new AccountModel
		{
			PublicKey_Base64 = InPublicKey,
			PublicId = PublicId,
			Creation = DateTime.UtcNow,
		};
	}
	private async Task<String> AttemptGenerateAsync(Func<String> InGenerateFunc, Func<String, IDbConnection, Task<Boolean>> InCheckFunc, IDbConnection InConnection)
	{
		var AttemptCounter = 0;
		var IsInvalid = true;
		var GeneratedValue = InGenerateFunc();

		while ((IsInvalid = await InCheckFunc(GeneratedValue, InConnection)) && ++AttemptCounter < AccountConstants.MAX_GENERATE_ACCOUNT_ATTEMPTS) GeneratedValue = InGenerateFunc();
		return IsInvalid ? null : GeneratedValue;
	}

	private String GeneratePublicId()
	{
		var PublicIdBuilder = new StringBuilder();
		for (var Index = 0; Index < AccountConstants.PUBLIC_ID_LENGTH; Index++) PublicIdBuilder.Append(AccountConstants.PUBLIC_ID_CHARSET[RandomStatics.RANDOM.Next(AccountConstants.PUBLIC_ID_CHARSET.Length)]);
		return PublicIdBuilder.ToString();
	}

	// TEMP
	private String GenerateDummyPublicKey()
	{
		var PublicKeyBuilder = new StringBuilder();
		for (var Index = 0; Index < AccountConstants.PUBLIC_KEY_TEXT_LENGTH; Index++) PublicKeyBuilder.Append(AccountConstants.PUBLIC_ID_CHARSET[RandomStatics.RANDOM.Next(AccountConstants.PUBLIC_ID_CHARSET.Length)]);
		return PublicKeyBuilder.ToString();
	}
	private async Task CreateDummyAccounts(IDbConnection InConnection)
	{
		const Int32 DUMMY_ACCOUNT_COUNT = 100000;
		var ProfileData = await _ProfileService.GetProfileDataAsync(InConnection);

		for (var Index = 0; Index < DUMMY_ACCOUNT_COUNT; Index++)
		{
			var PublicKey = await AttemptGenerateAsync(GenerateDummyPublicKey, _DatabaseService.CheckAccountPublicKeyAsync, InConnection);
			var GenAccount = await GenerateNewAccountAsync(PublicKey, InConnection);
			if (GenAccount == null) return;
			if ((GenAccount.Id = await _DatabaseService.InsertModelReturnIdAsync(GenAccount, InConnection)) <= 0) return;

			var PronounsId = ProfileData.AllPronouns.FirstOrDefault(X => X.Id == RandomStatics.RANDOM.Next(0, ProfileData.AllPronouns.Count() + 1))?.Id;
			var SexId = ProfileData.AllSexes.FirstOrDefault(X => X.Id == RandomStatics.RANDOM.Next(0, ProfileData.AllSexes.Count() + 1))?.Id;
			var GenderId = ProfileData.AllGenders.FirstOrDefault(X => X.Id == RandomStatics.RANDOM.Next(0, ProfileData.AllGenders.Count() + 1))?.Id;
			var RegionId = ProfileData.AllRegions.FirstOrDefault(X => X.Id == RandomStatics.RANDOM.Next(0, ProfileData.AllRegions.Count() + 1))?.Id;

			var Countries = ProfileData.AllCountries.Where(X => X.RegionId == RegionId);
			var CountryId = RegionId != null ? Countries.ElementAtOrDefault(RandomStatics.RANDOM.Next(0, Countries.Count()))?.Id : null;

			var BirthdayTicks = DateTime.UtcNow.ToBinary();
			var Age = RandomStatics.RANDOM.Next(ProfileConstants.MIN_AGE, ProfileConstants.MAX_AGE);
			BirthdayTicks -= TimeSpan.FromDays(365 * Age).Ticks;

			var GenProfile = new ProfileModel
			{
				Avatar = ProfileConstants.DEFAULT_AVATAR,
				DisplayName = GenAccount.PublicId,
				PronounsId = PronounsId,
				SexId = SexId,
				GenderId = GenderId,
				RegionId = RegionId,
				CountryId = CountryId,
				BirthDate = DateTime.FromBinary(BirthdayTicks)
			};

			var GenFilter = new FilterModel
			{
				// PronounsId = PronounsId,
				// SexId = SexId,
				// GenderId = GenderId,
				// RegionId = RegionId,
				// CountryId = CountryId,
				// FromAge = Math.Max(Age - 5, ProfileConstants.MIN_AGE),
				// ToAge = Math.Min(Age + 5, ProfileConstants.MAX_AGE),
				// AgeEnabled = true,
			};

			if ((GenProfile.Id = await _DatabaseService.InsertModelReturnIdAsync(GenProfile, InConnection)) <= 0) return;
			if ((GenFilter.Id = await _DatabaseService.InsertModelReturnIdAsync(GenFilter, InConnection)) <= 0) return;

			GenAccount.ProfileId = GenProfile.Id;
			GenAccount.FilterId = GenFilter.Id;
			if (await _DatabaseService.UpdateModelByIdAsync(GenAccount, InConnection) <= 0) return;
		}
	}
}