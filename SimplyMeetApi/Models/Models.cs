namespace SimplyMeetApi.Models;

public record AuthHubModel(String ConnectionId = default, Int32 AccountId = default, IEnumerable<RoleModel> Roles = default);
public record AuthModel(IPAddress RemoteIpAddress = default, Int32 AccountId = default, IEnumerable<RoleModel> Roles = default);
public record ChallengeModel(Byte[] Challenge = default, Byte[] SolvedChallenge = default, DateTime ExpireDateUTC = default);
public record GetMatchChoicesModel(Int32 AccountId = default, Int32 Offset = default, Int32 Count = default, EMatchChoice Choice = default);
public record GetNewMatchModel(Int32 AccountId = default, Int32? Profile_PronounsId = default, Int32? Profile_SexId = default, Int32? Profile_GenderId = default, Int32? Profile_RegionId = default, Int32? Profile_CountryId = default, Int32? Profile_Age = default, ELookingFor Profile_LookingFor = default, Int32? Filter_PronounsId = default, Int32? Filter_SexId = default, Int32? Filter_GenderId = default, Int32? Filter_RegionId = default, Int32? Filter_CountryId = default, Int32 Filter_FromAge = default, Int32 Filter_ToAge = default, Boolean Filter_AgeEnabled = default);
public record IdsModel(IEnumerable<Int32> Ids = default);
public record MessageInfoModel(String FromPublicId = default);
public record ProfileEditAvatarModel(AuthModel Auth = default, IFormFile File = default);
public record ReportedAccountModel(Int32 AccountId = default, Int32 ReportCount = default);
public record ServiceHubModel<TRequest>(AuthHubModel Auth = default, TRequest Request = default);
public record ServiceModel<TRequest>(AuthModel Auth = default, TRequest Request = default);
public record ThrottleLimitModel(Int32 Limit = default, Int32 ResetMinutes = default);
//public record TokenModel(DateTime Created = default, AuthenticationTicket Ticket = default);