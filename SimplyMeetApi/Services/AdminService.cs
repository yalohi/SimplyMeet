using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SimplyMeetApi.Configuration;
using SimplyMeetApi.Models;
using SimplyMeetShared.Constants;
using SimplyMeetShared.Enums;
using SimplyMeetShared.Models;
using SimplyMeetShared.RequestModels;
using SimplyMeetShared.ResponseModels;

namespace SimplyMeetApi.Services
{
	public class AdminService
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Fields
		private readonly DatabaseService _DatabaseService;
		private readonly MainHubService _MainHubService;
		private readonly ProfileService _ProfileService;
		private readonly ProfileCompactService _ProfileCompactService;
		private readonly TokenService _TokenService;
		private readonly AdminConfiguration _AdminConfig;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public AdminService(DatabaseService InDatabaseService, MainHubService InMainHubService, ProfileService InProfileService, ProfileCompactService InProfileCompactService, TokenService InTokenService, IOptions<AdminConfiguration> InAdminConfig)
		{
			_DatabaseService = InDatabaseService;
			_MainHubService = InMainHubService;
			_ProfileService = InProfileService;
			_ProfileCompactService = InProfileCompactService;
			_TokenService = InTokenService;
			_AdminConfig = InAdminConfig.Value;
		}

		public async Task<AdminGetReportedProfilesResponseModel> GetReportedProfilesAsync(ServiceModel<AdminGetReportedProfilesRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				var ReportedAccounts = await _DatabaseService.GetReportedAccountsAsync(InModel.Request, InConnection);
				var ReportedProfileList = new List<ReportedProfileModel>();

				foreach (var ReportedAccount in ReportedAccounts)
				{
					ReportedProfileList.Add(new ReportedProfileModel
					{
						CompactProfile = await _ProfileCompactService.GetAsync(ReportedAccount.AccountId, EMatchChoice.None, InConnection),
						ReportCount = ReportedAccount.ReportCount,
					});
				}

				return new AdminGetReportedProfilesResponseModel
				{
					ReportedProfiles = ReportedProfileList,
					TotalReportedAccounts = await _DatabaseService.GetTotalReportedAccounts(InConnection),
				};
			});
		}
		public async Task<AdminGetProfileDataResponseModel> GetProfileDataAsync(ServiceModel<AdminGetProfileDataRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				return new AdminGetProfileDataResponseModel
				{
					ProfileData = await _ProfileService.GetProfileDataAsync(InConnection),
				};
			});
		}
		public async Task<AdminGetAccountRolesResponseModel> GetAccountRolesAsync(ServiceModel<AdminGetAccountRolesRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				var Roles = await _DatabaseService.GetAllAsync<RoleModel>(InConnection);
				var AccountRoles = await _DatabaseService.GetAllAsync<AccountRoleModel>(InConnection);
				var MainAdminAccount = await _DatabaseService.GetAccountByPublicIdAsync(_AdminConfig.MainAdminPublicId, InConnection);
				var CompactProfiles = new List<ProfileCompactModel>();

				foreach (var AccountRole in AccountRoles)
				{
					var Account = await _DatabaseService.GetAccountByPublicIdAsync(AccountRole.AccountPublicId, InConnection);
					var CompactProfile = await _ProfileCompactService.GetFromAccountAsync(Account, EMatchChoice.None, InConnection);
					CompactProfiles.Add(CompactProfile);
				}

				return new AdminGetAccountRolesResponseModel
				{
					Roles = Roles,
					AccountRoles = AccountRoles,
					MainAdminCompactProfile = await _ProfileCompactService.GetFromAccountAsync(MainAdminAccount, EMatchChoice.None, InConnection),
					CompactProfiles = CompactProfiles,
				};
			});
		}
		public async Task<AdminSuspendAccountResponseModel> SuspendAccountAsync(ServiceModel<AdminSuspendAccountRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				var Account = await _DatabaseService.GetAccountByPublicIdAsync(InModel.Request.AccountPublicId, InConnection);
				if (Account == null) return new AdminSuspendAccountResponseModel { Error = ErrorConstants.ERROR_NO_SUCH_ACCOUNT };

				Account.Status = EAccountStatus.Suspended;

				if (await _DatabaseService.DeleteAccountReportsAsync(Account, InConnection) <= 0)
					return new AdminSuspendAccountResponseModel { Error = ErrorConstants.ERROR_DATABASE };
				if (await _DatabaseService.UpdateModelByIdAsync(Account, InConnection) <= 0)
					return new AdminSuspendAccountResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				return new AdminSuspendAccountResponseModel();
			});
		}
		public async Task<AdminEditProfileDataResponseModel> EditProfileDataAsync(ServiceModel<AdminEditProfileDataRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				// TODO: validate model

				if (InModel.Request.Data.AllPronouns != null) await _DatabaseService.DeleteMissingIdsAsync<PronounsModel>(new IdsModel { Ids = InModel.Request.Data.AllPronouns.Where(X => X.Id > 0).Select(X => X.Id) }, InConnection);
				if (InModel.Request.Data.AllSexes != null) await _DatabaseService.DeleteMissingIdsAsync<SexModel>(new IdsModel { Ids = InModel.Request.Data.AllSexes.Where(X => X.Id > 0).Select(X => X.Id) }, InConnection);
				if (InModel.Request.Data.AllGenders != null) await _DatabaseService.DeleteMissingIdsAsync<GenderModel>(new IdsModel { Ids = InModel.Request.Data.AllGenders.Where(X => X.Id > 0).Select(X => X.Id) }, InConnection);
				if (InModel.Request.Data.AllRegions != null) await _DatabaseService.DeleteMissingIdsAsync<RegionModel>(new IdsModel { Ids = InModel.Request.Data.AllRegions.Where(X => X.Id > 0).Select(X => X.Id) }, InConnection);
				if (InModel.Request.Data.AllCountries != null) await _DatabaseService.DeleteMissingIdsAsync<CountryModel>(new IdsModel { Ids = InModel.Request.Data.AllCountries.Where(X => X.Id > 0).Select(X => X.Id) }, InConnection);
				if (InModel.Request.Data.AllSexualities != null) await _DatabaseService.DeleteMissingIdsAsync<SexualityModel>(new IdsModel { Ids = InModel.Request.Data.AllSexualities.Where(X => X.Id > 0).Select(X => X.Id) }, InConnection);

				if (InModel.Request.Data.AllPronouns != null) await _DatabaseService.InsertModelsAsync<PronounsModel>(InModel.Request.Data.AllPronouns.Where(X => X.Id <= 0), InConnection);
				if (InModel.Request.Data.AllSexes != null) await _DatabaseService.InsertModelsAsync<SexModel>(InModel.Request.Data.AllSexes.Where(X => X.Id <= 0), InConnection);
				if (InModel.Request.Data.AllGenders != null) await _DatabaseService.InsertModelsAsync<GenderModel>(InModel.Request.Data.AllGenders.Where(X => X.Id <= 0), InConnection);
				if (InModel.Request.Data.AllRegions != null) await _DatabaseService.InsertModelsAsync<RegionModel>(InModel.Request.Data.AllRegions.Where(X => X.Id <= 0), InConnection);
				if (InModel.Request.Data.AllCountries != null) await _DatabaseService.InsertModelsAsync<CountryModel>(InModel.Request.Data.AllCountries.Where(X => X.Id <= 0), InConnection);
				if (InModel.Request.Data.AllSexualities != null) await _DatabaseService.InsertModelsAsync<SexualityModel>(InModel.Request.Data.AllSexualities.Where(X => X.Id <= 0), InConnection);

				return new AdminEditProfileDataResponseModel();
			});
		}
		public async Task<AdminEditAccountRolesResponseModel> EditAccountRolesAsync(ServiceModel<AdminEditAccountRolesRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			var Response = await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				var ChangedAccountRoleList = new List<AccountRoleModel>();
				var AccountIdList = new List<Int32>();

				foreach (var AccountRoleId in InModel.Request.RemovedIds)
				{
					var AccountRole = await _DatabaseService.GetModelByIdAsync(new AccountRoleModel { Id = AccountRoleId }, InConnection);
					if (AccountRole != null) ChangedAccountRoleList.Add(AccountRole);
				}

				foreach (var AccountRole in InModel.Request.NewAccountRoles)
					ChangedAccountRoleList.Add(AccountRole);

				foreach (var AccountRole in ChangedAccountRoleList)
				{
					var Account = await _DatabaseService.GetAccountByPublicIdAsync(AccountRole.AccountPublicId, InConnection);
					if (Account != null) AccountIdList.Add(Account.Id);
				}

				var RemovedAccountRoles = InModel.Request.RemovedIds.Select(X => new AccountRoleModel { Id = X });

				if (RemovedAccountRoles.Count() > 0 && await _DatabaseService.DeleteModelsAsync<AccountRoleModel>(RemovedAccountRoles, InConnection) <= 0)
					return new AdminEditAccountRolesResponseModel { Error = ErrorConstants.ERROR_DATABASE };
				if (InModel.Request.NewAccountRoles.Count() > 0 && await _DatabaseService.InsertModelsAsync<AccountRoleModel>(InModel.Request.NewAccountRoles, InConnection) <= 0)
					return new AdminEditAccountRolesResponseModel { Error = ErrorConstants.ERROR_DATABASE};

				var AccountIds = AccountIdList.Distinct();
				foreach (var AccountId in AccountIds) await _MainHubService.OnLocalUserChangedAsync(AccountId, InConnection);
				return new AdminEditAccountRolesResponseModel();
			});

			return Response;
		}
	}
}