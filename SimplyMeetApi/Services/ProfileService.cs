using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimplyMeetApi.Configuration;
using SimplyMeetApi.Models;
using SimplyMeetShared.Constants;
using SimplyMeetShared.Enums;
using SimplyMeetShared.Extensions;
using SimplyMeetShared.Models;
using SimplyMeetShared.RequestModels;
using SimplyMeetShared.ResponseModels;

namespace SimplyMeetApi.Services
{
	public class ProfileService
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Fields
		private readonly ILogger _Logger;
		private readonly DatabaseService _DatabaseService;
		private readonly MainHubService _MainHubService;
		private readonly ProfileCompactService _ProfileCompactService;
		private readonly StaticFilesConfiguration _StaticFilesConfig;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public ProfileService(ILogger<ProfileService> InLogger, DatabaseService InDatabaseService, MainHubService InMainHubService, ProfileCompactService InProfileCompactService, IOptions<StaticFilesConfiguration> InStaticFilesConfig)
		{
			_Logger = InLogger;
			_DatabaseService = InDatabaseService;
			_MainHubService = InMainHubService;
			_ProfileCompactService = InProfileCompactService;
			_StaticFilesConfig = InStaticFilesConfig.Value;
		}

		public async Task<ProfileGetResponseModel> GetAsync(ServiceModel<ProfileGetRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				var Account = await _DatabaseService.GetAccountByPublicIdAsync(InModel.Request.AccountPublicId, InConnection);
				if (Account == null) return new ProfileGetResponseModel { Error = ErrorConstants.ERROR_NO_SUCH_ACCOUNT };

				return new ProfileGetResponseModel
				{
					FullProfile = await GetFullProfileFromAccountAsync(InModel.Auth, Account, true, InConnection)
				};
			});
		}
		public async Task<ProfileGetCompactResponseModel> GetCompactAsync(ServiceModel<ProfileGetCompactRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				var Account = await _DatabaseService.GetAccountByPublicIdAsync(InModel.Request.PublicId, InConnection);
				if (Account == null) return new ProfileGetCompactResponseModel { Error = ErrorConstants.ERROR_NO_SUCH_ACCOUNT };

				var CompactProfile = await _ProfileCompactService.GetFromAccountAsync(Account, EMatchChoice.None, InConnection);
				return new ProfileGetCompactResponseModel { CompactProfile = CompactProfile };
			});
		}
		public async Task<ProfileGetEditDataResponseModel> GetEditDataAsync(ServiceModel<ProfileGetEditDataRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
				if (Account == null) return new ProfileGetEditDataResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				return new ProfileGetEditDataResponseModel
				{
					FullProfile = await GetFullProfileFromAccountAsync(InModel.Auth, Account, false, InConnection),
				};
			});
		}
		public async Task<ProfileEditResponseModel> EditAsync(ServiceModel<ProfileEditRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
				if (Account == null) return new ProfileEditResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				var Profile = await _DatabaseService.GetModelByIdAsync(new ProfileModel { Id = Account.ProfileId }, InConnection);
				if (Profile == null) return new ProfileEditResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				// validate request data
				var ProfileData = await GetProfileDataAsync(InConnection);

				var Pronouns = InModel.Request.PronounsId != null ? ProfileData.AllPronouns.FirstOrDefault(X => X.Id == InModel.Request.PronounsId) : null;
				var Sex = InModel.Request.SexId != null ? ProfileData.AllSexes.FirstOrDefault(X => X.Id == InModel.Request.SexId) : null;
				var Gender = InModel.Request.GenderId != null ? ProfileData.AllGenders.FirstOrDefault(X => X.Id == InModel.Request.GenderId) : null;
				var Region = InModel.Request.RegionId != null ? ProfileData.AllRegions.FirstOrDefault(X => X.Id == InModel.Request.RegionId) : null;
				var Country = InModel.Request.CountryId != null ? ProfileData.AllCountries.FirstOrDefault(X => X.Id == InModel.Request.CountryId) : null;

				foreach (var Tag in InModel.Request.Tags) Tag.Name = Tag.Name.ToUpper().Truncate(ProfileConstants.MAX_TAG_NAME_LENGTH);
				InModel.Request.Tags = InModel.Request.Tags.Take(ProfileConstants.MAX_TAGS).Distinct(X => X.Name);
				InModel.Request.Sexualities = InModel.Request.Sexualities.Take(ProfileConstants.MAX_SEXUALITIES).Distinct(X => X.Name);

				// update profile
				Profile.DisplayName = InModel.Request.DisplayName;
				Profile.PronounsId = Pronouns?.Id;
				Profile.SexId = Sex?.Id;
				Profile.GenderId = Gender?.Id;
				Profile.RegionId = Country != null ? Country.RegionId : Region?.Id;
				Profile.CountryId = Country?.Id;
				Profile.BirthDate = InModel.Request.BirthDate;
				Profile.LookingFor = InModel.Request.LookingFor;
				Profile.AboutMe = InModel.Request.AboutMe;
				Profile.AboutYou = InModel.Request.AboutYou;

				// update database
				if (await _DatabaseService.UpdateModelByIdAsync(Profile, InConnection) <= 0)
					return new ProfileEditResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				await _DatabaseService.DeleteModelAsync(new ProfileTagModel { ProfileId = Profile.Id }, InConnection);
				foreach (var Tag in InModel.Request.Tags)
				{
					var TagWithId = await _DatabaseService.GetTagByNameAsync(Tag.Name, InConnection) ?? new TagModel { Name = Tag.Name };

					if (TagWithId.Id <= 0 && (TagWithId.Id = await _DatabaseService.InsertModelReturnIdAsync(Tag, InConnection)) <= 0)
						return new ProfileEditResponseModel { Error = ErrorConstants.ERROR_DATABASE };
					if (await _DatabaseService.InsertModelAsync(new ProfileTagModel { ProfileId = Profile.Id, TagId = TagWithId.Id }, InConnection) <= 0)
						return new ProfileEditResponseModel { Error = ErrorConstants.ERROR_DATABASE };
				}

				await _DatabaseService.DeleteModelAsync(new ProfileSexualityModel { ProfileId = Profile.Id }, InConnection);
				foreach (var Sexuality in InModel.Request.Sexualities)
				{
					if (await _DatabaseService.InsertModelAsync(new ProfileSexualityModel { ProfileId = Profile.Id, SexualityId = Sexuality.Id }, InConnection) <= 0)
						return new ProfileEditResponseModel { Error = ErrorConstants.ERROR_DATABASE };
				}

				await _MainHubService.SendUserUpdateAsync(Account, InConnection);
				return new ProfileEditResponseModel();
			});
		}
		public async Task<ProfileEditAvatarResponseModel> EditAvatarAsync(ProfileEditAvatarModel InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			if (InModel.File.Length > ProfileConstants.MAX_AVATAR_SIZE) return new ProfileEditAvatarResponseModel { Error = ErrorConstants.ERROR_INVALID_OPERATION };

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
				if (Account == null) return new ProfileEditAvatarResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				var Profile = await _DatabaseService.GetModelByIdAsync(new ProfileModel { Id = Account.ProfileId }, InConnection);
				if (Profile == null) return new ProfileEditAvatarResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				var Hash = String.Empty;
				using (var Stream = InModel.File.OpenReadStream())
				{
					var SHA = SHA256.Create();
					Hash = Convert.ToHexString(await SHA.ComputeHashAsync(Stream));
					Hash = Hash.Substring(Hash.Length - ProfileConstants.AVATAR_HASH_LENGTH).ToLower();
				}

				var AvatarFileName = GetAvatarFileName(Account, Hash);

				try
				{
					using (var AvatarInputStream = InModel.File.OpenReadStream())
						using (var Bitmap = SkiaSharp.SKBitmap.Decode(AvatarInputStream))
							using (var AvatarFileStream = File.Create(AvatarFileName))
								Bitmap.Encode(AvatarFileStream, SkiaSharp.SKEncodedImageFormat.Webp, ProfileConstants.AVATAR_QUALITY);
				}

				catch (Exception Ex)
				{
					_Logger.LogError(Ex, Ex.Message);
					return new ProfileEditAvatarResponseModel { Error = ErrorConstants.ERROR_INVALID_OPERATION };
				}

				var LastAvatarFileName = GetAvatarFileName(Profile.Avatar);
				if (!LastAvatarFileName.EndsWith(ProfileConstants.DEFAULT_AVATAR))
				{
					try { if (File.Exists(LastAvatarFileName)) File.Delete(LastAvatarFileName); }
					catch (IOException) { return new ProfileEditAvatarResponseModel { Error = ErrorConstants.ERROR_IO }; }
				}

				Profile.Avatar = Path.GetFileName(AvatarFileName);
				if (await _DatabaseService.UpdateModelByIdAsync(Profile, InConnection) <= 0) return new ProfileEditAvatarResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				await _MainHubService.SendUserUpdateAsync(Account, InConnection);
				return new ProfileEditAvatarResponseModel();
			});
		}
		public async Task<ProfileResetAvatarResponseModel> ResetAvatarAsync(ServiceModel<ProfileResetAvatarRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
				if (Account == null) return new ProfileResetAvatarResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				var Profile = await _DatabaseService.GetModelByIdAsync(new ProfileModel { Id = Account.ProfileId }, InConnection);
				if (Profile.Avatar == ProfileConstants.DEFAULT_AVATAR) return new ProfileResetAvatarResponseModel { Error = ErrorConstants.ERROR_INVALID_OPERATION };

				var AvatarFileName = GetAvatarFileName(Profile.Avatar);
				try { if (File.Exists(AvatarFileName)) File.Delete(AvatarFileName); }
				catch (IOException) { return new ProfileResetAvatarResponseModel { Error = ErrorConstants.ERROR_IO }; }

				Profile.Avatar = ProfileConstants.DEFAULT_AVATAR;
				if (await _DatabaseService.UpdateModelByIdAsync(Profile, InConnection) <= 0) return new ProfileResetAvatarResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				await _MainHubService.SendUserUpdateAsync(Account, InConnection);
				return new ProfileResetAvatarResponseModel();
			});
		}
		public async Task<ProfileReportResponseModel> ReportAsync(ServiceModel<ProfileReportRequestModel> InModel)
		{
			if (InModel == null) throw new ArgumentNullException(nameof(InModel));

			return await _DatabaseService.PerformTransactionAsync(async InConnection =>
			{
				var Account = await _DatabaseService.GetModelByIdAsync(new AccountModel { Id = InModel.Auth.AccountId }, InConnection);
				if (Account == null) return new ProfileReportResponseModel { Error = ErrorConstants.ERROR_DATABASE };
				if (Account.Status == EAccountStatus.Suspended) return new ProfileReportResponseModel { Error = ErrorConstants.ERROR_ACCOUNT_SUSPENDED };

				var ReportedAccount = await _DatabaseService.GetAccountByPublicIdAsync(InModel.Request.AccountPublicId, InConnection);
				if (ReportedAccount == null) return new ProfileReportResponseModel { Error = ErrorConstants.ERROR_NO_SUCH_ACCOUNT };
				if (ReportedAccount.Status == EAccountStatus.Suspended) return new ProfileReportResponseModel { Error = ErrorConstants.ERROR_INVALID_OPERATION };

				var Report = await _DatabaseService.GetReportAsync(new ReportModel { ReporterAccountId = InModel.Auth.AccountId, ReportedAccountId = ReportedAccount.Id }, InConnection);
				if (Report != null) return new ProfileReportResponseModel { Error = ErrorConstants.ERROR_INVALID_OPERATION };

				var NewReport = new ReportModel { ReporterAccountId = InModel.Auth.AccountId, ReportedAccountId = ReportedAccount.Id, ReportReasonId = InModel.Request.ReportReasonId };
				if (await _DatabaseService.InsertModelAsync(NewReport, InConnection) <= 0) return new ProfileReportResponseModel { Error = ErrorConstants.ERROR_DATABASE };

				return new ProfileReportResponseModel();
			});
		}

		public async Task<ProfileFullModel> GetFullProfileFromAccountAsync(AuthModel InAuth, AccountModel InAccount, Boolean InIsPublic, IDbConnection InConnection)
		{
			if (InAuth == null) throw new ArgumentNullException(nameof(InAuth));
			if (InAccount == null) throw new ArgumentNullException(nameof(InAccount));
			if (InConnection == null) throw new ArgumentNullException(nameof(InConnection));

			var AccountFlags = EAccountFlags.None;
			if ((DateTime.UtcNow - InAccount.Creation).TotalDays <= AccountConstants.FLAG_NEW_DAYS) AccountFlags |= EAccountFlags.New;
			if (InAccount.LastLogin != null && (DateTime.UtcNow - InAccount.LastLogin.Value).TotalDays <= AccountConstants.FLAG_ACTIVE_DAYS) AccountFlags |= EAccountFlags.Active;

			var Profile = await _DatabaseService.GetModelByIdAsync(new ProfileModel { Id = InAccount.ProfileId }, InConnection);
			if (Profile == null) return null;

			var Filter = await _DatabaseService.GetModelByIdAsync(new FilterModel { Id = InAccount.FilterId }, InConnection);
			if (Filter == null) return null;

			var FullProfile = new ProfileFullModel
			{
				AccountFlags = AccountFlags,
				Data = await GetProfileDataAsync(InConnection),

				Account = InAccount,
				Profile = Profile,
				Filter = Filter,

				Tags = await _DatabaseService.GetProfileTagsAsync(Profile, InConnection),
				Sexualities = await _DatabaseService.GetProfileSexualitiesAsync(Profile, InConnection),

				ReportReasons = await _DatabaseService.GetAllAsync<ReportReasonModel>(InConnection),
				Reported = (await _DatabaseService.GetReportAsync(new ReportModel { ReporterAccountId = InAuth.AccountId, ReportedAccountId = InAccount.Id }, InConnection) != null),
			};

			if (InIsPublic)
			{
				FullProfile.Account.Id = -1;
				FullProfile.Account.PublicKey_Base64 = null;
				FullProfile.Account.Creation = DateTime.UnixEpoch;
				FullProfile.Account.LastLogin = null;
				FullProfile.Filter = null;
			}

			return FullProfile;
		}
		public async Task<ProfileDataModel> GetProfileDataAsync(IDbConnection InConnection)
		{
			if (InConnection == null) throw new ArgumentNullException(nameof(InConnection));

			return new ProfileDataModel
			{
				AllPronouns = await _DatabaseService.GetAllAsync<PronounsModel>(InConnection),
				AllSexes = await _DatabaseService.GetAllAsync<SexModel>(InConnection),
				AllGenders = await _DatabaseService.GetAllAsync<GenderModel>(InConnection),
				AllRegions = await _DatabaseService.GetAllAsync<RegionModel>(InConnection),
				AllCountries = await _DatabaseService.GetAllAsync<CountryModel>(InConnection),
				AllSexualities = await _DatabaseService.GetAllAsync<SexualityModel>(InConnection),
			};
		}

		//===========================================================================================
		// Private Methods
		//===========================================================================================
		private String GetAvatarFileName(String InFileName) => $"{Path.TrimEndingDirectorySeparator(_StaticFilesConfig.AvatarsPath)}/{InFileName}";
		private String GetAvatarFileName(AccountModel InAccount, String InHash) => $"{Path.TrimEndingDirectorySeparator(_StaticFilesConfig.AvatarsPath)}/{InAccount.PublicId}_{InHash}.{ProfileConstants.AVATAR_EXTENSION}";
	}
}