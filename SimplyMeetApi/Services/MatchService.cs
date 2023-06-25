namespace SimplyMeetApi.Services;

public class MatchService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly DatabaseService _DatabaseService;
	private readonly MainHubService _MainHubService;
	private readonly ProfileService _ProfileService;
	private readonly ProfileCompactService _ProfileCompactService;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public MatchService(DatabaseService InDatabaseService, MainHubService InMainHubService, ProfileService InProfileService, ProfileCompactService InProfileCompactService)
	{
		_DatabaseService = InDatabaseService;
		_MainHubService = InMainHubService;
		_ProfileService = InProfileService;
		_ProfileCompactService = InProfileCompactService;
	}

	public async Task<MatchGetNewResponseModel> GetNewAsync(ServiceModel<MatchGetNewRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
			if (Account == null) return new MatchGetNewResponseModel { Error = ErrorConstants.ERROR_DATABASE };
			if (Account.Status == EAccountStatus.Suspended) return new MatchGetNewResponseModel { Error = ErrorConstants.ERROR_ACCOUNT_SUSPENDED };

			if (await _DatabaseService.UpdateAccountActiveAsync(Account, InConnection) <= 0) return new MatchGetNewResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			var Profile = await _DatabaseService.GetModelByIdAsync(new ProfileModel { Id = Account.ProfileId }, InConnection);
			if (Profile == null) return new MatchGetNewResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			var Filter = await _DatabaseService.GetModelByIdAsync(new FilterModel { Id = Account.FilterId }, InConnection);
			if (Filter == null) return new MatchGetNewResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			var GetNewMatchModel = new GetNewMatchModel
			(
				Account.Id,
				Profile.PronounsId, Profile.SexId, Profile.GenderId, Profile.RegionId, Profile.CountryId, Profile.BirthDate?.GetAge(), Profile.LookingFor,
				Filter.PronounsId, Filter.SexId, Filter.GenderId, Filter.RegionId, Filter.CountryId, Filter.FromAge, Filter.ToAge, Filter.AgeEnabled
			);

			var MatchAccount = await _DatabaseService.GetNewMatchAsync(GetNewMatchModel, InConnection);
			if (MatchAccount == null) return new MatchGetNewResponseModel();

			return new MatchGetNewResponseModel
			{
				FullProfile = await _ProfileService.GetFullProfileFromAccountAsync(InModel.Auth, MatchAccount, true, InConnection)
			};
		});
	}
	public async Task<MatchGetChoicesResponseModel> GetChoicesAsync(ServiceModel<MatchGetChoicesRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
			if (Account == null) return new MatchGetChoicesResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			if (await _DatabaseService.UpdateAccountActiveAsync(Account, InConnection) <= 0) return new MatchGetChoicesResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			var GetMatchChoicesModel = new GetMatchChoicesModel(InModel.Auth.AccountId, InModel.Request.Offset, InModel.Request.Count, InModel.Request.Choice);
			var MatchChoices = await _DatabaseService.GetMatchChoicesAsync(GetMatchChoicesModel, InConnection);
			var CompactProfileList = new List<ProfileCompactModel>();

			foreach (var Choice in MatchChoices)
			{
				var MatchingAccount = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = Choice.MatchAccountId }, InConnection);
				if (MatchingAccount == null) continue;

				var NewCompactProfile = await _ProfileCompactService.GetFromAccountAsync(MatchingAccount, InModel.Request.Choice, InConnection);
				CompactProfileList.Add(NewCompactProfile);
			}

			return new MatchGetChoicesResponseModel
			{
				CompactProfiles = CompactProfileList,
				TotalProfiles = await _DatabaseService.GetTotalMatchChoicesAsync(GetMatchChoicesModel, InConnection),
			};
		});
	}
	public async Task<MatchGetFilterResponseModel> GetFilterAsync(ServiceModel<MatchGetFilterRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
			if (Account == null) return new MatchGetFilterResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			return new MatchGetFilterResponseModel
			{
				FullProfile = await _ProfileService.GetFullProfileFromAccountAsync(InModel.Auth, Account, false, InConnection)
			};
		});
	}
	public async Task<MatchEditFilterResponseModel> EditFilterAsync(ServiceModel<MatchEditFilterRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
			if (Account == null) return new MatchEditFilterResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			var Filter = await _DatabaseService.GetModelByIdAsync(new FilterModel { Id = Account.FilterId }, InConnection);
			if (Filter == null) return new MatchEditFilterResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			// TODO: validate model
			var ProfileData = await _ProfileService.GetProfileDataAsync(InConnection);

			var Pronouns = InModel.Request.PronounsId != null ? ProfileData.AllPronouns.FirstOrDefault(X => X.Id == InModel.Request.PronounsId) : null;
			var Sex = InModel.Request.SexId != null ? ProfileData.AllSexes.FirstOrDefault(X => X.Id == InModel.Request.SexId) : null;
			var Gender = InModel.Request.GenderId != null ? ProfileData.AllGenders.FirstOrDefault(X => X.Id == InModel.Request.GenderId) : null;
			var Region = InModel.Request.RegionId != null ? ProfileData.AllRegions.FirstOrDefault(X => X.Id == InModel.Request.RegionId) : null;
			var Country = InModel.Request.CountryId != null ? ProfileData.AllCountries.FirstOrDefault(X => X.Id == InModel.Request.CountryId) : null;

			Filter = Filter with
			{
				PronounsId = Pronouns?.Id,
				SexId = Sex?.Id,
				GenderId = Gender?.Id,
				RegionId = Country != null ? Country.RegionId : Region?.Id,
				CountryId = Country?.Id,
				FromAge = Math.Clamp(InModel.Request.FromAge, ProfileConstants.MIN_AGE, ProfileConstants.MAX_AGE),
				ToAge = Math.Clamp(InModel.Request.ToAge, Filter.FromAge, ProfileConstants.MAX_AGE),
				AgeEnabled = InModel.Request.AgeEnabled,
			};

			if (await _DatabaseService.UpdateModelByIdAsync(Filter, InConnection) <= 0)
				return new MatchEditFilterResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			return new MatchEditFilterResponseModel();
		});
	}
	public async Task<MatchChooseResponseModel> ChooseAsync(ServiceModel<MatchChooseRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
			if (Account == null) return new MatchChooseResponseModel { Error = ErrorConstants.ERROR_DATABASE };
			if (Account.Status == EAccountStatus.Suspended) return new MatchChooseResponseModel { Error = ErrorConstants.ERROR_ACCOUNT_SUSPENDED };
			if (await _DatabaseService.CheckAccountHasMatch(Account, InConnection)) return new MatchChooseResponseModel { Error = ErrorConstants.ERROR_INVALID_OPERATION };

			var MatchAccount = await _DatabaseService.GetAccountByPublicIdAsync(InModel.Request.PublicId, InConnection);
			if (MatchAccount == null) return new MatchChooseResponseModel { Error = ErrorConstants.ERROR_NO_SUCH_ACCOUNT };
			if (MatchAccount.Status == EAccountStatus.Suspended) return new MatchChooseResponseModel { Error = ErrorConstants.ERROR_ACCOUNT_SUSPENDED };

			var NewMatchChoice = new MatchChoiceModel { AccountId = Account.Id, MatchAccountId = MatchAccount.Id, Choice = InModel.Request.Choice };
			var PreviousMatchChoice = await _DatabaseService.GetMatchChoiceAsync(NewMatchChoice, InConnection);
			if (PreviousMatchChoice != null) NewMatchChoice = NewMatchChoice with { Id = PreviousMatchChoice.Id };
			if (PreviousMatchChoice != null && NewMatchChoice.Choice == PreviousMatchChoice.Choice) return new MatchChooseResponseModel { Error = ErrorConstants.ERROR_INVALID_OPERATION };

			var InvertedMatchChoice = new MatchChoiceModel { AccountId = MatchAccount.Id, MatchAccountId = Account.Id };
			InvertedMatchChoice = await _DatabaseService.GetMatchChoiceAsync(InvertedMatchChoice, InConnection);

			if (InvertedMatchChoice == null || InvertedMatchChoice.Choice <= 0 || NewMatchChoice.Choice <= 0)
			{
				if (PreviousMatchChoice == null && await _DatabaseService.InsertModelAsync<MatchChoiceModel>(NewMatchChoice, InConnection) <= 0)
					return new MatchChooseResponseModel { Error = ErrorConstants.ERROR_DATABASE };
				if (PreviousMatchChoice != null && await _DatabaseService.UpdateModelByIdAsync<MatchChoiceModel>(NewMatchChoice, InConnection) <= 0)
					return new MatchChooseResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				return new MatchChooseResponseModel();
			}

			await _DatabaseService.DeleteModelAsync<MatchChoiceModel>(NewMatchChoice, InConnection);
			await _DatabaseService.DeleteModelAsync<MatchChoiceModel>(InvertedMatchChoice, InConnection);

			var NewMatch = new MatchModel { AccountId = Account.Id, MatchAccountId = MatchAccount.Id };
			if (await _DatabaseService.InsertModelReturnIdAsync<MatchModel>(NewMatch, InConnection) is var MatchId && MatchId <= 0) return new MatchChooseResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			NewMatch = NewMatch with { Id = MatchId };
			await _MainHubService.OnMatchAsync(NewMatch, InConnection);
			return new MatchChooseResponseModel();
		});
	}
	public async Task<MatchUnmatchResponseModel> UnmatchAsync(ServiceModel<MatchUnmatchRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			var Match = await _DatabaseService.GetModelByIdAsync(new MatchModel { Id = InModel.Request.MatchId }, InConnection);
			if (Match == null || (Match.AccountId != InModel.Auth.AccountId && Match.MatchAccountId != InModel.Auth.AccountId)) return new MatchUnmatchResponseModel { Error = ErrorConstants.ERROR_INVALID_OPERATION };

			if (await _DatabaseService.DeleteModelAsync(Match, InConnection) <= 0) return new MatchUnmatchResponseModel { Error = ErrorConstants.ERROR_DATABASE };

			await _MainHubService.OnUnmatchAsync(Match.AccountId, Match.Id);
			await _MainHubService.OnUnmatchAsync(Match.MatchAccountId, Match.Id);
			return new MatchUnmatchResponseModel();
		});
	}
}