namespace SimplyMeetApi.Services;

public class TokenService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly ILogger _Logger;
	private readonly DatabaseService _DatabaseService;
	private readonly AdminConfiguration _AdminConfig;
	private readonly TokenConfiguration _TokenConfig;

	private readonly Dictionary<String, List<ChallengeModel>> _ChallengeDict;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public TokenService(ILogger<TokenService> InLogger, DatabaseService InDatabaseService, IOptions<AdminConfiguration> InAdminConfig, IOptions<TokenConfiguration> InTokenConfig)
	{
		_Logger = InLogger;
		_DatabaseService = InDatabaseService;
		_AdminConfig = InAdminConfig.Value;
		_TokenConfig = InTokenConfig.Value;

		_ChallengeDict = new ();
	}

	public AccountGetChallengeResponseModel CreateChallenge(Byte[] InUserPublicKey)
	{
		if (InUserPublicKey == null) throw new ArgumentNullException(nameof(InUserPublicKey));

		try
		{
			var Nonce = new Byte[NaCl.Curve25519XSalsa20Poly1305.NonceLength];
			RandomStatics.RANDOM_CRYPTO.GetBytes(Nonce);

			var SolvedChallenge = new Byte[AccountConstants.SOLVED_CHALLENGE_LENGTH];
			RandomStatics.RANDOM_CRYPTO.GetBytes(SolvedChallenge);

			NaCl.Curve25519XSalsa20Poly1305.KeyPair(out var ServerPrivateKey, out var ServerPublicKey);
			var CryptoBox = new NaCl.Curve25519XSalsa20Poly1305(ServerPrivateKey, InUserPublicKey);
			var Challenge = new Byte[SolvedChallenge.Length + NaCl.Curve25519XSalsa20Poly1305.TagLength];
			CryptoBox.Encrypt(Challenge, SolvedChallenge, Nonce);

			var NewChallenge = new ChallengeModel(Challenge, SolvedChallenge, DateTime.UtcNow.Add(TimeSpan.FromSeconds(AccountConstants.CHALLENGE_TIMEOUT_SECONDS)));

			lock (_ChallengeDict)
			{
				var UserPublicKey_Base64 = Convert.ToBase64String(InUserPublicKey);
				if (!_ChallengeDict.TryGetValue(UserPublicKey_Base64, out var ChallengeList)) _ChallengeDict.TryAdd(UserPublicKey_Base64, ChallengeList = new ());
				ChallengeList.RemoveAll(X => X.ExpireDateUTC <= DateTime.UtcNow);
				ChallengeList.Add(NewChallenge);
			}

			return new AccountGetChallengeResponseModel
			{
				Nonce = Nonce,
				ServerPublicKey = ServerPublicKey,
				Challenge = NewChallenge.Challenge,
			};
		}

		catch (Exception Ex)
		{
			_Logger.LogError(Ex, Ex.Message);
			return new AccountGetChallengeResponseModel { Error = ErrorConstants.ERROR_CRYPTO };
		}
	}
	public async Task<String> AuthenticateAsync(Byte[] InUserPublicKey, Byte[] InSolvedChallenge, IDbConnection InConnection)
	{
		if (InUserPublicKey == null) throw new ArgumentNullException(nameof(InUserPublicKey));
		if (InSolvedChallenge == null) throw new ArgumentNullException(nameof(InSolvedChallenge));
		if (InConnection == null) throw new ArgumentNullException(nameof(InConnection));

		var UserPublicKey_Base64 = Convert.ToBase64String(InUserPublicKey);

		lock (_ChallengeDict)
		{
			if (!_ChallengeDict.TryGetValue(UserPublicKey_Base64, out var ChallengeList)) return null;
			ChallengeList.RemoveAll(X => X.ExpireDateUTC <= DateTime.UtcNow);

			var Challenge = ChallengeList.FirstOrDefault(X => X.SolvedChallenge.SequenceEqual(InSolvedChallenge));
			if (Challenge == null) return null;

			ChallengeList.Remove(Challenge);
		}

		var Account = await _DatabaseService.GetAccountByPublicKeyAsync(UserPublicKey_Base64, InConnection);
		if (Account == null) return null;

		return CreateToken(Account);
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private String CreateToken(AccountModel InAccount)
	{
		var Claims = new List<Claim>();
		Claims.Add(new Claim(ClaimTypes.NameIdentifier, InAccount.Id.ToString()));

		var SecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_TokenConfig.SecretKey));
		var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
		var TokenDescriptor = new JwtSecurityToken(_TokenConfig.Issuer, _TokenConfig.Issuer, Claims, null, null, Credentials);
		return new JwtSecurityTokenHandler().WriteToken(TokenDescriptor);
	}
}