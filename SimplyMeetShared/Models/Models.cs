namespace SimplyMeetShared.Models;

// abstract
public abstract record AccountModelBase(Int32 Id = default, String PublicId = default, String PublicKey_Base64 = default, DateTime Creation = default, DateTime? LastActive = default, EAccountStatus Status = default, Int32 ProfileId = default, Int32 FilterId = default);
public abstract record AccountRoleModelBase(Int32 Id = default, Int32 RoleId = default, String AccountPublicId = default);
public abstract record CardModelBase(Int32 Id = default, String Title = default, String Content = default);
public abstract record CountryModelBase(Int32 RegionId = default) : ProfileDataIconModelBase;
public abstract record FilterModelBase(Int32 Id = default, Int32? PronounsId = default, Int32? SexId = default, Int32? GenderId = default, Int32? RegionId = default, Int32? CountryId = default, Int32 FromAge = default, Int32 ToAge = default, Boolean AgeEnabled = default);
public abstract record MainHubLocalUserModelBase(ProfileCompactModel CompactProfile = default, IEnumerable<String> Roles = default);
public abstract record MainHubMatchUserModelBase(ProfileCompactModel CompactProfile = default, Byte[] PublicKey = default, Int32 MatchId = default);
public abstract record MatchChoiceModelBase(Int32 Id = default, Int32 AccountId = default, Int32 MatchAccountId = default, EMatchChoice Choice = default);
public abstract record MatchModelBase(Int32 Id = default, Int32 AccountId = default, Int32 MatchAccountId = default);
public abstract record MessageClientDataModelBase(String Message = default);
public abstract record MessageModelBase(Int32 Id = default, Int32 MatchId = default, String FromPublicId = default, String ServerDataJson = default, String ClientDataJson_Encrypted_Base64 = default, String ClientDataJson_Nonce_Base64 = default); //Int32 ToPublicKeyId
public abstract record MessageServerDataModelBase(DateTime DateTime = default);
public abstract record ProfileCompactModelBase(String Avatar = default, String DisplayName = default, String PublicId = default, EMatchChoice Choice = default);
public abstract record ProfileDataModelBase(Int32 Id = default, String Name = default);
public abstract record ProfileDataIconModelBase(String Icon = default) : ProfileDataModelBase;
public abstract record ProfileFullModelBase(EAccountFlags AccountFlags = default, M_ProfileDataModel Data = default, AccountModel Account = default, ProfileModel Profile = default, FilterModel Filter = default, IEnumerable<TagModel> Tags = default, IEnumerable<SexualityModel> Sexualities = default, IEnumerable<ReportReasonModel> ReportReasons = default, Boolean Reported = default);
public abstract record ProfileModelBase(Int32 Id = default, String Avatar = default, String DisplayName = default, Int32? PronounsId = default, Int32? SexId = default, Int32? GenderId = default, Int32? RegionId = default, Int32? CountryId = default, DateTime? BirthDate = default, ELookingFor LookingFor = default, String AboutMe = default, String AboutYou = default);
public abstract record ProfileSexualityModelBase(Int32 ProfileId = default, Int32 SexualityId = default);
public abstract record ProfileTagModelBase(Int32 ProfileId = default, Int32 TagId = default);
public abstract record ReportedProfileModelBase(ProfileCompactModel CompactProfile = default, Int32 ReportCount = default);
public abstract record ReportModelBase(Int32 Id = default, Int32 ReporterAccountId = default, Int32 ReportedAccountId = default, Int32 ReportReasonId = default);
public abstract record ReportReasonModelBase(Int32 Id = default, String Name = default);
public abstract record RoleModelBase(Int32 Id = default, String Name = default);
public abstract record TagModelBase(Int32 Id = default, String Name = default);

// models
public record AccountModel : AccountModelBase;
public record AccountRoleModel : AccountRoleModelBase;
public record CardModel : CardModelBase;
public record CountryModel : CountryModelBase;
public record FilterModel : FilterModelBase;
public record GenderModel : ProfileDataModelBase;
public record MainHubLocalUserModel : MainHubLocalUserModelBase;
public record MainHubMatchUserModel : MainHubMatchUserModelBase;
public record MatchChoiceModel : MatchChoiceModelBase;
public record MatchModel : MatchModelBase;
public record MessageClientDataModel : MessageClientDataModelBase;
public record MessageModel : MessageModelBase;
public record MessageServerDataModel : MessageServerDataModelBase;
public record ProfileCompactModel : ProfileCompactModelBase;
public record ProfileFullModel : ProfileFullModelBase;
public record ProfileModel : ProfileModelBase;
public record ProfileSexualityModel : ProfileSexualityModelBase;
public record ProfileTagModel : ProfileTagModelBase;
public record PronounsModel : ProfileDataModelBase;
public record RegionModel : ProfileDataIconModelBase;
public record ReportedProfileModel : ReportedProfileModelBase;
public record ReportModel : ReportModelBase;
public record ReportReasonModel : ReportReasonModelBase;
public record RoleModel : RoleModelBase;
public record SexModel : ProfileDataIconModelBase;
public record SexualityModel : ProfileDataModelBase;
public record TagModel : TagModelBase;