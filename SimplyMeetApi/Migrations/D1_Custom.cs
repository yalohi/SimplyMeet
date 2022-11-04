// NOTICE: This is unused code! FluentMigrator has some issues with not setting the correct constraints for SQLite databases.
// I wasn't sure what the lesser of the two evils is so I am keeping around both implementations for now.

using static SimplyMeetApi.Constants.DatabaseConstants;

namespace SimplyMeetApi.Migrations;

public class D1_Custom : CustomMigration
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly DatabaseService _DatabaseService;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public D1_Custom(DatabaseService InDatabaseService)
	{
		_DatabaseService = InDatabaseService;
	}

	public override void Up()
	{
		_DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {CARDS_TABLE}
			(
				[{nameof(CardModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(CardModel.Title)}] {TEXT} {NOT_NULL},
				[{nameof(CardModel.Content)}] {TEXT} {NOT_NULL},
				{GetPrimaryKeySql(nameof(CardModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {ACCOUNTS_TABLE}
			(
				[{nameof(AccountModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(AccountModel.PublicId)}] {TEXT} {NOT_NULL} {UNIQUE},
				[{nameof(AccountModel.PublicKey_Base64)}] {TEXT} {NOT_NULL} {UNIQUE},
				[{nameof(AccountModel.Creation)}] {TEXT} {NOT_NULL},
				[{nameof(AccountModel.LastActive)}] {TEXT},
				[{nameof(AccountModel.Status)}] {INTEGER} {NOT_NULL},
				[{nameof(AccountModel.ProfileId)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(AccountModel.FilterId)}] {INTEGER} {NOT_NULL} {UNIQUE},
				{GetPrimaryKeySql(nameof(AccountModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {ACCOUNT_ROLES_TABLE}
			(
				[{nameof(AccountRoleModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(AccountRoleModel.RoleId)}] {INTEGER} {NOT_NULL},
				[{nameof(AccountRoleModel.AccountPublicId)}] {TEXT} {NOT_NULL},
				{GetForeignKeySql(nameof(AccountRoleModel.RoleId), ROLES_TABLE, nameof(RoleModel.Id), ON_DELETE_CASCADE)},
				{GetForeignKeySql(nameof(AccountRoleModel.AccountPublicId), ACCOUNTS_TABLE, nameof(AccountModel.PublicId), ON_DELETE_CASCADE)},
				{GetPrimaryKeySql(nameof(AccountRoleModel.Id))}
				{UNIQUE}({nameof(AccountRoleModel.RoleId)},{nameof(AccountRoleModel.AccountPublicId)})
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {PROFILES_TABLE}
			(
				[{nameof(ProfileModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(ProfileModel.Avatar)}] {TEXT} {NOT_NULL},
				[{nameof(ProfileModel.DisplayName)}] {TEXT} {NOT_NULL},
				[{nameof(ProfileModel.PronounsId)}] {INTEGER},
				[{nameof(ProfileModel.SexId)}] {INTEGER},
				[{nameof(ProfileModel.GenderId)}] {INTEGER},
				[{nameof(ProfileModel.RegionId)}] {INTEGER},
				[{nameof(ProfileModel.CountryId)}] {INTEGER},
				[{nameof(ProfileModel.BirthDate)}] {TEXT},
				[{nameof(ProfileModel.LookingFor)}] {INTEGER} {NOT_NULL},
				[{nameof(ProfileModel.AboutMe)}] {TEXT},
				[{nameof(ProfileModel.AboutYou)}] {TEXT},
				{GetForeignKeySql(nameof(ProfileModel.PronounsId), PRONOUNS_TABLE, nameof(PronounsModel.Id), ON_DELETE_SET_NULL)},
				{GetForeignKeySql(nameof(ProfileModel.SexId), SEXES_TABLE, nameof(SexModel.Id), ON_DELETE_SET_NULL)},
				{GetForeignKeySql(nameof(ProfileModel.GenderId), GENDERS_TABLE, nameof(GenderModel.Id), ON_DELETE_SET_NULL)},
				{GetForeignKeySql(nameof(ProfileModel.RegionId), REGIONS_TABLE, nameof(RegionModel.Id), ON_DELETE_SET_NULL)},
				{GetForeignKeySql(nameof(ProfileModel.CountryId), COUNTRIES_TABLE, nameof(CountryModel.Id), ON_DELETE_SET_NULL)},
				{GetPrimaryKeySql(nameof(ProfileModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {PROFILE_TAGS_TABLE}
			(
				[{nameof(ProfileTagModel.ProfileId)}] {INTEGER} {NOT_NULL},
				[{nameof(ProfileTagModel.TagId)}] {INTEGER} {NOT_NULL},
				{GetForeignKeySql(nameof(ProfileTagModel.ProfileId), PROFILES_TABLE, nameof(ProfileModel.Id), ON_DELETE_CASCADE)},
				{GetForeignKeySql(nameof(ProfileTagModel.TagId), TAGS_TABLE, nameof(TagModel.Id), ON_DELETE_CASCADE)},
				{UNIQUE}({nameof(ProfileTagModel.ProfileId)},{nameof(ProfileTagModel.TagId)})
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {PROFILE_SEXUALITIES_TABLE}
			(
				[{nameof(ProfileSexualityModel.ProfileId)}] {INTEGER} {NOT_NULL},
				[{nameof(ProfileSexualityModel.SexualityId)}] {INTEGER} {NOT_NULL},
				{GetForeignKeySql(nameof(ProfileSexualityModel.ProfileId), PROFILES_TABLE, nameof(ProfileModel.Id), ON_DELETE_CASCADE)},
				{GetForeignKeySql(nameof(ProfileSexualityModel.SexualityId), SEXUALITIES_TABLE, nameof(SexualityModel.Id), ON_DELETE_CASCADE)},
				{UNIQUE}({nameof(ProfileSexualityModel.ProfileId)},{nameof(ProfileSexualityModel.SexualityId)})
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {FILTERS_TABLE}
			(
				[{nameof(FilterModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(FilterModel.PronounsId)}] {INTEGER},
				[{nameof(FilterModel.SexId)}] {INTEGER},
				[{nameof(FilterModel.GenderId)}] {INTEGER},
				[{nameof(FilterModel.RegionId)}] {INTEGER},
				[{nameof(FilterModel.CountryId)}] {INTEGER},
				[{nameof(FilterModel.FromAge)}] {INTEGER},
				[{nameof(FilterModel.ToAge)}] {INTEGER},
				[{nameof(FilterModel.AgeEnabled)}] {INTEGER},
				{GetForeignKeySql(nameof(FilterModel.PronounsId), PRONOUNS_TABLE, nameof(PronounsModel.Id), ON_DELETE_SET_NULL)},
				{GetForeignKeySql(nameof(FilterModel.SexId), SEXES_TABLE, nameof(SexModel.Id), ON_DELETE_SET_NULL)},
				{GetForeignKeySql(nameof(FilterModel.GenderId), GENDERS_TABLE, nameof(GenderModel.Id), ON_DELETE_SET_NULL)},
				{GetForeignKeySql(nameof(FilterModel.RegionId), REGIONS_TABLE, nameof(RegionModel.Id), ON_DELETE_SET_NULL)},
				{GetForeignKeySql(nameof(FilterModel.CountryId), COUNTRIES_TABLE, nameof(CountryModel.Id), ON_DELETE_SET_NULL)},
				{GetPrimaryKeySql(nameof(FilterModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {ROLES_TABLE}
			(
				[{nameof(RoleModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(RoleModel.Name)}] {TEXT} {NOT_NULL} {UNIQUE},
				{GetPrimaryKeySql(nameof(RoleModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {TAGS_TABLE}
			(
				[{nameof(TagModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(TagModel.Name)}] {TEXT} {NOT_NULL} {UNIQUE},
				{GetPrimaryKeySql(nameof(TagModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {SEXUALITIES_TABLE}
			(
				[{nameof(SexualityModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(SexualityModel.Name)}] {TEXT} {NOT_NULL} {UNIQUE},
				{GetPrimaryKeySql(nameof(SexualityModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {PRONOUNS_TABLE}
			(
				[{nameof(PronounsModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(PronounsModel.Name)}] {TEXT} {NOT_NULL} {UNIQUE},
				{GetPrimaryKeySql(nameof(PronounsModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {SEXES_TABLE}
			(
				[{nameof(SexModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(SexModel.Name)}] {TEXT} {NOT_NULL} {UNIQUE},
				[{nameof(SexModel.Icon)}] {TEXT} {NOT_NULL},
				{GetPrimaryKeySql(nameof(SexModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {GENDERS_TABLE}
			(
				[{nameof(GenderModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(GenderModel.Name)}] {TEXT} {NOT_NULL} {UNIQUE},
				{GetPrimaryKeySql(nameof(GenderModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {REGIONS_TABLE}
			(
				[{nameof(RegionModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(RegionModel.Name)}] {TEXT} {NOT_NULL} {UNIQUE},
				[{nameof(RegionModel.Icon)}] {TEXT} {NOT_NULL},
				{GetPrimaryKeySql(nameof(RegionModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {COUNTRIES_TABLE}
			(
				[{nameof(CountryModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(CountryModel.RegionId)}] {TEXT} {NOT_NULL},
				[{nameof(CountryModel.Name)}] {TEXT} {NOT_NULL} {UNIQUE},
				[{nameof(CountryModel.Icon)}] {TEXT} {NOT_NULL},
				{GetForeignKeySql(nameof(CountryModel.RegionId), REGIONS_TABLE, nameof(RegionModel.Id), ON_DELETE_CASCADE)},
				{GetPrimaryKeySql(nameof(CountryModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {MATCH_CHOICES_TABLE}
			(
				[{nameof(MatchChoiceModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(MatchChoiceModel.AccountId)}] {INTEGER} {NOT_NULL},
				[{nameof(MatchChoiceModel.MatchAccountId)}] {INTEGER} {NOT_NULL},
				[{nameof(MatchChoiceModel.Choice)}] {INTEGER} {NOT_NULL},
				{GetForeignKeySql(nameof(MatchChoiceModel.AccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id), ON_DELETE_CASCADE)},
				{GetForeignKeySql(nameof(MatchChoiceModel.MatchAccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id), ON_DELETE_CASCADE)},
				{GetPrimaryKeySql(nameof(MatchChoiceModel.Id))},
				{UNIQUE}({nameof(MatchChoiceModel.AccountId)},{nameof(MatchChoiceModel.MatchAccountId)})
			)");

			// TODO: BI-DIRECTIONAL UNIQUE AccountId,MatchAccountId?
			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {MATCHES_TABLE}
			(
				[{nameof(MatchModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(MatchModel.AccountId)}] {INTEGER} {NOT_NULL},
				[{nameof(MatchModel.MatchAccountId)}] {INTEGER} {NOT_NULL},
				{GetForeignKeySql(nameof(MatchModel.AccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id), ON_DELETE_CASCADE)},
				{GetForeignKeySql(nameof(MatchModel.MatchAccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id), ON_DELETE_CASCADE)},
				{GetPrimaryKeySql(nameof(MatchModel.Id))},
				{UNIQUE}({nameof(MatchModel.AccountId)},{nameof(MatchModel.MatchAccountId)})
			)");

			// TODO: {GetForeignKeySql(nameof(MessageModel.ToPublicKeyId), PUBLIC_KEYS_TABLE, nameof(PublicKeyModel.Id), ON_DELETE_CASCADE)},
			// [{nameof(MessageModel.ToPublicKeyId)}] {INTEGER} {NOT_NULL},
			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {MESSAGES_TABLE}
			(
				[{nameof(MessageModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(MessageModel.MatchId)}] {INTEGER} {NOT_NULL},
				[{nameof(MessageModel.FromPublicId)}] {TEXT} {NOT_NULL},
				[{nameof(MessageModel.ServerDataJson)}] {TEXT} {NOT_NULL},
				[{nameof(MessageModel.ClientDataJson_Encrypted_Base64)}] {TEXT} {NOT_NULL},
				[{nameof(MessageModel.ClientDataJson_Nonce_Base64)}] {TEXT} {NOT_NULL},
				{GetForeignKeySql(nameof(MessageModel.MatchId), MATCHES_TABLE, nameof(MatchModel.Id), ON_DELETE_CASCADE)},
				{GetForeignKeySql(nameof(MessageModel.FromPublicId), ACCOUNTS_TABLE, nameof(AccountModel.PublicId), ON_DELETE_CASCADE)},
				{GetPrimaryKeySql(nameof(MessageModel.Id))}
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {REPORTS_TABLE}
			(
				[{nameof(ReportModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(ReportModel.ReporterAccountId)}] {INTEGER} {NOT_NULL},
				[{nameof(ReportModel.ReportedAccountId)}] {INTEGER} {NOT_NULL},
				[{nameof(ReportModel.ReportReasonId)}] {INTEGER} {NOT_NULL},
				{GetForeignKeySql(nameof(ReportModel.ReporterAccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id), ON_DELETE_CASCADE)},
				{GetForeignKeySql(nameof(ReportModel.ReportedAccountId), ACCOUNTS_TABLE, nameof(AccountModel.Id), ON_DELETE_CASCADE)},
				{GetForeignKeySql(nameof(ReportModel.ReportReasonId), REPORT_REASONS_TABLE, nameof(ReportReasonModel.Id), ON_DELETE_CASCADE)},
				{GetPrimaryKeySql(nameof(ReportModel.Id))},
				{UNIQUE}({nameof(ReportModel.ReporterAccountId)},{nameof(ReportModel.ReportedAccountId)})
			)");

			await InConnection.ExecuteAsync($@"{CREATE_TABLE} {REPORT_REASONS_TABLE}
			(
				[{nameof(ReportReasonModel.Id)}] {INTEGER} {NOT_NULL} {UNIQUE},
				[{nameof(ReportReasonModel.Name)}] {TEXT} {NOT_NULL} {UNIQUE},
				{GetPrimaryKeySql(nameof(ReportReasonModel.Id))}
			)");

			await InsertDefaultsAsync(InConnection);
		}).Wait();
	}
	public override void Down()
	{
		_DatabaseService.PerformTransactionAsync(async InConnection =>
		{
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {CARDS_TABLE}");

			await InConnection.ExecuteAsync($@"{DROP_TABLE} {ACCOUNTS_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {ACCOUNT_ROLES_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {PROFILES_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {PROFILE_TAGS_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {PROFILE_SEXUALITIES_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {FILTERS_TABLE}");

			await InConnection.ExecuteAsync($@"{DROP_TABLE} {ROLES_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {TAGS_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {SEXUALITIES_TABLE}");

			await InConnection.ExecuteAsync($@"{DROP_TABLE} {PRONOUNS_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {SEXES_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {GENDERS_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {REGIONS_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {COUNTRIES_TABLE}");

			await InConnection.ExecuteAsync($@"{DROP_TABLE} {MATCH_CHOICES_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {MATCHES_TABLE}");

			await InConnection.ExecuteAsync($@"{DROP_TABLE} {MESSAGES_TABLE}");

			await InConnection.ExecuteAsync($@"{DROP_TABLE} {REPORTS_TABLE}");
			await InConnection.ExecuteAsync($@"{DROP_TABLE} {REPORT_REASONS_TABLE}");
		}).Wait();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task InsertDefaultsAsync(IDbConnection InConnection)
	{
		// Roles
		var RoleNames = Enum.GetNames(typeof(EAccountRole));
		foreach (var RoleName in RoleNames) await _DatabaseService.InsertModelAsync(new RoleModel { Name = RoleName }, InConnection);

		// Sexualities
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Abroromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Abrosexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Androgyneromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Androgynesexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Androromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Androsexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Aromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Asexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Biromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Bisexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Ceteroromantic/Skolioromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Ceterosexual/Skoliosexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Demiromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Demisexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Finromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Finsexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Greyromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Greysexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Gyneromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Gynesexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Heteroromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Heterosexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Homoromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Homosexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Omniromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Omnisexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Other" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Panromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Pansexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Polyromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Polysexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Pomoromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Pomosexual" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Queer" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Questioning" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Sapioromantic" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexualityModel { Name = "Sapiosexual" }, InConnection);

		// Pronouns
		await _DatabaseService.InsertModelAsync(new PronounsModel { Name = "He/Him" }, InConnection);
		await _DatabaseService.InsertModelAsync(new PronounsModel { Name = "She/Her" }, InConnection);
		await _DatabaseService.InsertModelAsync(new PronounsModel { Name = "They/Them" }, InConnection);

		// Sexes
		await _DatabaseService.InsertModelAsync(new SexModel { Name = "Female", Icon = "♀" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexModel { Name = "Male", Icon = "♂" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexModel { Name = "Transsexual (F2M)", Icon = "⚧" }, InConnection);
		await _DatabaseService.InsertModelAsync(new SexModel { Name = "Transsexual (M2F)", Icon = "⚧" }, InConnection);

		// Genders
		await _DatabaseService.InsertModelAsync(new GenderModel { Name = "Cisgender" }, InConnection);
		await _DatabaseService.InsertModelAsync(new GenderModel { Name = "Non-Binary" }, InConnection);
		await _DatabaseService.InsertModelAsync(new GenderModel { Name = "Transgender" }, InConnection);

		// Regions
		var AfricaId = await _DatabaseService.InsertModelReturnIdAsync(new RegionModel { Name = "Africa", Icon = "🌍" }, InConnection);
		var AntarcticaId = await _DatabaseService.InsertModelReturnIdAsync(new RegionModel { Name = "Antarctica", Icon = "🇦🇶" }, InConnection);
		var AsiaId = await _DatabaseService.InsertModelReturnIdAsync(new RegionModel { Name = "Asia", Icon = "🌏" }, InConnection);
		var EuropeId = await _DatabaseService.InsertModelReturnIdAsync(new RegionModel { Name = "Europe", Icon = "🇪🇺" }, InConnection);
		var NorthAmericaId = await _DatabaseService.InsertModelReturnIdAsync(new RegionModel { Name = "North America", Icon = "🌎" }, InConnection);
		var OceaniaId = await _DatabaseService.InsertModelReturnIdAsync(new RegionModel { Name = "Oceania", Icon = "🌏" }, InConnection);
		var SouthAmericaId = await _DatabaseService.InsertModelReturnIdAsync(new RegionModel { Name = "South America", Icon = "🌎" }, InConnection);

		// Countries
		// => Africa
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Algeria", Icon = "🇩🇿", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Angola", Icon = "🇦🇴", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Benin", Icon = "🇧🇯", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Botswana", Icon = "🇧🇼", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Burkina Faso", Icon = "🇧🇫", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Burundi", Icon = "🇧🇮", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Cameroon", Icon = "🇨🇲", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Cape Verde", Icon = "🇨🇻", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Central African Republic", Icon = "🇨🇫", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Chad", Icon = "🇹🇩", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Comoros", Icon = "🇰🇲", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Congo - Brazzaville", Icon = "🇨🇬", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Congo - Kinshasa", Icon = "🇨🇩", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Côte d'Ivoire", Icon = "🇨🇮", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Djibouti", Icon = "🇩🇯", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Equatorial Guinea", Icon = "🇬🇶", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Egypt", Icon = "🇪🇬", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Eritrea", Icon = "🇪🇷", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Ethiopia", Icon = "🇪🇹", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Gabon", Icon = "🇬🇦", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Gambia", Icon = "🇬🇲", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Ghana", Icon = "🇬🇭", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Guinea", Icon = "🇬🇳", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Guinea-Bissau", Icon = "🇬🇼", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Kenya", Icon = "🇰🇪", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Lesotho", Icon = "🇱🇸", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Liberia", Icon = "🇱🇷", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Libya", Icon = "🇱🇾", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Madagascar", Icon = "🇲🇬", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Malawi", Icon = "🇲🇼", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Mali", Icon = "🇲🇱", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Mauritania", Icon = "🇲🇷", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Mauritius", Icon = "🇲🇺", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Morocco", Icon = "🇲🇦", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Mozambique", Icon = "🇲🇿", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Namibia", Icon = "🇳🇦", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Niger", Icon = "🇳🇪", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Nigeria", Icon = "🇳🇬", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Rwanda", Icon = "🇷🇼", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "São Tomé & Príncipe", Icon = "🇸🇹", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Senegal", Icon = "🇸🇳", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Seychelles", Icon = "🇸🇨", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Sierra Leone", Icon = "🇸🇱", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Somalia", Icon = "🇸🇴", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "South Africa", Icon = "🇿🇦", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "South Sudan", Icon = "🇸🇸", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Sudan", Icon = "🇸🇩", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Eswatini", Icon = "🇸🇿", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Tanzania", Icon = "🇹🇿", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Togo", Icon = "🇹🇬", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Tunisia", Icon = "🇹🇳", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Uganda", Icon = "🇺🇬", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Western Sahara", Icon = "🇪🇭", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Zambia", Icon = "🇿🇲", RegionId = AfricaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Zimbabwe", Icon = "🇿🇼", RegionId = AfricaId }, InConnection);

		// => Asia
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Afghanistan", Icon = "🇦🇫", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Armenia", Icon = "🇦🇲", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Azerbaijan", Icon = "🇦🇿", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Bahrain", Icon = "🇧🇭", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Bangladesh", Icon = "🇧🇩", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Bhutan", Icon = "🇧🇹", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Brunei", Icon = "🇧🇳", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Cambodia", Icon = "🇰🇭", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "China", Icon = "🇨🇳", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Timor Leste", Icon = "🇹🇱", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Georgia (Asia)", Icon = "🇬🇪", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "India", Icon = "🇮🇳", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Indonesia", Icon = "🇮🇩", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Iran", Icon = "🇮🇷", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Iraq", Icon = "🇮🇶", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Israel", Icon = "🇮🇱", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Japan", Icon = "🇯🇵", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Jordan", Icon = "🇯🇴", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Kazakhstan", Icon = "🇰🇿", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Kuwait", Icon = "🇰🇼", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Kyrgyzstan", Icon = "🇰🇬", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Laos", Icon = "🇱🇦", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Lebanon", Icon = "🇱🇧", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Malaysia", Icon = "🇲🇾", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Maldives", Icon = "🇲🇻", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Mongolia", Icon = "🇲🇳", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Myanmar", Icon = "🇲🇲", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Nepal", Icon = "🇳🇵", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "North Korea", Icon = "🇰🇵", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Oman", Icon = "🇴🇲", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Pakistan", Icon = "🇵🇰", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Palestinian Territories", Icon = "🇵🇸", RegionId = AsiaId }, InConnection); // ?
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Philippines", Icon = "🇵🇭", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Qatar", Icon = "🇶🇦", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Russia (Asia)", Icon = "🇷🇺", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Saudi Arabia", Icon = "🇸🇦", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Singapore", Icon = "🇸🇬", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "South Korea", Icon = "🇰🇷", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Sri Lanka", Icon = "🇱🇰", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Syria", Icon = "🇸🇾", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Tajikistan", Icon = "🇹🇯", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Thailand", Icon = "🇹🇭", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Turkey (Asia)", Icon = "🇹🇷", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Turkmenistan", Icon = "🇹🇲", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Taiwan", Icon = "🇹🇼", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "United Arab Emirates", Icon = "🇦🇪", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Uzbekistan", Icon = "🇺🇿", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Vietnam", Icon = "🇻🇳", RegionId = AsiaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Yemen", Icon = "🇾🇪", RegionId = AsiaId }, InConnection);

		// => Europe
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Albania", Icon = "🇦🇱", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Andorra", Icon = "🇦🇩", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Austria", Icon = "🇦🇹", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Belarus", Icon = "🇧🇾", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Belgium", Icon = "🇧🇪", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Bosnia & Herzegovina", Icon = "🇧🇦", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Bulgaria", Icon = "🇧🇬", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Croatia", Icon = "🇭🇷", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Cyprus", Icon = "🇨🇾", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Czechia", Icon = "🇨🇿", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Denmark", Icon = "🇩🇰", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Estonia", Icon = "🇪🇪", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Finland", Icon = "🇫🇮", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "France", Icon = "🇫🇷", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Georgia (Europe)", Icon = "🇬🇪", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Germany", Icon = "🇩🇪", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Greece", Icon = "🇬🇷", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Hungary", Icon = "🇭🇺", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Iceland", Icon = "🇮🇸", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Ireland", Icon = "🇮🇪", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Italy", Icon = "🇮🇹", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Kosovo", Icon = "🇽🇰", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Latvia", Icon = "🇱🇻", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Liechtenstein", Icon = "🇱🇮", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Lithuania", Icon = "🇱🇹", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Luxembourg", Icon = "🇱🇺", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "North Macedonia", Icon = "🇲🇰", RegionId = EuropeId }, InConnection);

		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Malta", Icon = "🇲🇹", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Moldova", Icon = "🇲🇩", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Monaco", Icon = "🇲🇨", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Montenegro", Icon = "🇲🇪", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Netherlands", Icon = "🇳🇱", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Norway", Icon = "🇳🇴", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Poland", Icon = "🇵🇱", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Portugal", Icon = "🇵🇹", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Romania", Icon = "🇷🇴", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Russia (Europe)", Icon = "🇷🇺", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "San Marino", Icon = "🇸🇲", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Serbia", Icon = "🇷🇸", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Slovakia", Icon = "🇸🇰", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Slovenia", Icon = "🇸🇮", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Spain", Icon = "🇪🇸", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Sweden", Icon = "🇸🇪", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Switzerland", Icon = "🇨🇭", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Turkey (Europe)", Icon = "🇹🇷", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Ukraine", Icon = "🇺🇦", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "United Kingdom", Icon = "🇬🇧", RegionId = EuropeId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Vatican City", Icon = "🇻🇦", RegionId = EuropeId }, InConnection);

		// => North America
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Canada", Icon = "🇨🇦", RegionId = NorthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Greenland", Icon = "🇬🇱", RegionId = NorthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Mexico", Icon = "🇲🇽", RegionId = NorthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "United States", Icon = "🇺🇸", RegionId = NorthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Navassa Island", Icon = "🇺🇸", RegionId = NorthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Puerto Rico", Icon = "🇵🇷", RegionId = NorthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "U.S. Virgin Islands", Icon = "🇻🇮", RegionId = NorthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Guam", Icon = "🇬🇺", RegionId = NorthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "American Samoa", Icon = "🇦🇸", RegionId = NorthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Dominican Republic", Icon = "🇩🇴", RegionId = NorthAmericaId }, InConnection);

		// => Oceania
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Australia", Icon = "🇦🇺", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Fiji", Icon = "🇫🇯", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "New Zealand", Icon = "🇳🇿", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Micronesia", Icon = "🇫🇲", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Kiribati", Icon = "🇰🇮", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Marshall Islands", Icon = "🇲🇭", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Nauru", Icon = "🇳🇷", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Palau", Icon = "🇵🇼", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Papua New Guinea", Icon = "🇵🇬", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Samoa", Icon = "🇼🇸", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Solomon Islands", Icon = "🇸🇧", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Tonga", Icon = "🇹🇴", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Tuvalu", Icon = "🇹🇻", RegionId = OceaniaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Vanuatu", Icon = "🇻🇺", RegionId = OceaniaId }, InConnection);

		// => South America
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Brazil", Icon = "🇧🇷", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Argentina", Icon = "🇦🇷", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Bolivia", Icon = "🇧🇴", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Chile", Icon = "🇨🇱", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Colombia", Icon = "🇨🇴", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Ecuador", Icon = "🇪🇨", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Falkland Islands", Icon = "🇫🇰", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "French Guiana", Icon = "🇬🇫", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Guyana", Icon = "🇬🇾", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Paraguay", Icon = "🇵🇾", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Peru", Icon = "🇵🇪", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "South Georgia & South Sandwich Islands", Icon = "🇬🇸", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Suriname", Icon = "🇸🇷", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Uruguay", Icon = "🇺🇾", RegionId = SouthAmericaId }, InConnection);
		await _DatabaseService.InsertModelAsync(new CountryModel { Name = "Venezuela", Icon = "🇻🇪", RegionId = SouthAmericaId }, InConnection);

		// ReportReasons
		await _DatabaseService.InsertModelAsync(new ReportReasonModel { Name = "InappropriateProfile" }, InConnection);
	}
}