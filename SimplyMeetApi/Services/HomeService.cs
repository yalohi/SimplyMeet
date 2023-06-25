namespace SimplyMeetApi.Services;

public class HomeService
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
	public HomeService(DatabaseService InDatabaseService)
	{
		_DatabaseService = InDatabaseService;
	}

	public async Task<HomeGetDataResponseModel> GetDataAsync(ServiceModel<HomeGetDataRequestModel> InModel)
	{
		if (InModel == null) throw new ArgumentNullException(nameof(InModel));

		return await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			var Cards = await _DatabaseService.GetAllAsync<CardModel>(InConnection);

			return new HomeGetDataResponseModel
			{
				Cards = Cards,
				TotalActiveAccounts = await _DatabaseService.GetTotalActiveAccountsAsync(InConnection),
				TotalActiveMatches = await _DatabaseService.GetTotalActiveMatchesAsync(InConnection),
			};
		});
	}
}