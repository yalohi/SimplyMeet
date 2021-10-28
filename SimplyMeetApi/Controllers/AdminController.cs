using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplyMeetApi.Attributes;
using SimplyMeetApi.Enums;
using SimplyMeetApi.Models;
using SimplyMeetApi.Services;
using SimplyMeetShared.RequestModels;

namespace SimplyMeetApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class AdminController : ControllerBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Fields
		private readonly AuthorizationService _AuthorizationService;
		private readonly AdminService _AdminService;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public AdminController(AuthorizationService InAuthorizationService, AdminService InAdminService)
		{
			_AuthorizationService = InAuthorizationService;
			_AdminService = InAdminService;
		}

		[HttpPost]
		[Route(nameof(GetReportedProfiles))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> GetReportedProfiles(AdminGetReportedProfilesRequestModel InRequestModel)
		{
			var Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext, true);
			if (!_AuthorizationService.HasRole(Auth, EAccountRole.Admin) && !_AuthorizationService.HasRole(Auth, EAccountRole.Moderator)) return Unauthorized();
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<AdminGetReportedProfilesRequestModel> { Auth = Auth, Request = InRequestModel };
			var Response = await _AdminService.GetReportedProfilesAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(GetProfileData))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> GetProfileData(AdminGetProfileDataRequestModel InRequestModel)
		{
			var Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext, true);
			if (!_AuthorizationService.HasRole(Auth, EAccountRole.Admin)) return Unauthorized();
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<AdminGetProfileDataRequestModel> { Auth = Auth, Request = InRequestModel };
			var Response = await _AdminService.GetProfileDataAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(GetAccountRoles))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> GetAccountRoles(AdminGetAccountRolesRequestModel InRequestModel)
		{
			var Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext, true);
			if (!_AuthorizationService.HasRole(Auth, EAccountRole.Admin)) return Unauthorized();
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<AdminGetAccountRolesRequestModel> { Auth = Auth, Request = InRequestModel };
			var Response = await _AdminService.GetAccountRolesAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(SuspendAccount))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> SuspendAccount(AdminSuspendAccountRequestModel InRequestModel)
		{
			var Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext, true);
			if (!_AuthorizationService.HasRole(Auth, EAccountRole.Admin) && !_AuthorizationService.HasRole(Auth, EAccountRole.Moderator)) return Unauthorized();
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<AdminSuspendAccountRequestModel> { Auth = Auth, Request = InRequestModel };
			var Response = await _AdminService.SuspendAccountAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(EditProfileData))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> EditProfileData(AdminEditProfileDataRequestModel InRequestModel)
		{
			var Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext, true);
			if (!_AuthorizationService.HasRole(Auth, EAccountRole.Admin)) return Unauthorized();
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<AdminEditProfileDataRequestModel> { Auth = Auth, Request = InRequestModel };
			var Response = await _AdminService.EditProfileDataAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(EditAccountRoles))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> EditAccountRoles(AdminEditAccountRolesRequestModel InRequestModel)
		{
			var Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext, true);
			if (!_AuthorizationService.HasRole(Auth, EAccountRole.Admin)) return Unauthorized();
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<AdminEditAccountRolesRequestModel> { Auth = Auth, Request = InRequestModel };
			var Response = await _AdminService.EditAccountRolesAsync(Model);
			return Ok(Response);
		}
	}
}