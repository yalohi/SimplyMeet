using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Dapper;
using SimplyMeetApi.Configuration;
using SimplyMeetApi.Models;
using SimplyMeetShared.Models;
using SimplyMeetShared.RequestModels;
using SimplyMeetShared.Constants;

using static SimplyMeetApi.Constants.DatabaseConstants;
using SimplyMeetShared.Enums;
using Microsoft.Extensions.Logging;

namespace SimplyMeetApi.Services
{
	public class DatabaseService
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Static Fields
		// Insert Params
		private static readonly String PARAM_ACCOUNT_SQL = GetParamSql<AccountModel>(false);
		private static readonly String PARAM_ACCOUNT_ROLE_SQL = GetParamSql<AccountRoleModel>(false);
		private static readonly String PARAM_PROFILE_SQL = GetParamSql<ProfileModel>(false);
		private static readonly String PARAM_PROFILE_TAG_SQL = GetParamSql<ProfileTagModel>(false);
		private static readonly String PARAM_PROFILE_SEXUALITY_SQL = GetParamSql<ProfileSexualityModel>(false);
		private static readonly String PARAM_FILTER_SQL = GetParamSql<FilterModel>(false);

		private static readonly String PARAM_ROLE_SQL = GetParamSql<RoleModel>(false);
		private static readonly String PARAM_TAG_SQL = GetParamSql<TagModel>(false);
		private static readonly String PARAM_SEXUALITY_SQL = GetParamSql<SexualityModel>(false);

		private static readonly String PARAM_PRONOUNS_SQL = GetParamSql<PronounsModel>(false);
		private static readonly String PARAM_SEX_SQL = GetParamSql<SexModel>(false);
		private static readonly String PARAM_GENDER_SQL = GetParamSql<GenderModel>(false);
		private static readonly String PARAM_REGION_SQL = GetParamSql<RegionModel>(false);
		private static readonly String PARAM_COUNTRY_SQL = GetParamSql<CountryModel>(false);

		private static readonly String PARAM_MATCH_CHOICE_SQL = GetParamSql<MatchChoiceModel>(false);
		private static readonly String PARAM_MATCH_SQL = GetParamSql<MatchModel>(false);

		private static readonly String PARAM_MESSAGE_SQL = GetParamSql<MessageModel>(false);

		private static readonly String PARAM_REPORT_SQL = GetParamSql<ReportModel>(false);
		private static readonly String PARAM_REPORT_REASON_SQL = GetParamSql<ReportReasonModel>(false);

		// Update Params
		private static readonly String PARAM_UPDATE_ACCOUNT_SQL = GetParamSql<AccountModel>(true);
		private static readonly String PARAM_UPDATE_ACCOUNT_ROLE_SQL = GetParamSql<AccountRoleModel>(true);
		private static readonly String PARAM_UPDATE_PROFILE_SQL = GetParamSql<ProfileModel>(true);
		private static readonly String PARAM_UPDATE_PROFILE_TAG_SQL = GetParamSql<ProfileTagModel>(true);
		private static readonly String PARAM_UPDATE_PROFILE_SEXUALITY_SQL = GetParamSql<ProfileSexualityModel>(true);
		private static readonly String PARAM_UPDATE_FILTER_SQL = GetParamSql<FilterModel>(true);

		private static readonly String PARAM_UPDATE_ROLE_SQL = GetParamSql<RoleModel>(true);
		private static readonly String PARAM_UPDATE_TAG_SQL = GetParamSql<TagModel>(true);
		private static readonly String PARAM_UPDATE_SEXUALITY_SQL = GetParamSql<SexualityModel>(true);

		private static readonly String PARAM_UPDATE_PRONOUNS_SQL = GetParamSql<PronounsModel>(true);
		private static readonly String PARAM_UPDATE_SEX_SQL = GetParamSql<SexModel>(true);
		private static readonly String PARAM_UPDATE_GENDER_SQL = GetParamSql<GenderModel>(true);
		private static readonly String PARAM_UPDATE_REGION_SQL = GetParamSql<RegionModel>(true);
		private static readonly String PARAM_UPDATE_COUNTRY_SQL = GetParamSql<CountryModel>(true);

		private static readonly String PARAM_UPDATE_MATCH_CHOICE_SQL = GetParamSql<MatchChoiceModel>(true);
		private static readonly String PARAM_UPDATE_MATCH_SQL = GetParamSql<MatchModel>(true);

		private static readonly String PARAM_UPDATE_MESSAGE_SQL = GetParamSql<MessageModel>(true);

		private static readonly String PARAM_UPDATE_REPORT_SQL = GetParamSql<ReportModel>(true);
		private static readonly String PARAM_UPDATE_REPORT_REASON_SQL = GetParamSql<ReportReasonModel>(true);

		// Check
		private static readonly ReadOnlyDictionary<Type, String> CHECK_BY_ID_DICT = new (new Dictionary<Type, String>()
		{
			{ typeof(TagModel), $"{SELECT_COUNT_FROM} {TAGS_TABLE} {WHERE} {GetParamAndCondSql(nameof(TagModel.Id))}" },
			{ typeof(SexualityModel), $"{SELECT_COUNT_FROM} {SEXUALITIES_TABLE} {WHERE} {GetParamAndCondSql(nameof(SexualityModel.Id))}" },
		});

		private static readonly String CHECK_ACCOUNT_PUBLIC_KEY_SQL =
			$@"{SELECT_COUNT_FROM} {ACCOUNTS_TABLE}
			{WHERE} {GetParamAndCondSql(nameof(AccountModel.PublicKey_Base64))}";

		private static readonly String CHECK_ACCOUNT_PUBLIC_ID_SQL =
			$@"{SELECT_COUNT_FROM} {ACCOUNTS_TABLE}
			{WHERE} {GetParamAndCondSql(nameof(AccountModel.PublicId))}";

		private static readonly String CHECK_ACCOUNT_HAS_MATCH_SQL =
			$@"{SELECT_COUNT_FROM} {MATCHES_TABLE}
			{WHERE} {nameof(MatchModel.AccountId)} = @{nameof(AccountModel.Id)}
			{OR} {nameof(MatchModel.MatchAccountId)} = @{nameof(AccountModel.Id)}";

		// Get
		private static readonly ReadOnlyDictionary<Type, String> GET_ALL_DICT = new (new Dictionary<Type, String>()
		{
			{ typeof(CardModel), $"{SELECT_ALL_FROM} {CARDS_TABLE}" },

			{ typeof(AccountRoleModel), $"{SELECT_ALL_FROM} {ACCOUNT_ROLES_TABLE}" },

			{ typeof(RoleModel), $"{SELECT_ALL_FROM} {ROLES_TABLE}" },

			{ typeof(PronounsModel), $"{SELECT_ALL_FROM} {PRONOUNS_TABLE}" },
			{ typeof(SexModel), $"{SELECT_ALL_FROM} {SEXES_TABLE}" },
			{ typeof(GenderModel), $"{SELECT_ALL_FROM} {GENDERS_TABLE}" },
			{ typeof(RegionModel), $"{SELECT_ALL_FROM} {REGIONS_TABLE}" },
			{ typeof(CountryModel), $"{SELECT_ALL_FROM} {COUNTRIES_TABLE}" },
			{ typeof(SexualityModel), $"{SELECT_ALL_FROM} {SEXUALITIES_TABLE}" },

			{ typeof(ReportReasonModel), $"{SELECT_ALL_FROM} {REPORT_REASONS_TABLE}" },
		});

		private static readonly ReadOnlyDictionary<Type, String> GET_BY_ID_DICT = new (new Dictionary<Type, String>()
		{
			{ typeof(AccountModel), $"{SELECT_ALL_FROM} {ACCOUNTS_TABLE} {WHERE} {GetParamAndCondSql(nameof(AccountModel.Id))}" },
			{ typeof(ProfileModel), $"{SELECT_ALL_FROM} {PROFILES_TABLE} {WHERE} {GetParamAndCondSql(nameof(ProfileModel.Id))}" },
			{ typeof(FilterModel), $"{SELECT_ALL_FROM} {FILTERS_TABLE} {WHERE} {GetParamAndCondSql(nameof(FilterModel.Id))}" },

			{ typeof(AccountRoleModel), $"{SELECT_ALL_FROM} {ACCOUNT_ROLES_TABLE} {WHERE} {GetParamAndCondSql(nameof(AccountRoleModel.Id))}" },

			{ typeof(RoleModel), $"{SELECT_ALL_FROM} {ROLES_TABLE} {WHERE} {GetParamAndCondSql(nameof(RoleModel.Id))}" },
			{ typeof(TagModel), $"{SELECT_ALL_FROM} {TAGS_TABLE} {WHERE} {GetParamAndCondSql(nameof(TagModel.Id))}" },
			{ typeof(SexualityModel), $"{SELECT_ALL_FROM} {SEXUALITIES_TABLE} {WHERE} {GetParamAndCondSql(nameof(SexualityModel.Id))}" },

			{ typeof(MatchModel), $"{SELECT_ALL_FROM} {MATCHES_TABLE} {WHERE} {GetParamAndCondSql(nameof(MatchModel.Id))}" },
		});

		private static readonly String GET_TOTAL_ACTIVE_ACCOUNTS_SQL =
			$@"{SELECT_COUNT_FROM} {ACCOUNTS_TABLE}
			{WHERE} {DATE}({NOW}) < {DATE}({nameof(AccountModel.LastActive)}, '+{AccountConstants.FLAG_ACTIVE_DAYS} {DAYS}')";

		private static readonly String GET_TOTAL_ACTIVE_MATCHES_SQL =
			$@"{SELECT_COUNT_FROM} {MATCHES_TABLE}";

		private static readonly String GET_ACCOUNT_BY_PUBLIC_KEY_SQL =
			$@"{SELECT_ALL_FROM} {ACCOUNTS_TABLE}
			{WHERE} {GetParamAndCondSql(nameof(AccountModel.PublicKey_Base64))}";

		private static readonly String GET_ACCOUNT_BY_PUBLIC_ID_SQL =
			$@"{SELECT_ALL_FROM} {ACCOUNTS_TABLE}
			{WHERE} {GetParamAndCondSql(nameof(AccountModel.PublicId))}";

		private static readonly String GET_TAG_BY_NAME_SQL =
			$@"{SELECT_ALL_FROM} {TAGS_TABLE}
			{WHERE} {GetParamAndCondSql(nameof(TagModel.Name))}";

		private static readonly String GET_ACCOUNT_ROLES_SQL =
			$@"{SELECT_ALL_FROM} {ACCOUNT_ROLES_TABLE}
			{WHERE} {nameof(AccountRoleModel.AccountPublicId)} = @{nameof(AccountModel.PublicId)}";

		private static readonly String GET_PROFILE_TAGS_SQL =
			$@"{SELECT_ALL_FROM} {PROFILE_TAGS_TABLE}
			{WHERE} {nameof(ProfileTagModel.ProfileId)} = @{nameof(ProfileModel.Id)}";

		private static readonly String GET_PROFILE_SEXUALITIES_SQL =
			$@"{SELECT_ALL_FROM} {PROFILE_SEXUALITIES_TABLE}
			{WHERE} {nameof(ProfileSexualityModel.ProfileId)} = @{nameof(ProfileModel.Id)}";

		private static readonly String GET_MATCH_SQL =
			$@"{SELECT_ALL_FROM} {MATCHES_TABLE}
			{WHERE} {GetParamAndCondSql(nameof(MatchModel.AccountId), nameof(MatchModel.MatchAccountId))}";

		private static readonly String GET_MATCH_CHOICE_SQL =
			$@"{SELECT_ALL_FROM} {MATCH_CHOICES_TABLE}
			{WHERE} {GetParamAndCondSql(nameof(MatchChoiceModel.AccountId), nameof(MatchChoiceModel.MatchAccountId))}";

		private static readonly String GET_TOTAL_MATCH_CHOICES_SQL =
			$@"{SELECT_COUNT_FROM} {MATCH_CHOICES_TABLE}
			{WHERE} {nameof(MatchChoiceModel.AccountId)} = @{nameof(GetMatchChoicesModel.AccountId)}
			{AND} {nameof(MatchChoiceModel.Choice)} = @{nameof(GetMatchChoicesModel.Choice)}";

		private static readonly String GET_MATCH_CHOICES_SQL =
			$@"{SELECT_ALL_FROM} {MATCH_CHOICES_TABLE}
			{WHERE} {nameof(MatchChoiceModel.AccountId)} = @{nameof(GetMatchChoicesModel.AccountId)}
			{AND} {nameof(MatchChoiceModel.Choice)} = @{nameof(GetMatchChoicesModel.Choice)}
			{ORDER_BY} {nameof(MatchChoiceModel.Id)} {DESC}
			{LIMIT} @{nameof(GetMatchChoicesModel.Count)} {OFFSET} @{nameof(GetMatchChoicesModel.Offset)}";

		private static readonly String GET_ALL_MATCHES_SQL =
			$@"{SELECT_ALL_FROM} {MATCHES_TABLE}
			{WHERE} {nameof(MatchModel.AccountId)} = @{nameof(AccountModel.Id)}
			{OR} {nameof(MatchModel.MatchAccountId)} = @{nameof(AccountModel.Id)}";

		private static readonly String GET_REPORT_SQL =
			$@"{SELECT_ALL_FROM} {REPORTS_TABLE}
			{WHERE} {nameof(ReportModel.ReporterAccountId)} = @{nameof(ReportModel.ReporterAccountId)}
			{AND} {nameof(ReportModel.ReportedAccountId)} = @{nameof(ReportModel.ReportedAccountId)}";

		private static readonly String GET_PREVIOUS_MESSAGE_COUNT_SQL =
			$@"{SELECT_COUNT_FROM} {MESSAGES_TABLE}
			{WHERE} {nameof(MessageModel.MatchId)} = @{nameof(ChatGetHistoryRequestModel.MatchId)}
			{AND} {nameof(MessageModel.Id)} < @{nameof(ChatGetHistoryRequestModel.StartingMessageId)}";

		private static readonly String GET_FOLLOWING_MESSAGE_COUNT_SQL =
			$@"{SELECT_COUNT_FROM} {MESSAGES_TABLE}
			{WHERE} {nameof(MessageModel.MatchId)} = @{nameof(ChatGetHistoryRequestModel.MatchId)}
			{AND} {nameof(MessageModel.Id)} > @{nameof(ChatGetHistoryRequestModel.StartingMessageId)}";

		private static readonly String GET_PREVIOUS_MESSAGES_SQL =
			$@"{SELECT_ALL_FROM} {MESSAGES_TABLE}
			{WHERE} {nameof(MessageModel.MatchId)} = @{nameof(ChatGetHistoryRequestModel.MatchId)}
			{AND} {nameof(MessageModel.Id)} < @{nameof(ChatGetHistoryRequestModel.StartingMessageId)}
			{ORDER_BY} {nameof(MessageModel.Id)} {DESC}
			{LIMIT} @{nameof(ChatGetHistoryRequestModel.MessageCount)}";

		private static readonly String GET_FOLLOWING_MESSAGES_SQL =
			$@"{SELECT_ALL_FROM} {MESSAGES_TABLE}
			{WHERE} {nameof(MessageModel.MatchId)} = @{nameof(ChatGetHistoryRequestModel.MatchId)}
			{AND} {nameof(MessageModel.Id)} > @{nameof(ChatGetHistoryRequestModel.StartingMessageId)}
			{ORDER_BY} {nameof(MessageModel.Id)} {ASC}
			{LIMIT} @{nameof(ChatGetHistoryRequestModel.MessageCount)}";

		private static readonly String GET_NEW_MATCH_SQL =
			$@"{SELECT} A.* {FROM} {ACCOUNTS_TABLE} A

			{LEFT_JOIN} {MATCH_CHOICES_TABLE} C
			{ON} A.{nameof(AccountModel.Id)} = C.{nameof(MatchChoiceModel.AccountId)}
			{AND} C.{nameof(MatchChoiceModel.MatchAccountId)} = @{nameof(GetNewMatchModel.AccountId)}
			{LEFT_JOIN} {PROFILES_TABLE} P
			{ON} P.{nameof(ProfileModel.Id)} = A.{nameof(AccountModel.ProfileId)}
			{LEFT_JOIN} {FILTERS_TABLE} F
			{ON} F.{nameof(FilterModel.Id)} = A.{nameof(AccountModel.FilterId)}

			{WHERE} A.{nameof(AccountModel.Id)} != @{nameof(GetNewMatchModel.AccountId)}
			{AND} A.{nameof(AccountModel.Status)} != {(Int32)EAccountStatus.Suspended}
			{AND} A.{nameof(AccountModel.Id)} {NOT} {IN} ({SELECT} {nameof(MatchChoiceModel.MatchAccountId)} {FROM} {MATCH_CHOICES_TABLE} {WHERE} {nameof(MatchChoiceModel.AccountId)} = @{nameof(GetNewMatchModel.AccountId)})
			{AND} A.{nameof(AccountModel.Id)} {NOT} {IN} ({SELECT} {nameof(MatchModel.AccountId)} {FROM} {MATCHES_TABLE})
			{AND} A.{nameof(AccountModel.Id)} {NOT} {IN} ({SELECT} {nameof(MatchModel.MatchAccountId)} {FROM} {MATCHES_TABLE})

			{AND} (@{nameof(GetNewMatchModel.Filter_PronounsId)} {IS} {NULL} {OR} P.{nameof(ProfileModel.PronounsId)} = @{nameof(GetNewMatchModel.Filter_PronounsId)})
			{AND} (@{nameof(GetNewMatchModel.Filter_SexId)} {IS} {NULL} {OR} P.{nameof(ProfileModel.SexId)} = @{nameof(GetNewMatchModel.Filter_SexId)})
			{AND} (@{nameof(GetNewMatchModel.Filter_GenderId)} {IS} {NULL} {OR} P.{nameof(ProfileModel.GenderId)} = @{nameof(GetNewMatchModel.Filter_GenderId)})
			{AND} (@{nameof(GetNewMatchModel.Filter_RegionId)} {IS} {NULL} {OR} P.{nameof(ProfileModel.RegionId)} = @{nameof(GetNewMatchModel.Filter_RegionId)})
			{AND} (@{nameof(GetNewMatchModel.Filter_CountryId)} {IS} {NULL} {OR} P.{nameof(ProfileModel.CountryId)} = @{nameof(GetNewMatchModel.Filter_CountryId)})
			{AND} (@{nameof(GetNewMatchModel.Filter_AgeEnabled)} <= 0 {OR} {CAST}(({JULIANDAY}({NOW}) - {JULIANDAY}(P.{nameof(ProfileModel.BirthDate)})) / {ProfileConstants.DAYS_PER_YEAR} {AS} {INTEGER}) >= @{nameof(GetNewMatchModel.Filter_FromAge)})
			{AND} (@{nameof(GetNewMatchModel.Filter_AgeEnabled)} <= 0 {OR} {CAST}(({JULIANDAY}({NOW}) - {JULIANDAY}(P.{nameof(ProfileModel.BirthDate)})) / {ProfileConstants.DAYS_PER_YEAR} {AS} {INTEGER}) <= @{nameof(GetNewMatchModel.Filter_ToAge)})

			{AND} (F.{nameof(FilterModel.PronounsId)} {IS} {NULL} {OR} @{nameof(GetNewMatchModel.Profile_PronounsId)} = F.{nameof(FilterModel.PronounsId)})
			{AND} (F.{nameof(FilterModel.SexId)} {IS} {NULL} {OR} @{nameof(GetNewMatchModel.Profile_SexId)} = F.{nameof(FilterModel.SexId)})
			{AND} (F.{nameof(FilterModel.GenderId)} {IS} {NULL} {OR} @{nameof(GetNewMatchModel.Profile_GenderId)} = F.{nameof(FilterModel.GenderId)})
			{AND} (F.{nameof(FilterModel.RegionId)} {IS} {NULL} {OR} @{nameof(GetNewMatchModel.Profile_RegionId)} = F.{nameof(FilterModel.RegionId)})
			{AND} (F.{nameof(FilterModel.CountryId)} {IS} {NULL} {OR} @{nameof(GetNewMatchModel.Profile_CountryId)} = F.{nameof(FilterModel.CountryId)})
			{AND} (F.{nameof(FilterModel.AgeEnabled)} <= 0 {OR} @{nameof(GetNewMatchModel.Profile_Age)} >= F.{nameof(FilterModel.FromAge)})
			{AND} (F.{nameof(FilterModel.AgeEnabled)} <= 0 {OR} @{nameof(GetNewMatchModel.Profile_Age)} <= F.{nameof(FilterModel.ToAge)})

			{AND} (@{nameof(GetNewMatchModel.Profile_LookingFor)} <= 0 {OR} P.{nameof(ProfileModel.LookingFor)} <= 0 {OR} @{nameof(GetNewMatchModel.Profile_LookingFor)} & P.{nameof(ProfileModel.LookingFor)} > 0)

			{ORDER_BY} {DATE}({NOW}) < {DATE}(A.{nameof(AccountModel.LastActive)}, '+{AccountConstants.FLAG_ACTIVE_DAYS} {DAYS}') {DESC},
			C.{nameof(MatchChoiceModel.Choice)} {DESC}

			{LIMIT} 1";

		private static readonly String GET_TOTAL_REPORTED_ACCOUNTS_SQL =
			$@"{SELECT_COUNT_FROM} {REPORTS_TABLE}
			{GROUP_BY} {nameof(ReportModel.ReportedAccountId)}";

		private static readonly String GET_REPORTED_ACCOUNTS_SQL =
			$@"{SELECT}
			{nameof(ReportModel.ReportedAccountId)} {AS} {nameof(ReportedAccountModel.AccountId)},
			{COUNT}({nameof(ReportModel.ReportedAccountId)}) {AS} {nameof(ReportedAccountModel.ReportCount)}
			{FROM} {REPORTS_TABLE}
			{GROUP_BY} {nameof(ReportModel.ReportedAccountId)}
			{ORDER_BY} {nameof(ReportedAccountModel.ReportCount)} {DESC}
			{LIMIT} @{nameof(AdminGetReportedProfilesRequestModel.Count)} {OFFSET} @{nameof(AdminGetReportedProfilesRequestModel.Offset)}";

		// Insert
		private static readonly ReadOnlyDictionary<Type, String> INSERT_DICT = new (new Dictionary<Type, String>()
		{
			{ typeof(AccountModel), $"{INSERT_INTO} {ACCOUNTS_TABLE} {PARAM_ACCOUNT_SQL}" },
			{ typeof(AccountRoleModel), $"{INSERT_INTO} {ACCOUNT_ROLES_TABLE} {PARAM_ACCOUNT_ROLE_SQL}" },
			{ typeof(ProfileModel), $"{INSERT_INTO} {PROFILES_TABLE} {PARAM_PROFILE_SQL}" },
			{ typeof(ProfileTagModel), $"{INSERT_INTO} {PROFILE_TAGS_TABLE} {PARAM_PROFILE_TAG_SQL}" },
			{ typeof(ProfileSexualityModel), $"{INSERT_INTO} {PROFILE_SEXUALITIES_TABLE} {PARAM_PROFILE_SEXUALITY_SQL}" },
			{ typeof(FilterModel), $"{INSERT_INTO} {FILTERS_TABLE} {PARAM_FILTER_SQL}" },

			{ typeof(RoleModel), $"{INSERT_INTO} {ROLES_TABLE} {PARAM_ROLE_SQL}" },
			{ typeof(TagModel), $"{INSERT_INTO} {TAGS_TABLE} {PARAM_TAG_SQL}" },
			{ typeof(SexualityModel), $"{INSERT_INTO} {SEXUALITIES_TABLE} {PARAM_SEXUALITY_SQL}" },

			{ typeof(PronounsModel), $"{INSERT_INTO} {PRONOUNS_TABLE} {PARAM_PRONOUNS_SQL}" },
			{ typeof(SexModel), $"{INSERT_INTO} {SEXES_TABLE} {PARAM_SEX_SQL}" },
			{ typeof(GenderModel), $"{INSERT_INTO} {GENDERS_TABLE} {PARAM_GENDER_SQL}" },
			{ typeof(RegionModel), $"{INSERT_INTO} {REGIONS_TABLE} {PARAM_REGION_SQL}" },
			{ typeof(CountryModel), $"{INSERT_INTO} {COUNTRIES_TABLE} {PARAM_COUNTRY_SQL}" },

			{ typeof(MatchChoiceModel), $"{INSERT_INTO} {MATCH_CHOICES_TABLE} {PARAM_MATCH_CHOICE_SQL}" },
			{ typeof(MatchModel), $"{INSERT_INTO} {MATCHES_TABLE} {PARAM_MATCH_SQL}" },

			{ typeof(MessageModel), $"{INSERT_INTO} {MESSAGES_TABLE} {PARAM_MESSAGE_SQL}" },

			{ typeof(ReportModel), $"{INSERT_INTO} {REPORTS_TABLE} {PARAM_REPORT_SQL}" },
			{ typeof(ReportReasonModel), $"{INSERT_INTO} {REPORT_REASONS_TABLE} {PARAM_REPORT_REASON_SQL}" },
		});

		private static readonly ReadOnlyDictionary<Type, String> INSERT_RETURN_ID_DICT = new (new Dictionary<Type, String>()
		{
			{ typeof(AccountModel), $"{INSERT_INTO} {ACCOUNTS_TABLE} {PARAM_ACCOUNT_SQL} {RETURN_ID}" },
			{ typeof(AccountRoleModel), $"{INSERT_INTO} {ACCOUNT_ROLES_TABLE} {PARAM_ACCOUNT_ROLE_SQL} {RETURN_ID}" },
			{ typeof(ProfileModel), $"{INSERT_INTO} {PROFILES_TABLE} {PARAM_PROFILE_SQL} {RETURN_ID}" },
			{ typeof(ProfileTagModel), $"{INSERT_INTO} {PROFILE_TAGS_TABLE} {PARAM_PROFILE_TAG_SQL} {RETURN_ID}" },
			{ typeof(ProfileSexualityModel), $"{INSERT_INTO} {PROFILE_SEXUALITIES_TABLE} {PARAM_PROFILE_SEXUALITY_SQL} {RETURN_ID}" },
			{ typeof(FilterModel), $"{INSERT_INTO} {FILTERS_TABLE} {PARAM_FILTER_SQL} {RETURN_ID}" },

			{ typeof(RoleModel), $"{INSERT_INTO} {ROLES_TABLE} {PARAM_ROLE_SQL} {RETURN_ID}" },
			{ typeof(TagModel), $"{INSERT_INTO} {TAGS_TABLE} {PARAM_TAG_SQL} {RETURN_ID}" },
			{ typeof(SexualityModel), $"{INSERT_INTO} {SEXUALITIES_TABLE} {PARAM_SEXUALITY_SQL} {RETURN_ID}" },

			{ typeof(PronounsModel), $"{INSERT_INTO} {PRONOUNS_TABLE} {PARAM_PRONOUNS_SQL} {RETURN_ID}" },
			{ typeof(SexModel), $"{INSERT_INTO} {SEXES_TABLE} {PARAM_SEX_SQL} {RETURN_ID}" },
			{ typeof(GenderModel), $"{INSERT_INTO} {GENDERS_TABLE} {PARAM_GENDER_SQL} {RETURN_ID}" },
			{ typeof(RegionModel), $"{INSERT_INTO} {REGIONS_TABLE} {PARAM_REGION_SQL} {RETURN_ID}" },
			{ typeof(CountryModel), $"{INSERT_INTO} {COUNTRIES_TABLE} {PARAM_COUNTRY_SQL} {RETURN_ID}" },

			{ typeof(MatchChoiceModel), $"{INSERT_INTO} {MATCH_CHOICES_TABLE} {PARAM_MATCH_CHOICE_SQL} {RETURN_ID}" },
			{ typeof(MatchModel), $"{INSERT_INTO} {MATCHES_TABLE} {PARAM_MATCH_SQL} {RETURN_ID}" },

			{ typeof(MessageModel), $"{INSERT_INTO} {MESSAGES_TABLE} {PARAM_MESSAGE_SQL} {RETURN_ID}" },

			{ typeof(ReportModel), $"{INSERT_INTO} {REPORTS_TABLE} {PARAM_REPORT_SQL} {RETURN_ID}" },
			{ typeof(ReportReasonModel), $"{INSERT_INTO} {REPORT_REASONS_TABLE} {PARAM_REPORT_REASON_SQL} {RETURN_ID}" },
		});

		// Update
		private static readonly ReadOnlyDictionary<Type, String> UPDATE_BY_ID_DICT = new (new Dictionary<Type, String>()
		{
			{ typeof(AccountModel), $"{UPDATE} {ACCOUNTS_TABLE} {SET} {PARAM_UPDATE_ACCOUNT_SQL} {WHERE} {GetParamAndCondSql(nameof(AccountModel.Id))}" },
			{ typeof(ProfileModel), $"{UPDATE} {PROFILES_TABLE} {SET} {PARAM_UPDATE_PROFILE_SQL} {WHERE} {GetParamAndCondSql(nameof(ProfileModel.Id))}" },
			{ typeof(FilterModel), $"{UPDATE} {FILTERS_TABLE} {SET} {PARAM_UPDATE_FILTER_SQL} {WHERE} {GetParamAndCondSql(nameof(FilterModel.Id))}" },

			{ typeof(RoleModel), $"{UPDATE} {ROLES_TABLE} {SET} {PARAM_UPDATE_ROLE_SQL} {WHERE} {GetParamAndCondSql(nameof(RoleModel.Id))}" },
			{ typeof(TagModel), $"{UPDATE} {TAGS_TABLE} {SET} {PARAM_UPDATE_TAG_SQL} {WHERE} {GetParamAndCondSql(nameof(TagModel.Id))}" },
			{ typeof(SexualityModel), $"{UPDATE} {SEXUALITIES_TABLE} {SET} {PARAM_UPDATE_SEXUALITY_SQL} {WHERE} {GetParamAndCondSql(nameof(SexualityModel.Id))}" },

			{ typeof(PronounsModel), $"{UPDATE} {PRONOUNS_TABLE} {SET} {PARAM_UPDATE_PRONOUNS_SQL} {WHERE} {GetParamAndCondSql(nameof(PronounsModel.Id))}" },
			{ typeof(SexModel), $"{UPDATE} {SEXES_TABLE} {SET} {PARAM_UPDATE_SEX_SQL} {WHERE} {GetParamAndCondSql(nameof(SexModel.Id))}" },
			{ typeof(GenderModel), $"{UPDATE} {GENDERS_TABLE} {SET} {PARAM_UPDATE_GENDER_SQL} {WHERE} {GetParamAndCondSql(nameof(GenderModel.Id))}" },
			{ typeof(RegionModel), $"{UPDATE} {REGIONS_TABLE} {SET} {PARAM_UPDATE_REGION_SQL} {WHERE} {GetParamAndCondSql(nameof(RegionModel.Id))}" },
			{ typeof(CountryModel), $"{UPDATE} {COUNTRIES_TABLE} {SET} {PARAM_UPDATE_COUNTRY_SQL} {WHERE} {GetParamAndCondSql(nameof(CountryModel.Id))}" },

			{ typeof(MatchChoiceModel), $"{UPDATE} {MATCH_CHOICES_TABLE} {SET} {PARAM_UPDATE_MATCH_CHOICE_SQL} {WHERE} {GetParamAndCondSql(nameof(MatchChoiceModel.Id))}" },
			{ typeof(MatchModel), $"{UPDATE} {MATCHES_TABLE} {SET} {PARAM_UPDATE_MATCH_SQL} {WHERE} {GetParamAndCondSql(nameof(MatchModel.Id))}" },

			{ typeof(MessageModel), $"{UPDATE} {MESSAGES_TABLE} {SET} {PARAM_UPDATE_MESSAGE_SQL} {WHERE} {GetParamAndCondSql(nameof(MessageModel.Id))}" },

			{ typeof(ReportModel), $"{UPDATE} {REPORTS_TABLE} {SET} {PARAM_UPDATE_REPORT_SQL} {WHERE} {GetParamAndCondSql(nameof(ReportModel.Id))}" },
			{ typeof(ReportReasonModel), $"{UPDATE} {REPORT_REASONS_TABLE} {SET} {PARAM_UPDATE_REPORT_REASON_SQL} {WHERE} {GetParamAndCondSql(nameof(ReportReasonModel.Id))}" },
		});

		// Delete
		private static readonly ReadOnlyDictionary<Type, String> DELETE_DICT = new (new Dictionary<Type, String>()
		{
			{ typeof(AccountModel), $"{DELETE} {FROM} {ACCOUNTS_TABLE} {WHERE} {GetParamAndCondSql(nameof(AccountModel.Id))}" },
			{ typeof(AccountRoleModel), $"{DELETE} {FROM} {ACCOUNT_ROLES_TABLE} {WHERE} {GetParamAndCondSql(nameof(AccountRoleModel.Id))}" },
			{ typeof(ProfileModel), $"{DELETE} {FROM} {PROFILES_TABLE} {WHERE} {GetParamAndCondSql(nameof(ProfileModel.Id))}" },
			{ typeof(ProfileTagModel), $"{DELETE} {FROM} {PROFILE_TAGS_TABLE} {WHERE} {GetParamAndCondSql(nameof(ProfileTagModel.ProfileId))}" },
			{ typeof(ProfileSexualityModel), $"{DELETE} {FROM} {PROFILE_SEXUALITIES_TABLE} {WHERE} {GetParamAndCondSql(nameof(ProfileSexualityModel.ProfileId))}" },
			{ typeof(FilterModel), $"{DELETE} {FROM} {FILTERS_TABLE} {WHERE} {GetParamAndCondSql(nameof(FilterModel.Id))}" },

			{ typeof(MatchChoiceModel), $"{DELETE} {FROM} {MATCH_CHOICES_TABLE} {WHERE} {GetParamAndCondSql(nameof(MatchChoiceModel.AccountId), nameof(MatchChoiceModel.MatchAccountId))}" },
			{ typeof(MatchModel), $"{DELETE} {FROM} {MATCHES_TABLE} {WHERE} {GetParamAndCondSql(nameof(MatchModel.AccountId), nameof(MatchModel.MatchAccountId))}" },
		});

		private static readonly ReadOnlyDictionary<Type, String> DELETE_MISSING_IDS_DICT = new (new Dictionary<Type, String>()
		{
			{ typeof(SexualityModel), $"{DELETE} {FROM} {SEXUALITIES_TABLE} {WHERE} {nameof(SexualityModel.Id)} {NOT} {IN} @{nameof(IdsModel.Ids)}" },

			{ typeof(PronounsModel), $"{DELETE} {FROM} {PRONOUNS_TABLE} {WHERE} {nameof(PronounsModel.Id)} {NOT} {IN} @{nameof(IdsModel.Ids)}" },
			{ typeof(SexModel), $"{DELETE} {FROM} {SEXES_TABLE} {WHERE} {nameof(SexModel.Id)} {NOT} {IN} @{nameof(IdsModel.Ids)}" },
			{ typeof(GenderModel), $"{DELETE} {FROM} {GENDERS_TABLE} {WHERE} {nameof(GenderModel.Id)} {NOT} {IN} @{nameof(IdsModel.Ids)}" },
			{ typeof(RegionModel), $"{DELETE} {FROM} {REGIONS_TABLE} {WHERE} {nameof(RegionModel.Id)} {NOT} {IN} @{nameof(IdsModel.Ids)}" },
			{ typeof(CountryModel), $"{DELETE} {FROM} {COUNTRIES_TABLE} {WHERE} {nameof(CountryModel.Id)} {NOT} {IN} @{nameof(IdsModel.Ids)}" },
		});

		private static readonly String DELETE_ACCOUNT_REPORTS_SQL =
			$@"{DELETE} {FROM} {REPORTS_TABLE}
			{WHERE} {nameof(ReportModel.ReportedAccountId)} = @{nameof(AccountModel.Id)}";
		#endregion
		#region Fields
		private readonly ILogger _Logger;
		private readonly String _ConnectionString;
		#endregion

		//===========================================================================================
		// Private Static Methods
		//===========================================================================================
		private static String[] GetParamEqualsArray(params String[] InParamNames)
		{
			var ParamEqualsArray = new String[InParamNames.Length];
			for (var Index = 0; Index < InParamNames.Length; Index++) ParamEqualsArray[Index] = $"{InParamNames[Index]} = @{InParamNames[Index]}";
			return ParamEqualsArray;
		}
		private static String GetParamSql<T>(Boolean InIsUpdate)
		{
			var ParamNames = typeof(T).GetProperties().Select(X => X.Name).Where(X => X != ID_NAME).ToArray();
			return GetParamSql(InIsUpdate, ParamNames);
		}
		private static String GetParamSql(Boolean InIsUpdate, params String[] InParamNames)
		{
			if (!InIsUpdate) return $"({String.Join(",", InParamNames)}) {VALUES} (@{String.Join(",@", InParamNames)})";
			return String.Join(",", GetParamEqualsArray(InParamNames));
		}
		private static String GetParamAndCondSql(params String[] InParamNames) => String.Join($" {AND} ", GetParamEqualsArray(InParamNames));

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public DatabaseService(ILogger<DatabaseService> InLogger, IOptions<DatabaseConfiguration> InDatabaseConfig)
		{
			_Logger = InLogger;
			_ConnectionString = InDatabaseConfig.Value.ConnectionString;
		}

		public async Task<Boolean> PerformTransactionAsync(Func<IDbConnection, Task> InFunc)
		{
			if (InFunc == null) throw new ArgumentNullException(nameof(InFunc));

			try
			{
				using (var Connection = new SqliteConnection(_ConnectionString))
				{
					await Connection.OpenAsync();
					using (var Transaction = await Connection.BeginTransactionAsync())
					{
						await InFunc(Connection);
						await Transaction.CommitAsync();
						return true;
					}
				}
			}

			catch (Exception Ex)
			{
				// TODO: handle spam
				_Logger.LogError(Ex, Ex.Message);
				return false;
			}
		}
		public async Task<T> PerformTransactionAsync<T>(Func<IDbConnection, Task<T>> InFunc)
		{
			if (InFunc == null) throw new ArgumentNullException(nameof(InFunc));

			try
			{
				using (var Connection = new SqliteConnection(_ConnectionString))
				{
					await Connection.OpenAsync();
					using (var Transaction = await Connection.BeginTransactionAsync())
					{
						var Result = await InFunc(Connection);
						await Transaction.CommitAsync();
						return Result;
					}
				}
			}

			catch (Exception Ex)
			{
				// TODO: handle spam
				_Logger.LogError(Ex, Ex.Message);
				return default;
			}
		}

		// Check
		public async Task<Boolean> CheckModelByIdAsync<T>(T InModel, IDbConnection InConnection) => await InConnection.ExecuteScalarAsync<Boolean>(CHECK_BY_ID_DICT[typeof(T)], InModel);

		public async Task<Boolean> CheckAccountPublicKeyAsync(String InPublicKey, IDbConnection InConnection) => await InConnection.ExecuteScalarAsync<Boolean>(CHECK_ACCOUNT_PUBLIC_KEY_SQL, new AccountModel { PublicKey_Base64 = InPublicKey });
		public async Task<Boolean> CheckAccountPublicIdAsync(String InPublicId, IDbConnection InConnection) => await InConnection.ExecuteScalarAsync<Boolean>(CHECK_ACCOUNT_PUBLIC_ID_SQL, new AccountModel { PublicId = InPublicId });
		public async Task<Boolean> CheckAccountHasMatch(AccountModel InAccount, IDbConnection InConnection) => await InConnection.ExecuteScalarAsync<Boolean>(CHECK_ACCOUNT_HAS_MATCH_SQL, InAccount);

		// Get
		public async Task<IEnumerable<T>> GetAllAsync<T>(IDbConnection InConnection) => await InConnection.QueryAsync<T>(GET_ALL_DICT[typeof(T)]);
		public async Task<T> GetModelByIdAsync<T>(T InModel, IDbConnection InConnection) => await InConnection.QueryFirstOrDefaultAsync<T>(GET_BY_ID_DICT[typeof(T)], InModel);

		public async Task<Int32> GetTotalActiveAccountsAsync(IDbConnection InConnection) => await InConnection.ExecuteScalarAsync<Int32>(GET_TOTAL_ACTIVE_ACCOUNTS_SQL);
		public async Task<Int32> GetTotalActiveMatchesAsync(IDbConnection InConnection) => await InConnection.ExecuteScalarAsync<Int32>(GET_TOTAL_ACTIVE_MATCHES_SQL);
		public async Task<AccountModel> GetAccountByPublicKeyAsync(String InPublicKey, IDbConnection InConnection) => await InConnection.QueryFirstOrDefaultAsync<AccountModel>(GET_ACCOUNT_BY_PUBLIC_KEY_SQL, new AccountModel { PublicKey_Base64 = InPublicKey });
		public async Task<AccountModel> GetAccountByPublicIdAsync(String InPublicId, IDbConnection InConnection) => await InConnection.QueryFirstOrDefaultAsync<AccountModel>(GET_ACCOUNT_BY_PUBLIC_ID_SQL, new AccountModel { PublicId = InPublicId });
		public async Task<TagModel> GetTagByNameAsync(String InName, IDbConnection InConnection) => await InConnection.QueryFirstOrDefaultAsync<TagModel>(GET_TAG_BY_NAME_SQL, new TagModel { Name = InName });
		public async Task<IEnumerable<RoleModel>> GetAccountRolesAsync(AccountModel InAccount, IDbConnection InConnection)
		{
			var AccountRoles = await InConnection.QueryAsync<AccountRoleModel>(GET_ACCOUNT_ROLES_SQL, InAccount);
			var Roles = new List<RoleModel>();

			foreach (var AccountRole in AccountRoles)
			{
				var Role = await GetModelByIdAsync<RoleModel>(new RoleModel { Id = AccountRole.RoleId }, InConnection);
				if (Role != null) Roles.Add(Role);
			}

			return Roles;
		}
		public async Task<IEnumerable<TagModel>> GetProfileTagsAsync(ProfileModel InProfile, IDbConnection InConnection)
		{
			var ProfileTags = await InConnection.QueryAsync<ProfileTagModel>(GET_PROFILE_TAGS_SQL, InProfile);
			var Tags = new List<TagModel>();

			foreach (var ProfileTag in ProfileTags)
			{
				var Tag = await GetModelByIdAsync<TagModel>(new TagModel { Id = ProfileTag.TagId }, InConnection);
				if (Tag != null) Tags.Add(Tag);
			}

			return Tags;
		}
		public async Task<IEnumerable<SexualityModel>> GetProfileSexualitiesAsync(ProfileModel InProfile, IDbConnection InConnection)
		{
			var ProfileSexualities = await InConnection.QueryAsync<ProfileSexualityModel>(GET_PROFILE_SEXUALITIES_SQL, InProfile);
			var Sexualities = new List<SexualityModel>();

			foreach (var ProfileSexuality in ProfileSexualities)
			{
				var Sexuality = await GetModelByIdAsync<SexualityModel>(new SexualityModel { Id = ProfileSexuality.SexualityId }, InConnection);
				if (Sexuality != null) Sexualities.Add(Sexuality);
			}

			return Sexualities;
		}
		public async Task<MatchChoiceModel> GetMatchChoiceAsync(MatchChoiceModel InMatchChoice, IDbConnection InConnection) => await InConnection.QueryFirstOrDefaultAsync<MatchChoiceModel>(GET_MATCH_CHOICE_SQL, InMatchChoice);
		public async Task<Int32> GetTotalMatchChoicesAsync(GetMatchChoicesModel InModel, IDbConnection InConnection) => await InConnection.ExecuteScalarAsync<Int32>(GET_TOTAL_MATCH_CHOICES_SQL, InModel);
		public async Task<IEnumerable<MatchChoiceModel>> GetMatchChoicesAsync(GetMatchChoicesModel InModel, IDbConnection InConnection) => await InConnection.QueryAsync<MatchChoiceModel>(GET_MATCH_CHOICES_SQL, InModel);
		public async Task<IEnumerable<MatchModel>> GetAllMatchesAsync(AccountModel InAccount, IDbConnection InConnection) => await InConnection.QueryAsync<MatchModel>(GET_ALL_MATCHES_SQL, InAccount);
		public async Task<ReportModel> GetReportAsync(ReportModel InReport, IDbConnection InConnection) => await InConnection.QueryFirstOrDefaultAsync<ReportModel>(GET_REPORT_SQL, InReport);
		public async Task<Int32> GetPreviousMessageCountAsync(ChatGetHistoryRequestModel InRequestModel, IDbConnection InConnection) => await InConnection.ExecuteScalarAsync<Int32>(GET_PREVIOUS_MESSAGE_COUNT_SQL, InRequestModel);
		public async Task<Int32> GetFollowingMessageCountAsync(ChatGetHistoryRequestModel InRequestModel, IDbConnection InConnection) => await InConnection.ExecuteScalarAsync<Int32>(GET_FOLLOWING_MESSAGE_COUNT_SQL, InRequestModel);
		public async Task<IEnumerable<MessageModel>> GetPreviousMessagesAsync(ChatGetHistoryRequestModel InRequestModel, IDbConnection InConnection) => await InConnection.QueryAsync<MessageModel>(GET_PREVIOUS_MESSAGES_SQL, InRequestModel);
		public async Task<IEnumerable<MessageModel>> GetFollowingMessagesAsync(ChatGetHistoryRequestModel InRequestModel, IDbConnection InConnection) => await InConnection.QueryAsync<MessageModel>(GET_FOLLOWING_MESSAGES_SQL, InRequestModel);
		public async Task<AccountModel> GetNewMatchAsync(GetNewMatchModel InModel, IDbConnection InConnection) => await InConnection.QueryFirstOrDefaultAsync<AccountModel>(GET_NEW_MATCH_SQL, InModel);
		public async Task<Int32> GetTotalReportedAccounts(IDbConnection InConnection) => await InConnection.ExecuteScalarAsync<Int32>(GET_TOTAL_REPORTED_ACCOUNTS_SQL);
		public async Task<IEnumerable<ReportedAccountModel>> GetReportedAccountsAsync(AdminGetReportedProfilesRequestModel InRequest, IDbConnection InConnection) => await InConnection.QueryAsync<Models.ReportedAccountModel>(GET_REPORTED_ACCOUNTS_SQL, InRequest);

		// Insert
		public async Task<Int32> InsertModelsAsync<T>(IEnumerable<T> InModels, IDbConnection InConnection) => await InConnection.ExecuteAsync(INSERT_DICT[typeof(T)], InModels);
		public async Task<Int32> InsertModelAsync<T>(T InModel, IDbConnection InConnection) => await InConnection.ExecuteAsync(INSERT_DICT[typeof(T)], InModel);
		public async Task<Int32> InsertModelReturnIdAsync<T>(T InModel, IDbConnection InConnection) => await InConnection.ExecuteScalarAsync<Int32>(INSERT_RETURN_ID_DICT[typeof(T)], InModel);

		// Update
		public async Task<Int32> UpdateModelByIdAsync<T>(T InModel, IDbConnection InConnection) => await InConnection.ExecuteAsync(UPDATE_BY_ID_DICT[typeof(T)], InModel);
		public async Task<Int32> UpdateAccountActiveAsync(AccountModel InAccount, IDbConnection InConnection)
		{
			InAccount.LastActive = DateTime.UtcNow;
			return await UpdateModelByIdAsync(InAccount, InConnection);
		}

		// Delete
		public async Task<Int32> DeleteModelsAsync<T>(IEnumerable<T> InModels, IDbConnection InConnection) => await InConnection.ExecuteAsync(DELETE_DICT[typeof(T)], InModels);
		public async Task<Int32> DeleteModelAsync<T>(T InModel, IDbConnection InConnection) => await InConnection.ExecuteAsync(DELETE_DICT[typeof(T)], InModel);
		public async Task<Int32> DeleteMissingIdsAsync<T>(IdsModel InIds, IDbConnection InConnection) => await InConnection.ExecuteAsync(DELETE_MISSING_IDS_DICT[typeof(T)], InIds);
		public async Task<Int32> DeleteAccountAsync(AccountModel InAccount, IDbConnection InConnection)
		{
			await DeleteModelAsync<ProfileModel>(new ProfileModel { Id = InAccount.ProfileId }, InConnection);
			await DeleteModelAsync<FilterModel>(new FilterModel { Id = InAccount.FilterId }, InConnection);
			return await DeleteModelAsync<AccountModel>(InAccount, InConnection);
		}
		public async Task<Int32> DeleteAccountReportsAsync(AccountModel InAccount, IDbConnection InConnection) => await InConnection.ExecuteAsync(DELETE_ACCOUNT_REPORTS_SQL, InAccount);
	}
}