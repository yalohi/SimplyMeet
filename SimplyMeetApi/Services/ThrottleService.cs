namespace SimplyMeetApi.Services;

public class ThrottleService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly ConcurrentDictionary<EThrottleGroup, ThrottleLimitModel> _ThrottleLimitDict;
	private readonly ConcurrentDictionary<IPAddress, ConcurrentDictionary<EThrottleGroup, M_ThrottleModel>> _ThrottleDict;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public ThrottleService(IOptions<ThrottleConfiguration> InThrottleConfig)
	{
		_ThrottleLimitDict = new ();
		_ThrottleDict = new ();

		_ThrottleLimitDict.TryAdd(EThrottleGroup.General, new ThrottleLimitModel(60, 1));
		_ThrottleLimitDict.TryAdd(EThrottleGroup.AccountLogin, new ThrottleLimitModel(10, 15));
		_ThrottleLimitDict.TryAdd(EThrottleGroup.AccountCreate, new ThrottleLimitModel(3, 60));
		_ThrottleLimitDict.TryAdd(EThrottleGroup.Report, new ThrottleLimitModel(5, 60));
		_ThrottleLimitDict.TryAdd(EThrottleGroup.Chat, new ThrottleLimitModel(15, 1));

		if (InThrottleConfig.Value.ThrottleLimitDict != null)
		{
			foreach (var Pair in InThrottleConfig.Value.ThrottleLimitDict)
			{
				if (_ThrottleLimitDict.TryGetValue(Pair.Key, out var LimitModel)) _ThrottleLimitDict.TryUpdate(Pair.Key, Pair.Value, LimitModel);
				else _ThrottleLimitDict.TryAdd(Pair.Key, Pair.Value);
			}
		}
	}

	public Boolean IsThrottled(IPAddress InIpAddress, EThrottleGroup InGroup)
	{
		if (InIpAddress == null) throw new ArgumentNullException(nameof(InIpAddress));

		if (!_ThrottleDict.TryGetValue(InIpAddress, out var TypeDict)) return false;

		if (!TypeDict.TryGetValue(EThrottleGroup.General, out var GeneralThrottleModel)) return false;
		if (IsThrottleLimitReached(GeneralThrottleModel, EThrottleGroup.General)) return true;

		if (InGroup != EThrottleGroup.General)
		{
			if (!TypeDict.TryGetValue(InGroup, out var ThrottleModel)) return false;
			if (IsThrottleLimitReached(ThrottleModel, InGroup)) return true;
		}

		return false;
	}
	public void IncrementRequestCount(IPAddress InIpAddress, EThrottleGroup InGroup)
	{
		if (InIpAddress == null) throw new ArgumentNullException(nameof(InIpAddress));

		if (!_ThrottleDict.TryGetValue(InIpAddress, out var TypeDict)) _ThrottleDict.TryAdd(InIpAddress, TypeDict = new ());

		if (!TypeDict.TryGetValue(EThrottleGroup.General, out var GeneralThrottleModel)) TypeDict.TryAdd(InGroup, GeneralThrottleModel = new ());
		if (++GeneralThrottleModel.RequestCount == 1) GeneralThrottleModel.FirstRequestDateUTC = DateTime.UtcNow;

		if (InGroup != EThrottleGroup.General)
		{
			if (!TypeDict.TryGetValue(InGroup, out var ThrottleModel)) TypeDict.TryAdd(InGroup, ThrottleModel = new ());
			if (++ThrottleModel.RequestCount == 1) ThrottleModel.FirstRequestDateUTC = DateTime.UtcNow;
		}
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private Boolean IsThrottleLimitReached(M_ThrottleModel InModel, EThrottleGroup InGroup)
	{
		if (!_ThrottleLimitDict.TryGetValue(InGroup, out var LimitModel)) return false;

		if ((DateTime.UtcNow - InModel.FirstRequestDateUTC).TotalMinutes >= LimitModel.ResetMinutes)
		{
			InModel.RequestCount = 0;
			return false;
		}

		return InModel.RequestCount >= LimitModel.Limit;
	}
}