using System;
using System.Data;
using FluentMigrator;
using SimplyMeetApi.Extensions;
using SimplyMeetShared.Constants;
using SimplyMeetShared.Models;

using static SimplyMeetApi.Constants.DatabaseConstants;

namespace SimplyMeetApi.Migrations
{
	[Migration(1)]
	public class D1 : Migration
	{
		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public override void Up()
		{
			Create.Table(CARDS_TABLE)
				.WithIdColumn(nameof(CardModel.Id))
				.WithColumn(nameof(CardModel.Title)).AsString().NotNullable()
				.WithColumn(nameof(CardModel.Content)).AsString().NotNullable()
				;

			Create.Table(ACCOUNTS_TABLE)
				.WithIdColumn(nameof(AccountModel.Id))
				.WithColumn(nameof(AccountModel.PublicId)).AsFixedLengthString(AccountConstants.PUBLIC_ID_LENGTH).NotNullable().Unique()
				.WithColumn(nameof(AccountModel.PublicKey_Base64)).AsFixedLengthString(AccountConstants.PUBLIC_KEY_TEXT_LENGTH).NotNullable().Unique()
				.WithColumn(nameof(AccountModel.Creation)).AsDate().NotNullable()
				.WithColumn(nameof(AccountModel.LastLogin)).AsDate().Nullable()
				.WithColumn(nameof(AccountModel.Status)).AsInt32().NotNullable()
				.WithColumn(nameof(AccountModel.ProfileId)).AsString().NotNullable().Unique()
					// .ForeignKey(nameof(AccountModel.ProfileId), PROFILES_TABLE, nameof(ProfileModel.Id))
				.WithColumn(nameof(AccountModel.FilterId)).AsString().NotNullable().Unique()
					// .ForeignKey(nameof(AccountModel.FilterId), FILTERS_TABLE, nameof(FilterModel.Id))
				;

			Create.Table(ACCOUNT_ROLES_TABLE)
				.WithIdColumn(nameof(AccountRoleModel.Id))
				.WithColumn(nameof(AccountRoleModel.RoleId)).AsInt32().NotNullable()
					.ForeignKey(nameof(AccountRoleModel.RoleId), ROLES_TABLE, nameof(RoleModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(AccountRoleModel.AccountPublicId)).AsFixedLengthAnsiString(AccountConstants.PUBLIC_ID_LENGTH).NotNullable()
					.ForeignKey(nameof(AccountRoleModel.AccountPublicId), ACCOUNTS_TABLE, nameof(AccountModel.PublicId)).OnDelete(Rule.Cascade)
				;
			Create.UniqueConstraint()
				.OnTable(ACCOUNT_ROLES_TABLE)
				.Columns(nameof(AccountRoleModel.RoleId), nameof(AccountRoleModel.AccountPublicId))
				;

			Create.Table(PROFILES_TABLE)
				.WithIdColumn(nameof(ProfileModel.Id))
				.WithColumn(nameof(ProfileModel.Avatar)).AsString().NotNullable()
				.WithColumn(nameof(ProfileModel.DisplayName)).AsString().NotNullable()
				.WithColumn(nameof(ProfileModel.PronounsId)).AsInt32().Nullable()
					.ForeignKey(nameof(ProfileModel.PronounsId), PRONOUNS_TABLE, nameof(PronounsModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(ProfileModel.SexId)).AsInt32().Nullable()
					.ForeignKey(nameof(ProfileModel.SexId), SEXES_TABLE, nameof(SexModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(ProfileModel.GenderId)).AsInt32().Nullable()
					.ForeignKey(nameof(ProfileModel.GenderId), GENDERS_TABLE, nameof(GenderModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(ProfileModel.RegionId)).AsInt32().Nullable()
					.ForeignKey(nameof(ProfileModel.RegionId), REGIONS_TABLE, nameof(RegionModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(ProfileModel.CountryId)).AsInt32().Nullable()
					.ForeignKey(nameof(ProfileModel.CountryId), COUNTRIES_TABLE, nameof(CountryModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(ProfileModel.BirthDate)).AsDate().Nullable()
				.WithColumn(nameof(ProfileModel.LookingFor)).AsInt32().NotNullable()
				.WithColumn(nameof(ProfileModel.AboutMe)).AsString().Nullable()
				.WithColumn(nameof(ProfileModel.AboutYou)).AsString().Nullable()
				;

			Create.Table(PROFILE_TAGS_TABLE)
				.WithColumn(nameof(ProfileTagModel.ProfileId)).AsInt32().NotNullable()
					.ForeignKey(nameof(ProfileTagModel.ProfileId), PROFILES_TABLE, nameof(ProfileModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(ProfileTagModel.TagId)).AsInt32().NotNullable()
					.ForeignKey(nameof(ProfileTagModel.TagId), TAGS_TABLE, nameof(TagModel.Id)).OnDelete(Rule.Cascade)
				;
			Create.UniqueConstraint()
				.OnTable(PROFILE_TAGS_TABLE)
				.Columns(nameof(ProfileTagModel.ProfileId), nameof(ProfileTagModel.TagId))
				;

			Create.Table(PROFILE_SEXUALITIES_TABLE)
				.WithColumn(nameof(ProfileSexualityModel.ProfileId)).AsInt32().NotNullable()
					.ForeignKey(nameof(ProfileSexualityModel.ProfileId), PROFILES_TABLE, nameof(ProfileModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(ProfileSexualityModel.SexualityId)).AsInt32().NotNullable()
					.ForeignKey(nameof(ProfileSexualityModel.SexualityId), SEXUALITIES_TABLE, nameof(SexualityModel.Id)).OnDelete(Rule.Cascade)
				;
			Create.UniqueConstraint()
				.OnTable(PROFILE_SEXUALITIES_TABLE)
				.Columns(nameof(ProfileSexualityModel.ProfileId), nameof(ProfileSexualityModel.SexualityId))
				;

			Create.Table(FILTERS_TABLE)
				.WithIdColumn(nameof(FilterModel.Id))
				.WithColumn(nameof(FilterModel.PronounsId)).AsInt32().Nullable()
					.ForeignKey(nameof(FilterModel.PronounsId), PRONOUNS_TABLE, nameof(PronounsModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(FilterModel.SexId)).AsInt32().Nullable()
					.ForeignKey(nameof(FilterModel.SexId), SEXES_TABLE, nameof(SexModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(FilterModel.GenderId)).AsInt32().Nullable()
					.ForeignKey(nameof(FilterModel.GenderId), GENDERS_TABLE, nameof(GenderModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(FilterModel.RegionId)).AsInt32().Nullable()
					.ForeignKey(nameof(FilterModel.RegionId), REGIONS_TABLE, nameof(RegionModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(FilterModel.CountryId)).AsInt32().Nullable()
					.ForeignKey(nameof(FilterModel.CountryId), COUNTRIES_TABLE, nameof(CountryModel.Id)).OnDelete(Rule.SetNull)
				.WithColumn(nameof(FilterModel.FromAge)).AsInt32().Nullable()
				.WithColumn(nameof(FilterModel.ToAge)).AsInt32().Nullable()
				.WithColumn(nameof(FilterModel.AgeEnabled)).AsBoolean().NotNullable()
				;

			Create.Table(ROLES_TABLE)
				.WithIdColumn(nameof(RoleModel.Id))
				.WithColumn(nameof(RoleModel.Name)).AsString().NotNullable().Unique()
				;

			Create.Table(TAGS_TABLE)
				.WithIdColumn(nameof(TagModel.Id))
				.WithColumn(nameof(TagModel.Name)).AsString().NotNullable().Unique()
				;

			Create.Table(SEXUALITIES_TABLE)
				.WithIdColumn(nameof(SexualityModel.Id))
				.WithColumn(nameof(SexualityModel.Name)).AsString().NotNullable().Unique()
				;

			Create.Table(PRONOUNS_TABLE)
				.WithIdColumn(nameof(PronounsModel.Id))
				.WithColumn(nameof(PronounsModel.Name)).AsString().NotNullable().Unique()
				;

			Create.Table(SEXES_TABLE)
				.WithIdColumn(nameof(SexModel.Id))
				.WithColumn(nameof(SexModel.Name)).AsString().NotNullable().Unique()
				.WithColumn(nameof(SexModel.Icon)).AsString().NotNullable()
				;

			Create.Table(GENDERS_TABLE)
				.WithIdColumn(nameof(GenderModel.Id))
				.WithColumn(nameof(GenderModel.Name)).AsString().NotNullable().Unique()
				;

			Create.Table(REGIONS_TABLE)
				.WithIdColumn(nameof(RegionModel.Id))
				.WithColumn(nameof(RegionModel.Name)).AsString().NotNullable().Unique()
				.WithColumn(nameof(RegionModel.Icon)).AsString().NotNullable()
				;

			Create.Table(COUNTRIES_TABLE)
				.WithIdColumn(nameof(CountryModel.Id))
				.WithColumn(nameof(CountryModel.RegionId)).AsInt32().NotNullable()
					.ForeignKey(nameof(CountryModel.RegionId), REGIONS_TABLE, nameof(RegionModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(CountryModel.Name)).AsString().NotNullable().Unique()
				.WithColumn(nameof(CountryModel.Icon)).AsString().NotNullable()
				;

			Create.Table(MATCH_CHOICES_TABLE)
				.WithIdColumn(nameof(MatchChoiceModel.Id))
				.WithColumn(nameof(MatchChoiceModel.AccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(MatchChoiceModel.AccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(MatchChoiceModel.MatchAccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(MatchChoiceModel.MatchAccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(MatchChoiceModel.Choice)).AsInt32().NotNullable()
				;
			Create.UniqueConstraint()
				.OnTable(MATCH_CHOICES_TABLE)
				.Columns(nameof(MatchChoiceModel.AccountId), nameof(MatchChoiceModel.MatchAccountId))
				;

			Create.Table(MATCHES_TABLE)
				.WithIdColumn(nameof(MatchModel.Id))
				.WithColumn(nameof(MatchModel.AccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(MatchModel.AccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(MatchModel.MatchAccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(MatchModel.MatchAccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id)).OnDelete(Rule.Cascade)
				;
			Create.UniqueConstraint()
				.OnTable(MATCHES_TABLE)
				.Columns(nameof(MatchModel.AccountId), nameof(MatchModel.MatchAccountId))
				;

			Create.Table(MESSAGES_TABLE)
				.WithIdColumn(nameof(MessageModel.Id))
				.WithColumn(nameof(MessageModel.MatchId)).AsInt32().NotNullable()
					.ForeignKey(nameof(MessageModel.MatchId), MATCHES_TABLE, nameof(MatchModel.Id)).OnDelete(Rule.Cascade)
				// .WithColumn(nameof(MessageModel.ToPublicKeyId)).AsInt32().NotNullable()
					// TODO: ForeignKey
				.WithColumn(nameof(MessageModel.FromPublicId)).AsFixedLengthAnsiString(AccountConstants.PUBLIC_ID_LENGTH).NotNullable()
					.ForeignKey(nameof(MessageModel.FromPublicId), ACCOUNTS_TABLE, nameof(AccountModel.PublicId)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(MessageModel.ServerDataJson)).AsString().NotNullable()
				.WithColumn(nameof(MessageModel.ClientDataJson_Encrypted_Base64)).AsString().NotNullable()
				.WithColumn(nameof(MessageModel.ClientDataJson_Nonce_Base64)).AsString().NotNullable()
				;

			Create.Table(REPORTS_TABLE)
				.WithIdColumn(nameof(ReportModel.Id))
				.WithColumn(nameof(ReportModel.ReporterAccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(ReportModel.ReporterAccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(ReportModel.ReportedAccountId)).AsInt32().NotNullable()
					.ForeignKey(nameof(ReportModel.ReportedAccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id)).OnDelete(Rule.Cascade)
				.WithColumn(nameof(ReportModel.ReportReasonId)).AsInt32().NotNullable()
					.ForeignKey(nameof(ReportModel.ReportReasonId), REPORT_REASONS_TABLE, nameof(ReportReasonModel.Id)).OnDelete(Rule.Cascade)
				;
			Create.UniqueConstraint()
				.OnTable(REPORTS_TABLE)
				.Columns(nameof(ReportModel.ReporterAccountId), nameof(ReportModel.ReportedAccountId))
				;

			Create.Table(REPORT_REASONS_TABLE)
				.WithIdColumn(nameof(ReportReasonModel.Id))
				.WithColumn(nameof(ReportReasonModel.Name)).AsString().NotNullable().Unique()
				;

			InsertDefaults_Fluent();
		}
		public override void Down()
		{
			Delete.Table(CARDS_TABLE);

			Delete.Table(ACCOUNTS_TABLE);
			Delete.Table(ACCOUNT_ROLES_TABLE);
			Delete.Table(PROFILES_TABLE);
			Delete.Table(PROFILE_TAGS_TABLE);
			Delete.Table(PROFILE_SEXUALITIES_TABLE);
			Delete.Table(FILTERS_TABLE);

			Delete.Table(ROLES_TABLE);
			Delete.Table(TAGS_TABLE);
			Delete.Table(SEXUALITIES_TABLE);

			Delete.Table(PRONOUNS_TABLE);
			Delete.Table(SEXES_TABLE);
			Delete.Table(GENDERS_TABLE);
			Delete.Table(REGIONS_TABLE);
			Delete.Table(COUNTRIES_TABLE);

			Delete.Table(MATCH_CHOICES_TABLE);
			Delete.Table(MATCHES_TABLE);

			Delete.Table(MESSAGES_TABLE);

			Delete.Table(REPORTS_TABLE);
			Delete.Table(REPORT_REASONS_TABLE);
		}

		//===========================================================================================
		// Private Methods
		//===========================================================================================
		private void InsertDefaults_Fluent()
		{
			// Roles
			var IdCounter = 0;
			var RoleNames = Enum.GetNames(typeof(EAccountRole));
			foreach (var RoleName in RoleNames) Insert.IntoTable(ROLES_TABLE).Row(new RoleModel { Id = ++IdCounter, Name = RoleName });

			// Sexualities
			IdCounter = 0;
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Abroromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Abrosexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Androgyneromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Androgynesexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Androromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Androsexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Aromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Asexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Biromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Bisexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Ceteroromantic/Skolioromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Ceterosexual/Skoliosexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Demiromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Demisexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Finromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Finsexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Greyromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Greysexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Gyneromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Gynesexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Heteroromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Heterosexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Homoromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Homosexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Omniromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Omnisexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Other" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Panromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Pansexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Polyromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Polysexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Pomoromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Pomosexual" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Queer" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Questioning" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Sapioromantic" });
			Insert.IntoTable(SEXUALITIES_TABLE).Row(new SexualityModel { Id = ++IdCounter, Name = "Sapiosexual" });

			// Pronouns
			IdCounter = 0;
			Insert.IntoTable(PRONOUNS_TABLE).Row(new PronounsModel { Id = ++IdCounter, Name = "He/Him" });
			Insert.IntoTable(PRONOUNS_TABLE).Row(new PronounsModel { Id = ++IdCounter, Name = "She/Her" });
			Insert.IntoTable(PRONOUNS_TABLE).Row(new PronounsModel { Id = ++IdCounter, Name = "They/Them" });

			// Sexes
			IdCounter = 0;
			Insert.IntoTable(SEXES_TABLE).Row(new SexModel { Id = ++IdCounter, Name = "Female", Icon = "♀" });
			Insert.IntoTable(SEXES_TABLE).Row(new SexModel { Id = ++IdCounter, Name = "Male", Icon = "♂" });
			Insert.IntoTable(SEXES_TABLE).Row(new SexModel { Id = ++IdCounter, Name = "Transsexual (F2M)", Icon = "⚧" });
			Insert.IntoTable(SEXES_TABLE).Row(new SexModel { Id = ++IdCounter, Name = "Transsexual (M2F)", Icon = "⚧" });

			// Genders
			IdCounter = 0;
			Insert.IntoTable(GENDERS_TABLE).Row(new GenderModel { Id = ++IdCounter, Name = "Cisgender" });
			Insert.IntoTable(GENDERS_TABLE).Row(new GenderModel { Id = ++IdCounter, Name = "Non-Binary" });
			Insert.IntoTable(GENDERS_TABLE).Row(new GenderModel { Id = ++IdCounter, Name = "Transgender" });

			// Regions
			IdCounter = 0;

			var Africa = new RegionModel { Id = ++IdCounter, Name = "Africa", Icon = "🌍" };
			Insert.IntoTable(REGIONS_TABLE).Row(Africa);
			var Antarctica = new RegionModel { Id = ++IdCounter, Name = "Antarctica", Icon = "🇦🇶" };
			Insert.IntoTable(REGIONS_TABLE).Row(Antarctica);
			var Asia = new RegionModel { Id = ++IdCounter, Name = "Asia", Icon = "🌏" };
			Insert.IntoTable(REGIONS_TABLE).Row(Asia);
			var Europe = new RegionModel { Id = ++IdCounter, Name = "Europe", Icon = "🇪🇺" };
			Insert.IntoTable(REGIONS_TABLE).Row(Europe);
			var NorthAmerica = new RegionModel { Id = ++IdCounter, Name = "North America", Icon = "🌎" };
			Insert.IntoTable(REGIONS_TABLE).Row(NorthAmerica);
			var Oceania = new RegionModel { Id = ++IdCounter, Name = "Oceania", Icon = "🌏" };
			Insert.IntoTable(REGIONS_TABLE).Row(Oceania);
			var SouthAmerica = new RegionModel { Id = ++IdCounter, Name = "South America", Icon = "🌎" };
			Insert.IntoTable(REGIONS_TABLE).Row(SouthAmerica);

			// Countries
			IdCounter = 0;

			// => Africa
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Algeria", Icon = "🇩🇿", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Angola", Icon = "🇦🇴", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Benin", Icon = "🇧🇯", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Botswana", Icon = "🇧🇼", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Burkina Faso", Icon = "🇧🇫", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Burundi", Icon = "🇧🇮", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Cameroon", Icon = "🇨🇲", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Cape Verde", Icon = "🇨🇻", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Central African Republic", Icon = "🇨🇫", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Chad", Icon = "🇹🇩", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Comoros", Icon = "🇰🇲", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Congo - Brazzaville", Icon = "🇨🇬", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Congo - Kinshasa", Icon = "🇨🇩", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Côte d'Ivoire", Icon = "🇨🇮", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Djibouti", Icon = "🇩🇯", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Equatorial Guinea", Icon = "🇬🇶", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Egypt", Icon = "🇪🇬", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Eritrea", Icon = "🇪🇷", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Ethiopia", Icon = "🇪🇹", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Gabon", Icon = "🇬🇦", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Gambia", Icon = "🇬🇲", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Ghana", Icon = "🇬🇭", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Guinea", Icon = "🇬🇳", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Guinea-Bissau", Icon = "🇬🇼", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Kenya", Icon = "🇰🇪", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Lesotho", Icon = "🇱🇸", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Liberia", Icon = "🇱🇷", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Libya", Icon = "🇱🇾", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Madagascar", Icon = "🇲🇬", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Malawi", Icon = "🇲🇼", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Mali", Icon = "🇲🇱", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Mauritania", Icon = "🇲🇷", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Mauritius", Icon = "🇲🇺", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Morocco", Icon = "🇲🇦", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Mozambique", Icon = "🇲🇿", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Namibia", Icon = "🇳🇦", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Niger", Icon = "🇳🇪", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Nigeria", Icon = "🇳🇬", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Rwanda", Icon = "🇷🇼", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "São Tomé & Príncipe", Icon = "🇸🇹", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Senegal", Icon = "🇸🇳", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Seychelles", Icon = "🇸🇨", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Sierra Leone", Icon = "🇸🇱", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Somalia", Icon = "🇸🇴", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "South Africa", Icon = "🇿🇦", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "South Sudan", Icon = "🇸🇸", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Sudan", Icon = "🇸🇩", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Eswatini", Icon = "🇸🇿", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Tanzania", Icon = "🇹🇿", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Togo", Icon = "🇹🇬", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Tunisia", Icon = "🇹🇳", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Uganda", Icon = "🇺🇬", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Western Sahara", Icon = "🇪🇭", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Zambia", Icon = "🇿🇲", RegionId = Africa.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Zimbabwe", Icon = "🇿🇼", RegionId = Africa.Id });

			// => Asia
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Afghanistan", Icon = "🇦🇫", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Armenia", Icon = "🇦🇲", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Azerbaijan", Icon = "🇦🇿", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Bahrain", Icon = "🇧🇭", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Bangladesh", Icon = "🇧🇩", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Bhutan", Icon = "🇧🇹", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Brunei", Icon = "🇧🇳", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Cambodia", Icon = "🇰🇭", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "China", Icon = "🇨🇳", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Timor Leste", Icon = "🇹🇱", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Georgia (Asia)", Icon = "🇬🇪", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "India", Icon = "🇮🇳", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Indonesia", Icon = "🇮🇩", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Iran", Icon = "🇮🇷", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Iraq", Icon = "🇮🇶", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Israel", Icon = "🇮🇱", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Japan", Icon = "🇯🇵", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Jordan", Icon = "🇯🇴", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Kazakhstan", Icon = "🇰🇿", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Kuwait", Icon = "🇰🇼", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Kyrgyzstan", Icon = "🇰🇬", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Laos", Icon = "🇱🇦", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Lebanon", Icon = "🇱🇧", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Malaysia", Icon = "🇲🇾", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Maldives", Icon = "🇲🇻", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Mongolia", Icon = "🇲🇳", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Myanmar", Icon = "🇲🇲", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Nepal", Icon = "🇳🇵", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "North Korea", Icon = "🇰🇵", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Oman", Icon = "🇴🇲", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Pakistan", Icon = "🇵🇰", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Palestinian Territories", Icon = "🇵🇸", RegionId = Asia.Id }); // TODO: ?
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Philippines", Icon = "🇵🇭", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Qatar", Icon = "🇶🇦", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Russia (Asia)", Icon = "🇷🇺", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Saudi Arabia", Icon = "🇸🇦", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Singapore", Icon = "🇸🇬", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "South Korea", Icon = "🇰🇷", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Sri Lanka", Icon = "🇱🇰", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Syria", Icon = "🇸🇾", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Tajikistan", Icon = "🇹🇯", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Thailand", Icon = "🇹🇭", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Turkey (Asia)", Icon = "🇹🇷", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Turkmenistan", Icon = "🇹🇲", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Taiwan", Icon = "🇹🇼", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "United Arab Emirates", Icon = "🇦🇪", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Uzbekistan", Icon = "🇺🇿", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Vietnam", Icon = "🇻🇳", RegionId = Asia.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Yemen", Icon = "🇾🇪", RegionId = Asia.Id });

			// => Europe
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Albania", Icon = "🇦🇱", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Andorra", Icon = "🇦🇩", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Austria", Icon = "🇦🇹", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Belarus", Icon = "🇧🇾", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Belgium", Icon = "🇧🇪", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Bosnia & Herzegovina", Icon = "🇧🇦", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Bulgaria", Icon = "🇧🇬", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Croatia", Icon = "🇭🇷", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Cyprus", Icon = "🇨🇾", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Czechia", Icon = "🇨🇿", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Denmark", Icon = "🇩🇰", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Estonia", Icon = "🇪🇪", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Finland", Icon = "🇫🇮", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "France", Icon = "🇫🇷", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Georgia (Europe)", Icon = "🇬🇪", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Germany", Icon = "🇩🇪", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Greece", Icon = "🇬🇷", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Hungary", Icon = "🇭🇺", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Iceland", Icon = "🇮🇸", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Ireland", Icon = "🇮🇪", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Italy", Icon = "🇮🇹", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Kosovo", Icon = "🇽🇰", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Latvia", Icon = "🇱🇻", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Liechtenstein", Icon = "🇱🇮", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Lithuania", Icon = "🇱🇹", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Luxembourg", Icon = "🇱🇺", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "North Macedonia", Icon = "🇲🇰", RegionId = Europe.Id });

			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Malta", Icon = "🇲🇹", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Moldova", Icon = "🇲🇩", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Monaco", Icon = "🇲🇨", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Montenegro", Icon = "🇲🇪", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Netherlands", Icon = "🇳🇱", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Norway", Icon = "🇳🇴", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Poland", Icon = "🇵🇱", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Portugal", Icon = "🇵🇹", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Romania", Icon = "🇷🇴", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Russia (Europe)", Icon = "🇷🇺", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "San Marino", Icon = "🇸🇲", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Serbia", Icon = "🇷🇸", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Slovakia", Icon = "🇸🇰", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Slovenia", Icon = "🇸🇮", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Spain", Icon = "🇪🇸", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Sweden", Icon = "🇸🇪", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Switzerland", Icon = "🇨🇭", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Turkey (Europe)", Icon = "🇹🇷", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Ukraine", Icon = "🇺🇦", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "United Kingdom", Icon = "🇬🇧", RegionId = Europe.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Vatican City", Icon = "🇻🇦", RegionId = Europe.Id });

			// => North America
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Canada", Icon = "🇨🇦", RegionId = NorthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Greenland", Icon = "🇬🇱", RegionId = NorthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Mexico", Icon = "🇲🇽", RegionId = NorthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "United States", Icon = "🇺🇸", RegionId = NorthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Navassa Island", Icon = "🇺🇸", RegionId = NorthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Puerto Rico", Icon = "🇵🇷", RegionId = NorthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "U.S. Virgin Islands", Icon = "🇻🇮", RegionId = NorthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Guam", Icon = "🇬🇺", RegionId = NorthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "American Samoa", Icon = "🇦🇸", RegionId = NorthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Dominican Republic", Icon = "🇩🇴", RegionId = NorthAmerica.Id });

			// => Oceania
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Australia", Icon = "🇦🇺", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Fiji", Icon = "🇫🇯", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "New Zealand", Icon = "🇳🇿", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Micronesia", Icon = "🇫🇲", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Kiribati", Icon = "🇰🇮", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Marshall Islands", Icon = "🇲🇭", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Nauru", Icon = "🇳🇷", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Palau", Icon = "🇵🇼", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Papua New Guinea", Icon = "🇵🇬", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Samoa", Icon = "🇼🇸", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Solomon Islands", Icon = "🇸🇧", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Tonga", Icon = "🇹🇴", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Tuvalu", Icon = "🇹🇻", RegionId = Oceania.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Vanuatu", Icon = "🇻🇺", RegionId = Oceania.Id });

			// => South America
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Brazil", Icon = "🇧🇷", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Argentina", Icon = "🇦🇷", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Bolivia", Icon = "🇧🇴", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Chile", Icon = "🇨🇱", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Colombia", Icon = "🇨🇴", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Ecuador", Icon = "🇪🇨", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Falkland Islands", Icon = "🇫🇰", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "French Guiana", Icon = "🇬🇫", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Guyana", Icon = "🇬🇾", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Paraguay", Icon = "🇵🇾", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Peru", Icon = "🇵🇪", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "South Georgia & South Sandwich Islands", Icon = "🇬🇸", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Suriname", Icon = "🇸🇷", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Uruguay", Icon = "🇺🇾", RegionId = SouthAmerica.Id });
			Insert.IntoTable(COUNTRIES_TABLE).Row(new CountryModel { Id = ++IdCounter, Name = "Venezuela", Icon = "🇻🇪", RegionId = SouthAmerica.Id });

			// ReportReasons
			IdCounter = 0;
			Insert.IntoTable(REPORT_REASONS_TABLE).Row(new ReportReasonModel { Id = ++IdCounter, Name = "InappropriateProfile" });
		}
	}
}