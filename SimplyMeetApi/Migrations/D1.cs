using System;
using System.Data;
using FluentMigrator;
using SimplyMeetApi.Extensions;
using SimplyMeetShared.Constants;
using SimplyMeetShared.Enums;

namespace SimplyMeetApi.Migrations
{
	[Migration(1)]
	public class D1 : Migration
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Const Fields
		private const String D1_CARDS_TABLE = "CARDS";
		private const String D1_ACCOUNTS_TABLE = "ACCOUNTS";
		private const String D1_ACCOUNT_ROLES_TABLE = "ACCOUNT_ROLES";
		private const String D1_PROFILES_TABLE = "PROFILES";
		private const String D1_PROFILE_TAGS_TABLE = "PROFILE_TAGS";
		private const String D1_PROFILE_SEXUALITIES_TABLE = "PROFILE_SEXUALITIES";
		private const String D1_FILTERS_TABLE = "FILTERS";
		private const String D1_ROLES_TABLE = "ROLES";
		private const String D1_TAGS_TABLE = "TAGS";
		private const String D1_SEXUALITIES_TABLE = "SEXUALITIES";
		private const String D1_PRONOUNS_TABLE = "PRONOUNS";
		private const String D1_SEXES_TABLE = "SEXES";
		private const String D1_GENDERS_TABLE = "GENDERS";
		private const String D1_REGIONS_TABLE = "REGIONS";
		private const String D1_COUNTRIES_TABLE = "COUNTRIES";
		private const String D1_MATCH_CHOICES_TABLE = "MATCH_CHOICES";
		private const String D1_MATCHES_TABLE = "MATCHES";
		private const String D1_MESSAGES_TABLE = "MESSAGES";
		private const String D1_REPORTS_TABLE = "REPORTS";
		private const String D1_REPORT_REASONS_TABLE = "REPORT_REASONS";
		#endregion

		//===========================================================================================
		// Migration Models
		//===========================================================================================
		internal class D1_CardModel
		{
			public Int32 Id { get; set; }
			public String Title { get; set; }
			public String Content { get; set; }
		}
		internal class D1_AccountModel
		{
			public Int32 Id { get; set; }

			public String PublicId { get; set; }
			public String PublicKey_Base64 { get; set; }
			public DateTime Creation { get; set; }
			public DateTime? LastLogin { get; set; }
			public EAccountStatus Status { get; set; }

			public Int32 ProfileId { get; set; }
			public Int32 FilterId { get; set; }
		}
		internal class D1_AccountRoleModel
		{
			public Int32 Id { get; set; }
			public Int32 RoleId { get; set; }
			public String AccountPublicId { get; set; }
		}
		internal class D1_ProfileModel
		{
			public Int32 Id { get; set; }

			public String Avatar { get; set; }
			public String DisplayName { get; set; }

			public Int32? PronounsId { get; set; }
			public Int32? SexId { get; set; }
			public Int32? GenderId { get; set; }
			public Int32? RegionId { get; set; }
			public Int32? CountryId { get; set; }

			public DateTime? BirthDate { get; set; }

			public ELookingFor LookingFor { get; set; }

			public String AboutMe { get; set; }
			public String AboutYou { get; set; }
		}
		internal class D1_ProfileTagModel
		{
			public Int32 ProfileId { get; set; }
			public Int32 TagId { get; set; }
		}
		internal class D1_ProfileSexualityModel
		{
			public Int32 ProfileId { get; set; }
			public Int32 SexualityId { get; set; }
		}
		internal class D1_FilterModel
		{
			public Int32 Id { get; set; }

			public Int32? PronounsId { get; set; }
			public Int32? SexId { get; set; }
			public Int32? GenderId { get; set; }
			public Int32? RegionId { get; set; }
			public Int32? CountryId { get; set; }

			public Int32 FromAge { get; set; }
			public Int32 ToAge { get; set; }
			public Boolean AgeEnabled { get; set; }
		}
		internal class D1_RoleModel
		{
			public Int32 Id { get; set; }
			public String Name { get; set; }
		}
		internal class D1_TagModel
		{
			public Int32 Id { get; set; }
			public String Name { get; set; }
		}
		internal class D1_SexualityModel : D1_ProfileDataBaseModel
		{
		}
		internal class D1_PronounsModel : D1_ProfileDataBaseModel
		{
		}
		internal class D1_SexModel : D1_ProfileDataIconBaseModel
		{
		}
		internal class D1_GenderModel : D1_ProfileDataBaseModel
		{
		}
		internal class D1_RegionModel : D1_ProfileDataIconBaseModel
		{
		}
		internal class D1_CountryModel : D1_ProfileDataIconBaseModel
		{
			public Int32 RegionId { get; set; }
		}
		internal abstract class D1_ProfileDataIconBaseModel : D1_ProfileDataBaseModel
		{
			public String Icon { get; set; }
		}
		internal abstract class D1_ProfileDataBaseModel
		{
			public Int32 Id { get; set; }
			public String Name { get; set; }
		}
		internal class D1_MatchChoiceModel
		{
			public Int32 Id { get; set; }
			public Int32 AccountId { get; set; }
			public Int32 MatchAccountId { get; set; }
			public EMatchChoice Choice { get; set; }
		}
		internal class D1_MatchModel
		{
			public Int32 Id { get; set; }
			public Int32 AccountId { get; set; }
			public Int32 MatchAccountId { get; set; }
		}
		internal class D1_MessageModel
		{
			public Int32 Id { get; set; }
			public Int32 MatchId { get; set; }
			public String FromPublicId { get; set; }

			public String ServerDataJson { get; set; }
			public String ClientDataJson_Encrypted_Base64 { get; set; }
			public String ClientDataJson_Nonce_Base64 { get; set; }
		}
		internal class D1_ReportModel
		{
			public Int32 Id { get; set; }
			public Int32 ReporterAccountId { get; set; }
			public Int32 ReportedAccountId { get; set; }
			public Int32 ReportReasonId { get; set; }
		}
		internal class D1_ReportReasonModel
		{
			public Int32 Id { get; set; }
			public String Name { get; set; }
		}

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public override void Up()
		{
			Create.Table(D1_CARDS_TABLE)
				.WithIdColumn(nameof(D1_CardModel.Id))
				.WithColumn(nameof(D1_CardModel.Title)).AsString().NotNullable()
				.WithColumn(nameof(D1_CardModel.Content)).AsString().NotNullable()
				;

			Create.Table(D1_ACCOUNTS_TABLE)
				.WithIdColumn(nameof(D1_AccountModel.Id))
				.WithColumn(nameof(D1_AccountModel.PublicId)).AsFixedLengthString(AccountConstants.PUBLIC_ID_LENGTH).NotNullable().Unique()
				.WithColumn(nameof(D1_AccountModel.PublicKey_Base64)).AsFixedLengthString(AccountConstants.PUBLIC_KEY_TEXT_LENGTH).NotNullable().Unique()
				.WithColumn(nameof(D1_AccountModel.Creation)).AsDate().NotNullable()
				.WithColumn(nameof(D1_AccountModel.LastLogin)).AsDate().Nullable()
				.WithColumn(nameof(D1_AccountModel.Status)).AsInt32().NotNullable()
				.WithColumn(nameof(D1_AccountModel.ProfileId)).AsString().NotNullable().Unique()
					// .ForeignKey(nameof(D1_AccountModel.ProfileId), PROFILES_TABLE, nameof(ProfileModel.Id))
				.WithColumn(nameof(D1_AccountModel.FilterId)).AsString().NotNullable().Unique()
					// .ForeignKey(nameof(D1_AccountModel.FilterId), FILTERS_TABLE, nameof(FilterModel.Id))
				;

			Create.Table(D1_ACCOUNT_ROLES_TABLE)
				.WithIdColumn(nameof(D1_AccountRoleModel.Id))
				.WithColumn(nameof(D1_AccountRoleModel.RoleId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_AccountRoleModel.RoleId), D1_ROLES_TABLE, nameof(D1_RoleModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(D1_AccountRoleModel.AccountPublicId)).AsFixedLengthAnsiString(AccountConstants.PUBLIC_ID_LENGTH).NotNullable()
					.ForeignKey(nameof(D1_AccountRoleModel.AccountPublicId), D1_ACCOUNTS_TABLE, nameof(D1_AccountModel.PublicId)).OnDelete(Rule.Cascade)
				;
			Create.UniqueConstraint()
				.OnTable(D1_ACCOUNT_ROLES_TABLE)
				.Columns(nameof(D1_AccountRoleModel.RoleId), nameof(D1_AccountRoleModel.AccountPublicId))
				;

			Create.Table(D1_PROFILES_TABLE)
				.WithIdColumn(nameof(D1_ProfileModel.Id))
				.WithColumn(nameof(D1_ProfileModel.Avatar)).AsString().NotNullable()
				.WithColumn(nameof(D1_ProfileModel.DisplayName)).AsString().NotNullable()
				.WithColumn(nameof(D1_ProfileModel.PronounsId)).AsInt32().Nullable()
					.ForeignKey(nameof(D1_ProfileModel.PronounsId), D1_PRONOUNS_TABLE, nameof(D1_PronounsModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(D1_ProfileModel.SexId)).AsInt32().Nullable()
					.ForeignKey(nameof(D1_ProfileModel.SexId), D1_SEXES_TABLE, nameof(D1_SexModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(D1_ProfileModel.GenderId)).AsInt32().Nullable()
					.ForeignKey(nameof(D1_ProfileModel.GenderId), D1_GENDERS_TABLE, nameof(D1_GenderModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(D1_ProfileModel.RegionId)).AsInt32().Nullable()
					.ForeignKey(nameof(D1_ProfileModel.RegionId), D1_REGIONS_TABLE, nameof(D1_RegionModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(D1_ProfileModel.CountryId)).AsInt32().Nullable()
					.ForeignKey(nameof(D1_ProfileModel.CountryId), D1_COUNTRIES_TABLE, nameof(D1_CountryModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(D1_ProfileModel.BirthDate)).AsDate().Nullable()
				.WithColumn(nameof(D1_ProfileModel.LookingFor)).AsInt32().NotNullable()
				.WithColumn(nameof(D1_ProfileModel.AboutMe)).AsString().Nullable()
				.WithColumn(nameof(D1_ProfileModel.AboutYou)).AsString().Nullable()
				;

			Create.Table(D1_PROFILE_TAGS_TABLE)
				.WithColumn(nameof(D1_ProfileTagModel.ProfileId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_ProfileTagModel.ProfileId), D1_PROFILES_TABLE, nameof(D1_ProfileModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(D1_ProfileTagModel.TagId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_ProfileTagModel.TagId), D1_TAGS_TABLE, nameof(D1_TagModel.Id)).OnDelete(Rule.Cascade)
				;
			Create.UniqueConstraint()
				.OnTable(D1_PROFILE_TAGS_TABLE)
				.Columns(nameof(D1_ProfileTagModel.ProfileId), nameof(D1_ProfileTagModel.TagId))
				;

			Create.Table(D1_PROFILE_SEXUALITIES_TABLE)
				.WithColumn(nameof(D1_ProfileSexualityModel.ProfileId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_ProfileSexualityModel.ProfileId), D1_PROFILES_TABLE, nameof(D1_ProfileModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(D1_ProfileSexualityModel.SexualityId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_ProfileSexualityModel.SexualityId), D1_SEXUALITIES_TABLE, nameof(D1_SexualityModel.Id)).OnDelete(Rule.Cascade)
				;
			Create.UniqueConstraint()
				.OnTable(D1_PROFILE_SEXUALITIES_TABLE)
				.Columns(nameof(D1_ProfileSexualityModel.ProfileId), nameof(D1_ProfileSexualityModel.SexualityId))
				;

			Create.Table(D1_FILTERS_TABLE)
				.WithIdColumn(nameof(D1_FilterModel.Id))
				.WithColumn(nameof(D1_FilterModel.PronounsId)).AsInt32().Nullable()
					.ForeignKey(nameof(D1_FilterModel.PronounsId), D1_PRONOUNS_TABLE, nameof(D1_PronounsModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(D1_FilterModel.SexId)).AsInt32().Nullable()
					.ForeignKey(nameof(D1_FilterModel.SexId), D1_SEXES_TABLE, nameof(D1_SexModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(D1_FilterModel.GenderId)).AsInt32().Nullable()
					.ForeignKey(nameof(D1_FilterModel.GenderId), D1_GENDERS_TABLE, nameof(D1_GenderModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(D1_FilterModel.RegionId)).AsInt32().Nullable()
					.ForeignKey(nameof(D1_FilterModel.RegionId), D1_REGIONS_TABLE, nameof(D1_RegionModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(D1_FilterModel.CountryId)).AsInt32().Nullable()
					.ForeignKey(nameof(D1_FilterModel.CountryId), D1_COUNTRIES_TABLE, nameof(D1_CountryModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(D1_FilterModel.FromAge)).AsInt32().Nullable()
				.WithColumn(nameof(D1_FilterModel.ToAge)).AsInt32().Nullable()
				.WithColumn(nameof(D1_FilterModel.AgeEnabled)).AsBoolean().NotNullable()
				;

			Create.Table(D1_ROLES_TABLE)
				.WithIdColumn(nameof(D1_RoleModel.Id))
				.WithColumn(nameof(D1_RoleModel.Name)).AsString().NotNullable().Unique()
				;

			Create.Table(D1_TAGS_TABLE)
				.WithIdColumn(nameof(D1_TagModel.Id))
				.WithColumn(nameof(D1_TagModel.Name)).AsString().NotNullable().Unique()
				;

			Create.Table(D1_SEXUALITIES_TABLE)
				.WithIdColumn(nameof(D1_SexualityModel.Id))
				.WithColumn(nameof(D1_SexualityModel.Name)).AsString().NotNullable().Unique()
				;

			Create.Table(D1_PRONOUNS_TABLE)
				.WithIdColumn(nameof(D1_PronounsModel.Id))
				.WithColumn(nameof(D1_PronounsModel.Name)).AsString().NotNullable().Unique()
				;

			Create.Table(D1_SEXES_TABLE)
				.WithIdColumn(nameof(D1_SexModel.Id))
				.WithColumn(nameof(D1_SexModel.Name)).AsString().NotNullable().Unique()
				.WithColumn(nameof(D1_SexModel.Icon)).AsString().NotNullable()
				;

			Create.Table(D1_GENDERS_TABLE)
				.WithIdColumn(nameof(D1_GenderModel.Id))
				.WithColumn(nameof(D1_GenderModel.Name)).AsString().NotNullable().Unique()
				;

			Create.Table(D1_REGIONS_TABLE)
				.WithIdColumn(nameof(D1_RegionModel.Id))
				.WithColumn(nameof(D1_RegionModel.Name)).AsString().NotNullable().Unique()
				.WithColumn(nameof(D1_RegionModel.Icon)).AsString().NotNullable()
				;

			Create.Table(D1_COUNTRIES_TABLE)
				.WithIdColumn(nameof(D1_CountryModel.Id))
				.WithColumn(nameof(D1_CountryModel.RegionId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_CountryModel.RegionId), D1_REGIONS_TABLE, nameof(D1_RegionModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(D1_CountryModel.Name)).AsString().NotNullable().Unique()
				.WithColumn(nameof(D1_CountryModel.Icon)).AsString().NotNullable()
				;

			Create.Table(D1_MATCH_CHOICES_TABLE)
				.WithIdColumn(nameof(D1_MatchChoiceModel.Id))
				.WithColumn(nameof(D1_MatchChoiceModel.AccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_MatchChoiceModel.AccountId), D1_ACCOUNTS_TABLE, nameof(D1_AccountModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(D1_MatchChoiceModel.MatchAccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_MatchChoiceModel.MatchAccountId), D1_ACCOUNTS_TABLE, nameof(D1_AccountModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(D1_MatchChoiceModel.Choice)).AsInt32().NotNullable()
				;
			Create.UniqueConstraint()
				.OnTable(D1_MATCH_CHOICES_TABLE)
				.Columns(nameof(D1_MatchChoiceModel.AccountId), nameof(D1_MatchChoiceModel.MatchAccountId))
				;

			Create.Table(D1_MATCHES_TABLE)
				.WithIdColumn(nameof(D1_MatchModel.Id))
				.WithColumn(nameof(D1_MatchModel.AccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_MatchModel.AccountId), D1_ACCOUNTS_TABLE, nameof(D1_AccountModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(D1_MatchModel.MatchAccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_MatchModel.MatchAccountId), D1_ACCOUNTS_TABLE, nameof(D1_AccountModel.Id)).OnDelete(Rule.Cascade)
				;
			Create.UniqueConstraint()
				.OnTable(D1_MATCHES_TABLE)
				.Columns(nameof(D1_MatchModel.AccountId), nameof(D1_MatchModel.MatchAccountId))
				;

			Create.Table(D1_MESSAGES_TABLE)
				.WithIdColumn(nameof(D1_MessageModel.Id))
				.WithColumn(nameof(D1_MessageModel.MatchId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_MessageModel.MatchId), D1_MATCHES_TABLE, nameof(D1_MatchModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(D1_MessageModel.FromPublicId)).AsFixedLengthAnsiString(AccountConstants.PUBLIC_ID_LENGTH).NotNullable()
					.ForeignKey(nameof(D1_MessageModel.FromPublicId), D1_ACCOUNTS_TABLE, nameof(D1_AccountModel.PublicId)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(D1_MessageModel.ServerDataJson)).AsString().NotNullable()
				.WithColumn(nameof(D1_MessageModel.ClientDataJson_Encrypted_Base64)).AsString().NotNullable()
				.WithColumn(nameof(D1_MessageModel.ClientDataJson_Nonce_Base64)).AsString().NotNullable()
				;

			Create.Table(D1_REPORTS_TABLE)
				.WithIdColumn(nameof(D1_ReportModel.Id))
				.WithColumn(nameof(D1_ReportModel.ReporterAccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_ReportModel.ReporterAccountId), D1_ACCOUNTS_TABLE, nameof(D1_AccountModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(D1_ReportModel.ReportedAccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_ReportModel.ReportedAccountId), D1_ACCOUNTS_TABLE, nameof(D1_AccountModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(D1_ReportModel.ReportReasonId)).AsInt32().NotNullable()
					.ForeignKey(nameof(D1_ReportModel.ReportReasonId), D1_REPORT_REASONS_TABLE, nameof(D1_ReportReasonModel.Id)).OnDelete(Rule.Cascade)
				;
			Create.UniqueConstraint()
				.OnTable(D1_REPORTS_TABLE)
				.Columns(nameof(D1_ReportModel.ReporterAccountId), nameof(D1_ReportModel.ReportedAccountId))
				;

			Create.Table(D1_REPORT_REASONS_TABLE)
				.WithIdColumn(nameof(D1_ReportReasonModel.Id))
				.WithColumn(nameof(D1_ReportReasonModel.Name)).AsString().NotNullable().Unique()
				;

			InsertDefaults_Fluent();
		}
		public override void Down()
		{
			Delete.Table(D1_CARDS_TABLE);

			Delete.Table(D1_ACCOUNTS_TABLE);
			Delete.Table(D1_ACCOUNT_ROLES_TABLE);
			Delete.Table(D1_PROFILES_TABLE);
			Delete.Table(D1_PROFILE_TAGS_TABLE);
			Delete.Table(D1_PROFILE_SEXUALITIES_TABLE);
			Delete.Table(D1_FILTERS_TABLE);

			Delete.Table(D1_ROLES_TABLE);
			Delete.Table(D1_TAGS_TABLE);
			Delete.Table(D1_SEXUALITIES_TABLE);

			Delete.Table(D1_PRONOUNS_TABLE);
			Delete.Table(D1_SEXES_TABLE);
			Delete.Table(D1_GENDERS_TABLE);
			Delete.Table(D1_REGIONS_TABLE);
			Delete.Table(D1_COUNTRIES_TABLE);

			Delete.Table(D1_MATCH_CHOICES_TABLE);
			Delete.Table(D1_MATCHES_TABLE);

			Delete.Table(D1_MESSAGES_TABLE);

			Delete.Table(D1_REPORTS_TABLE);
			Delete.Table(D1_REPORT_REASONS_TABLE);
		}

		//===========================================================================================
		// Private Methods
		//===========================================================================================
		private void InsertDefaults_Fluent()
		{
			// Cards
			var IdCounter = 0;
			Insert.IntoTable(CARDS_TABLE).Row(new CardModel { Id = ++IdCounter, Title = "Hiki Meet", Content = "A place to meet <a href=\"https://en.wikipedia.org/wiki/Hikikomori\">Hikikomori</a>, whether you just need someone to talk, want to make a new friend or looking to find something more romantic." });
			Insert.IntoTable(CARDS_TABLE).Row(new CardModel { Id = ++IdCounter, Title = "Simple", Content = "Generate a new account and get started with just 1 single click. You can start matching others right afterwards, but you might want to fill out some of your profile fields first to get better matches." });
			Insert.IntoTable(CARDS_TABLE).Row(new CardModel { Id = ++IdCounter, Title = "Private", Content = "Every field in your profile is optional. Your messages are end to end encrypted so that their content can only be read by you and your match. Delete your account at any point in time, together with all of your data. Check out our simple <a href=\"privacy\">Privacy Policy</a> for more information." });
			Insert.IntoTable(CARDS_TABLE).Row(new CardModel { Id = ++IdCounter, Title = "FOSS", Content = "Powered by free and open source software. No advertisement, no feature pay walls. You are <strong>NOT</strong> the product. Find out more about the project on the <a href=\"https://github.com/yalohi/SimplyMeet\">SimplyMeet Github</a>." });
			Insert.IntoTable(CARDS_TABLE).Row(new CardModel { Id = ++IdCounter, Title = "A simple plea", Content = "Treat others like you want to be treated yourself. Be kind and honest. Be yourself. Remember that we are all in this together. It is never easy to find someone who really clicks with you on a personal level. Let's try our best to find the right person for each of us, together!" });

			// Roles
			IdCounter = 0;
			var RoleNames = Enum.GetNames(typeof(EAccountRole));
			foreach (var RoleName in RoleNames) Insert.IntoTable(D1_ROLES_TABLE).Row(new D1_RoleModel { Id = ++IdCounter, Name = RoleName });

			// Sexualities
			IdCounter = 0;
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Abroromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Abrosexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Androgyneromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Androgynesexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Androromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Androsexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Aromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Asexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Biromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Bisexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Ceteroromantic/Skolioromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Ceterosexual/Skoliosexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Demiromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Demisexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Finromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Finsexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Greyromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Greysexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Gyneromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Gynesexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Heteroromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Heterosexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Homoromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Homosexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Omniromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Omnisexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Other" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Panromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Pansexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Polyromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Polysexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Pomoromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Pomosexual" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Queer" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Questioning" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Sapioromantic" });
			Insert.IntoTable(D1_SEXUALITIES_TABLE).Row(new D1_SexualityModel { Id = ++IdCounter, Name = "Sapiosexual" });

			// Pronouns
			IdCounter = 0;
			Insert.IntoTable(D1_PRONOUNS_TABLE).Row(new D1_PronounsModel { Id = ++IdCounter, Name = "He/Him" });
			Insert.IntoTable(D1_PRONOUNS_TABLE).Row(new D1_PronounsModel { Id = ++IdCounter, Name = "She/Her" });
			Insert.IntoTable(D1_PRONOUNS_TABLE).Row(new D1_PronounsModel { Id = ++IdCounter, Name = "They/Them" });

			// Sexes
			IdCounter = 0;
			Insert.IntoTable(D1_SEXES_TABLE).Row(new D1_SexModel { Id = ++IdCounter, Name = "Female", Icon = "♀" });
			Insert.IntoTable(D1_SEXES_TABLE).Row(new D1_SexModel { Id = ++IdCounter, Name = "Male", Icon = "♂" });
			Insert.IntoTable(D1_SEXES_TABLE).Row(new D1_SexModel { Id = ++IdCounter, Name = "Transsexual (F2M)", Icon = "⚧" });
			Insert.IntoTable(D1_SEXES_TABLE).Row(new D1_SexModel { Id = ++IdCounter, Name = "Transsexual (M2F)", Icon = "⚧" });

			// Genders
			IdCounter = 0;
			Insert.IntoTable(D1_GENDERS_TABLE).Row(new D1_GenderModel { Id = ++IdCounter, Name = "Cisgender" });
			Insert.IntoTable(D1_GENDERS_TABLE).Row(new D1_GenderModel { Id = ++IdCounter, Name = "Non-Binary" });
			Insert.IntoTable(D1_GENDERS_TABLE).Row(new D1_GenderModel { Id = ++IdCounter, Name = "Transgender" });

			// Regions
			IdCounter = 0;

			var Africa = new D1_RegionModel { Id = ++IdCounter, Name = "Africa", Icon = "🌍" };
			Insert.IntoTable(D1_REGIONS_TABLE).Row(Africa);
			var Antarctica = new D1_RegionModel { Id = ++IdCounter, Name = "Antarctica", Icon = "🇦🇶" };
			Insert.IntoTable(D1_REGIONS_TABLE).Row(Antarctica);
			var Asia = new D1_RegionModel { Id = ++IdCounter, Name = "Asia", Icon = "🌏" };
			Insert.IntoTable(D1_REGIONS_TABLE).Row(Asia);
			var Europe = new D1_RegionModel { Id = ++IdCounter, Name = "Europe", Icon = "🇪🇺" };
			Insert.IntoTable(D1_REGIONS_TABLE).Row(Europe);
			var NorthAmerica = new D1_RegionModel { Id = ++IdCounter, Name = "North America", Icon = "🌎" };
			Insert.IntoTable(D1_REGIONS_TABLE).Row(NorthAmerica);
			var Oceania = new D1_RegionModel { Id = ++IdCounter, Name = "Oceania", Icon = "🌏" };
			Insert.IntoTable(D1_REGIONS_TABLE).Row(Oceania);
			var SouthAmerica = new D1_RegionModel { Id = ++IdCounter, Name = "South America", Icon = "🌎" };
			Insert.IntoTable(D1_REGIONS_TABLE).Row(SouthAmerica);

			// Countries
			IdCounter = 0;

			// => Africa
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Algeria", Icon = "🇩🇿", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Angola", Icon = "🇦🇴", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Benin", Icon = "🇧🇯", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Botswana", Icon = "🇧🇼", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Burkina Faso", Icon = "🇧🇫", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Burundi", Icon = "🇧🇮", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Cameroon", Icon = "🇨🇲", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Cape Verde", Icon = "🇨🇻", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Central African Republic", Icon = "🇨🇫", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Chad", Icon = "🇹🇩", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Comoros", Icon = "🇰🇲", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Congo - Brazzaville", Icon = "🇨🇬", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Congo - Kinshasa", Icon = "🇨🇩", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Côte d'Ivoire", Icon = "🇨🇮", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Djibouti", Icon = "🇩🇯", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Equatorial Guinea", Icon = "🇬🇶", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Egypt", Icon = "🇪🇬", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Eritrea", Icon = "🇪🇷", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Ethiopia", Icon = "🇪🇹", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Gabon", Icon = "🇬🇦", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Gambia", Icon = "🇬🇲", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Ghana", Icon = "🇬🇭", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Guinea", Icon = "🇬🇳", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Guinea-Bissau", Icon = "🇬🇼", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Kenya", Icon = "🇰🇪", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Lesotho", Icon = "🇱🇸", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Liberia", Icon = "🇱🇷", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Libya", Icon = "🇱🇾", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Madagascar", Icon = "🇲🇬", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Malawi", Icon = "🇲🇼", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Mali", Icon = "🇲🇱", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Mauritania", Icon = "🇲🇷", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Mauritius", Icon = "🇲🇺", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Morocco", Icon = "🇲🇦", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Mozambique", Icon = "🇲🇿", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Namibia", Icon = "🇳🇦", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Niger", Icon = "🇳🇪", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Nigeria", Icon = "🇳🇬", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Rwanda", Icon = "🇷🇼", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "São Tomé & Príncipe", Icon = "🇸🇹", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Senegal", Icon = "🇸🇳", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Seychelles", Icon = "🇸🇨", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Sierra Leone", Icon = "🇸🇱", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Somalia", Icon = "🇸🇴", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "South Africa", Icon = "🇿🇦", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "South Sudan", Icon = "🇸🇸", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Sudan", Icon = "🇸🇩", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Eswatini", Icon = "🇸🇿", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Tanzania", Icon = "🇹🇿", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Togo", Icon = "🇹🇬", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Tunisia", Icon = "🇹🇳", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Uganda", Icon = "🇺🇬", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Western Sahara", Icon = "🇪🇭", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Zambia", Icon = "🇿🇲", RegionId = Africa.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Zimbabwe", Icon = "🇿🇼", RegionId = Africa.Id });

			// => Asia
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Afghanistan", Icon = "🇦🇫", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Armenia", Icon = "🇦🇲", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Azerbaijan", Icon = "🇦🇿", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Bahrain", Icon = "🇧🇭", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Bangladesh", Icon = "🇧🇩", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Bhutan", Icon = "🇧🇹", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Brunei", Icon = "🇧🇳", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Cambodia", Icon = "🇰🇭", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "China", Icon = "🇨🇳", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Timor Leste", Icon = "🇹🇱", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Georgia (Asia)", Icon = "🇬🇪", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "India", Icon = "🇮🇳", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Indonesia", Icon = "🇮🇩", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Iran", Icon = "🇮🇷", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Iraq", Icon = "🇮🇶", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Israel", Icon = "🇮🇱", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Japan", Icon = "🇯🇵", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Jordan", Icon = "🇯🇴", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Kazakhstan", Icon = "🇰🇿", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Kuwait", Icon = "🇰🇼", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Kyrgyzstan", Icon = "🇰🇬", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Laos", Icon = "🇱🇦", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Lebanon", Icon = "🇱🇧", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Malaysia", Icon = "🇲🇾", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Maldives", Icon = "🇲🇻", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Mongolia", Icon = "🇲🇳", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Myanmar", Icon = "🇲🇲", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Nepal", Icon = "🇳🇵", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "North Korea", Icon = "🇰🇵", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Oman", Icon = "🇴🇲", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Pakistan", Icon = "🇵🇰", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Palestinian Territories", Icon = "🇵🇸", RegionId = Asia.Id }); // TODO: ?
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Philippines", Icon = "🇵🇭", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Qatar", Icon = "🇶🇦", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Russia (Asia)", Icon = "🇷🇺", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Saudi Arabia", Icon = "🇸🇦", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Singapore", Icon = "🇸🇬", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "South Korea", Icon = "🇰🇷", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Sri Lanka", Icon = "🇱🇰", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Syria", Icon = "🇸🇾", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Tajikistan", Icon = "🇹🇯", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Thailand", Icon = "🇹🇭", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Turkey (Asia)", Icon = "🇹🇷", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Turkmenistan", Icon = "🇹🇲", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Taiwan", Icon = "🇹🇼", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "United Arab Emirates", Icon = "🇦🇪", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Uzbekistan", Icon = "🇺🇿", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Vietnam", Icon = "🇻🇳", RegionId = Asia.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Yemen", Icon = "🇾🇪", RegionId = Asia.Id });

			// => Europe
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Albania", Icon = "🇦🇱", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Andorra", Icon = "🇦🇩", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Austria", Icon = "🇦🇹", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Belarus", Icon = "🇧🇾", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Belgium", Icon = "🇧🇪", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Bosnia & Herzegovina", Icon = "🇧🇦", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Bulgaria", Icon = "🇧🇬", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Croatia", Icon = "🇭🇷", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Cyprus", Icon = "🇨🇾", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Czechia", Icon = "🇨🇿", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Denmark", Icon = "🇩🇰", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Estonia", Icon = "🇪🇪", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Finland", Icon = "🇫🇮", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "France", Icon = "🇫🇷", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Georgia (Europe)", Icon = "🇬🇪", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Germany", Icon = "🇩🇪", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Greece", Icon = "🇬🇷", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Hungary", Icon = "🇭🇺", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Iceland", Icon = "🇮🇸", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Ireland", Icon = "🇮🇪", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Italy", Icon = "🇮🇹", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Kosovo", Icon = "🇽🇰", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Latvia", Icon = "🇱🇻", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Liechtenstein", Icon = "🇱🇮", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Lithuania", Icon = "🇱🇹", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Luxembourg", Icon = "🇱🇺", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "North Macedonia", Icon = "🇲🇰", RegionId = Europe.Id });

			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Malta", Icon = "🇲🇹", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Moldova", Icon = "🇲🇩", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Monaco", Icon = "🇲🇨", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Montenegro", Icon = "🇲🇪", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Netherlands", Icon = "🇳🇱", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Norway", Icon = "🇳🇴", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Poland", Icon = "🇵🇱", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Portugal", Icon = "🇵🇹", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Romania", Icon = "🇷🇴", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Russia (Europe)", Icon = "🇷🇺", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "San Marino", Icon = "🇸🇲", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Serbia", Icon = "🇷🇸", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Slovakia", Icon = "🇸🇰", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Slovenia", Icon = "🇸🇮", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Spain", Icon = "🇪🇸", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Sweden", Icon = "🇸🇪", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Switzerland", Icon = "🇨🇭", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Turkey (Europe)", Icon = "🇹🇷", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Ukraine", Icon = "🇺🇦", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "United Kingdom", Icon = "🇬🇧", RegionId = Europe.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Vatican City", Icon = "🇻🇦", RegionId = Europe.Id });

			// => North America
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Canada", Icon = "🇨🇦", RegionId = NorthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Greenland", Icon = "🇬🇱", RegionId = NorthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Mexico", Icon = "🇲🇽", RegionId = NorthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "United States", Icon = "🇺🇸", RegionId = NorthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Navassa Island", Icon = "🇺🇸", RegionId = NorthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Puerto Rico", Icon = "🇵🇷", RegionId = NorthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "U.S. Virgin Islands", Icon = "🇻🇮", RegionId = NorthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Guam", Icon = "🇬🇺", RegionId = NorthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "American Samoa", Icon = "🇦🇸", RegionId = NorthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Dominican Republic", Icon = "🇩🇴", RegionId = NorthAmerica.Id });

			// => Oceania
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Australia", Icon = "🇦🇺", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Fiji", Icon = "🇫🇯", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "New Zealand", Icon = "🇳🇿", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Micronesia", Icon = "🇫🇲", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Kiribati", Icon = "🇰🇮", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Marshall Islands", Icon = "🇲🇭", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Nauru", Icon = "🇳🇷", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Palau", Icon = "🇵🇼", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Papua New Guinea", Icon = "🇵🇬", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Samoa", Icon = "🇼🇸", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Solomon Islands", Icon = "🇸🇧", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Tonga", Icon = "🇹🇴", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Tuvalu", Icon = "🇹🇻", RegionId = Oceania.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Vanuatu", Icon = "🇻🇺", RegionId = Oceania.Id });

			// => South America
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Brazil", Icon = "🇧🇷", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Argentina", Icon = "🇦🇷", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Bolivia", Icon = "🇧🇴", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Chile", Icon = "🇨🇱", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Colombia", Icon = "🇨🇴", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Ecuador", Icon = "🇪🇨", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Falkland Islands", Icon = "🇫🇰", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "French Guiana", Icon = "🇬🇫", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Guyana", Icon = "🇬🇾", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Paraguay", Icon = "🇵🇾", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Peru", Icon = "🇵🇪", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "South Georgia & South Sandwich Islands", Icon = "🇬🇸", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Suriname", Icon = "🇸🇷", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Uruguay", Icon = "🇺🇾", RegionId = SouthAmerica.Id });
			Insert.IntoTable(D1_COUNTRIES_TABLE).Row(new D1_CountryModel { Id = ++IdCounter, Name = "Venezuela", Icon = "🇻🇪", RegionId = SouthAmerica.Id });

			// ReportReasons
			IdCounter = 0;
			Insert.IntoTable(D1_REPORT_REASONS_TABLE).Row(new D1_ReportReasonModel { Id = ++IdCounter, Name = "InappropriateProfile" });
		}
	}
}