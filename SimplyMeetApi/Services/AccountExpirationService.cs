namespace SimplyMeetApi.Services;

public class AccountExpirationService : BackgroundService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly AccountService _AccountService;
	private readonly DatabaseService _DatabaseService;
	private readonly ILogger _Logger;
	private readonly AccountConfiguration _AccountConfig;
	private readonly PeriodicTimer _Timer;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public AccountExpirationService(AccountService InAccountService, DatabaseService InDatabaseService, ILogger<AccountExpirationService> InLogger, IOptions<AccountConfiguration> InAccountConfig)
	{
		_AccountService = InAccountService;
		_DatabaseService = InDatabaseService;
		_Logger = InLogger;
		_AccountConfig = InAccountConfig.Value;
		_Timer = new PeriodicTimer(TimeSpan.FromSeconds(_AccountConfig.AccountExpirationCheckSeconds));
	}

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task ExecuteAsync(CancellationToken InCancelToken)
	{
		if (_AccountConfig.AccountExpirationDayCount <= 0) return;

		while (await _Timer.WaitForNextTickAsync(InCancelToken) && !InCancelToken.IsCancellationRequested)
			await DeleteExpiredAccountsAsync();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task DeleteExpiredAccountsAsync()
	{
		await _DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			var AccountIds = await _DatabaseService.GetExpiredAccountIdsAsync(new GetExpiredAccountIdsModel(_AccountConfig.AccountExpirationDayCount), InConnection);
			foreach (var AccountId in AccountIds) await _AccountService.DeleteAsync(AccountId, InConnection);
		});
	}
}