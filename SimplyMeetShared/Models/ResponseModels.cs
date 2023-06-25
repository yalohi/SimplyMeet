namespace SimplyMeetShared.ResponseModels;

// abstract
public abstract record ResponseModelBase(String Error = default, HttpStatusCode? ErrorCode = default);

public abstract record AccountGetChallengeResponseModelBase(Byte[] Nonce = default, Byte[] ServerPublicKey = default, Byte[] Challenge = default) : ResponseModelBase;
public abstract record AccountLoginResponseModelBase(String Token = default) : ResponseModelBase;
public abstract record AdminGetAccountRolesResponseModelBase(IEnumerable<RoleModel> Roles = default, IEnumerable<AccountRoleModel> AccountRoles = default, ProfileCompactModel MainAdminCompactProfile = default, IEnumerable<ProfileCompactModel> CompactProfiles = default) : ResponseModelBase;
public abstract record AdminGetProfileDataResponseModelBase(M_ProfileDataModel ProfileData = default) : ResponseModelBase;
public abstract record AdminGetReportedProfilesResponseModelBase(IEnumerable<ReportedProfileModel> ReportedProfiles = default, Int32 TotalReportedAccounts = default) : ResponseModelBase;
public abstract record HomeGetDataResponseModelBase(IEnumerable<CardModel> Cards = default, Int32 TotalActiveAccounts = default, Int32 TotalActiveMatches = default) : ResponseModelBase;
public abstract record MatchGetChoicesResponseModelBase(IEnumerable<ProfileCompactModel> CompactProfiles = default, Int32 TotalProfiles = default) : ResponseModelBase;
public abstract record MatchGetFilterResponseModelBase(ProfileFullModel FullProfile = default) : ResponseModelBase;
public abstract record MatchGetNewResponseModelBase(ProfileFullModel FullProfile = default) : ResponseModelBase;
public abstract record ProfileGetCompactResponseModelBase(ProfileCompactModel CompactProfile = default) : ResponseModelBase;
public abstract record ProfileGetEditDataResponseModelBase(ProfileFullModel FullProfile = default) : ResponseModelBase;
public abstract record ProfileGetResponseModelBase(ProfileFullModel FullProfile = default) : ResponseModelBase;

// models
public record AccountDeleteResponseModel : ResponseModelBase;
public record AccountGetChallengeResponseModel : AccountGetChallengeResponseModelBase;
public record AccountLoginResponseModel : AccountLoginResponseModelBase;
public record AdminEditAccountRolesResponseModel : ResponseModelBase;
public record AdminEditProfileDataResponseModel : ResponseModelBase;
public record AdminGetAccountRolesResponseModel : AdminGetAccountRolesResponseModelBase;
public record AdminGetProfileDataResponseModel : AdminGetProfileDataResponseModelBase;
public record AdminGetReportedProfilesResponseModel : AdminGetReportedProfilesResponseModelBase;
public record AdminSuspendAccountResponseModel : ResponseModelBase;
public record ErrorResponseModel : ResponseModelBase;
public record HomeGetDataResponseModel : HomeGetDataResponseModelBase;
public record MatchChooseResponseModel : ResponseModelBase;
public record MatchEditFilterResponseModel : ResponseModelBase;
public record MatchGetChoicesResponseModel : MatchGetChoicesResponseModelBase;
public record MatchGetFilterResponseModel : MatchGetFilterResponseModelBase;
public record MatchGetNewResponseModel : MatchGetNewResponseModelBase;
public record MatchUnmatchResponseModel : ResponseModelBase;
public record ProfileEditAvatarResponseModel : ResponseModelBase;
public record ProfileEditResponseModel : ResponseModelBase;
public record ProfileGetCompactResponseModel : ProfileGetCompactResponseModelBase;
public record ProfileGetEditDataResponseModel : ProfileGetEditDataResponseModelBase;
public record ProfileGetResponseModel : ProfileGetResponseModelBase;
public record ProfileReportResponseModel : ResponseModelBase;
public record ProfileResetAvatarResponseModel : ResponseModelBase;