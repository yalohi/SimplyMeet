namespace SimplyMeetWasm.Services;

public class AccountService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	#endregion
	#region Fields
	private readonly AppState _AppState;
	private readonly HttpService _HttpService;
	private readonly LocalizationService _LocalizationService;
	private readonly LocalStorageService _LocalStorageService;
	private readonly NavigationService _NavigationService;
	private readonly NotificationService _NotificationService;
	#endregion
	#region Events
	public event EventHandler Login;
	public event EventHandler Logout;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public AccountService(AppState InAppState, HttpService InHttpService, LocalizationService InLocalizationService, LocalStorageService InLocalStorageService, NavigationService InNavigationService, NotificationService InNotificationService)
	{
		_AppState = InAppState;
		_HttpService = InHttpService;
		_LocalizationService = InLocalizationService;
		_LocalStorageService = InLocalStorageService;
		_NavigationService = InNavigationService;
		_NotificationService = InNotificationService;
	}

	public async Task<Byte[]> GetLocalPublicKeyAsync()
	{
		TryGetPublicKey(await _LocalStorageService.GetItemAsync<String>(LocalStorageConstants.PRIVATE_KEY_STORAGE_KEY), out var PublicKey);
		return PublicKey;
	}

	public async Task RequestLoginAsync(String InUserPrivateKey_Base64)
	{
		if (InUserPrivateKey_Base64 == null) throw new ArgumentNullException(nameof(InUserPrivateKey_Base64));

		if (!TryGetPublicKey(InUserPrivateKey_Base64, out var UserPublicKey))
		{
			_NotificationService.SetMainNotification(_LocalizationService[ErrorConstants.ERROR_INVALID_PRIVATE_KEY], ENotificationType.Danger);
			return;
		}

		await _LocalStorageService.SetItemAsync<String>(LocalStorageConstants.PRIVATE_KEY_STORAGE_KEY, InUserPrivateKey_Base64);

		var GetChallengeRequestModel = new AccountGetChallengeRequestModel { UserPublicKey = UserPublicKey };
		var GetChallengeResponse = await _HttpService.PostJsonRequestAsync<AccountGetChallengeRequestModel, AccountGetChallengeResponseModel>(ApiRequestConstants.ACCOUNT_GET_CHALLENGE, GetChallengeRequestModel);
		if (!_HttpService.ValidateResponse(GetChallengeResponse)) return;

		var UserPrivateKey = Convert.FromBase64String(InUserPrivateKey_Base64);
		TrySolveChallengeAsync(GetChallengeResponse, UserPrivateKey, out var SolvedChallenge);
		if (SolvedChallenge != null) await LoginAsync(UserPublicKey, SolvedChallenge);
	}
	public async Task RequestGenerateAccountAsync()
	{
		NaCl.Curve25519XSalsa20Poly1305.KeyPair(out var UserPrivateKey, out var UserPublicKey);

		await _LocalStorageService.SetItemAsync<String>(LocalStorageConstants.PRIVATE_KEY_STORAGE_KEY, Convert.ToBase64String(UserPrivateKey));
		_AppState.IsFirstLogin = true;

		var CreateRequestModel = new AccountCreateRequestModel { UserPublicKey = UserPublicKey };
		var CreateResponse = await _HttpService.PostJsonRequestAsync<AccountCreateRequestModel, AccountGetChallengeResponseModel>(ApiRequestConstants.ACCOUNT_CREATE, CreateRequestModel);
		if (!_HttpService.ValidateResponse(CreateResponse)) return;

		TrySolveChallengeAsync(CreateResponse, UserPrivateKey, out var SolvedChallenge);
		if (SolvedChallenge != null) await LoginAsync(UserPublicKey, SolvedChallenge);
	}
	public async Task LogoutAsync()
	{
		await _LocalStorageService.RemoveItemAsync(LocalStorageConstants.LOGIN_TOKEN_STORAGE_KEY);
		await _LocalStorageService.RemoveItemAsync(LocalStorageConstants.PRIVATE_KEY_STORAGE_KEY);
		_AppState.IsFirstLogin = false;
		Logout?.Invoke(this, EventArgs.Empty);
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task LoginAsync(Byte[] InUserPublicKey, Byte[] InSolvedChallenge)
	{
		var LoginRequestModel = new AccountLoginRequestModel
		{
			UserPublicKey = InUserPublicKey,
			SolvedChallenge = InSolvedChallenge,
		};

		var LoginResponse = await _HttpService.PostJsonRequestAsync<AccountLoginRequestModel, AccountLoginResponseModel>(ApiRequestConstants.ACCOUNT_LOGIN, LoginRequestModel);
		if (!_HttpService.ValidateResponse(LoginResponse)) return;

		_NotificationService.ClearMainNotification();

		await _LocalStorageService.SetItemAsync<String>(LocalStorageConstants.LOGIN_TOKEN_STORAGE_KEY, LoginResponse.Token);
		Login?.Invoke(this, EventArgs.Empty);

		if (await _AppState.HasLoginAsync()) _NavigationService.NavigateTo(NavigationConstants.NAV_PROFILE);
	}

	private Boolean TryGetPublicKey(String InPrivateKey_Base64, out Byte[] OutPublicKey)
	{
		var PrivateKey = Convert.FromBase64String(InPrivateKey_Base64);
		OutPublicKey = null;

		try
		{
			OutPublicKey = NaCl.Curve25519.ScalarMultiplicationBase(PrivateKey);
			return true;
		}

		catch (Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
			return false;
		}
	}
	private Boolean TrySolveChallengeAsync(AccountGetChallengeResponseModel InGetChallengeResponse, Byte[] InPrivateKey, out Byte[] OutSolvedChallenge)
	{
		try
		{
			var CryptoBox = new NaCl.Curve25519XSalsa20Poly1305(InPrivateKey, InGetChallengeResponse.ServerPublicKey);
			OutSolvedChallenge = new Byte[InGetChallengeResponse.Challenge.Length - NaCl.Curve25519XSalsa20Poly1305.TagLength];
			return CryptoBox.TryDecrypt(OutSolvedChallenge, InGetChallengeResponse.Challenge, InGetChallengeResponse.Nonce);
		}

		catch (Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
			OutSolvedChallenge = null;
			return false;
		}
	}
}