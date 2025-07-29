namespace SimplyMeetApi.Services;

public class HomeService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly DatabaseService _DatabaseService;
	private readonly HomeConfiguration _HomeConfig;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public HomeService(DatabaseService InDatabaseService, IOptions<HomeConfiguration> InHomeConfig)
	{
		_DatabaseService = InDatabaseService;
		_HomeConfig = InHomeConfig.Value;
	}

	public async Task<HomeGetDataResponseModel> GetDataAsync(ServiceModel<HomeGetDataRequestModel> InModel)
	{
		ArgumentNullException.ThrowIfNull(InModel);

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			return new HomeGetDataResponseModel
			{
				Cards = _HomeConfig.Cards,
				TotalActiveAccounts = await _DatabaseService.GetTotalActiveAccountsAsync(InConnection),
				TotalActiveMatches = await _DatabaseService.GetTotalActiveMatchesAsync(InConnection),
			};
		});
	}
	public async Task<HomeGetServerInfoResponseModel> GetServerInfoAsync(ServiceModel<HomeGetServerInfoRequestModel> InModel)
	{
		ArgumentNullException.ThrowIfNull(InModel);

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			return new HomeGetServerInfoResponseModel
			{
				ShortDescription = _HomeConfig.ShortDescription,
				Administration = _HomeConfig.Administration,
				TotalActiveAccounts = await _DatabaseService.GetTotalActiveAccountsAsync(InConnection),
				TotalActiveMatches = await _DatabaseService.GetTotalActiveMatchesAsync(InConnection),
			};
		});
	}
}
