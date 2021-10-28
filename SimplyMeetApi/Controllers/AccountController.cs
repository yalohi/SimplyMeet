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
	public class AccountController : ControllerBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Fields
		private readonly AuthorizationService _AuthorizationService;
		private readonly AccountService _AccountService;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public AccountController(AuthorizationService InAuthorizationService, AccountService InAccountService)
		{
			_AuthorizationService = InAuthorizationService;
			_AccountService = InAccountService;
		}

		[HttpPost]
		[Route(nameof(GetChallenge))]
		[Throttle(Group = EThrottleGroup.General)]
		[AllowAnonymous]
		public async Task<IActionResult> GetChallenge(AccountGetChallengeRequestModel InRequestModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<AccountGetChallengeRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
			var Response = await _AccountService.GetChallengeAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(Login))]
		[Throttle(Group = EThrottleGroup.General, ErrorGroup = EThrottleGroup.AccountLogin)]
		[AllowAnonymous]
		public async Task<IActionResult> Login(AccountLoginRequestModel InRequestModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<AccountLoginRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
			var Response = await _AccountService.LoginAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(Create))]
		[Throttle(Group = EThrottleGroup.AccountCreate, ErrorGroup = EThrottleGroup.General)]
		[AllowAnonymous]
		public async Task<IActionResult> Create(AccountCreateRequestModel InRequestModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<AccountCreateRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
			var Response = await _AccountService.CreateAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(Delete))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> Delete(AccountDeleteRequestModel InRequestModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<AccountDeleteRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
			var Response = await _AccountService.DeleteAsync(Model);
			return Ok(Response);
		}
	}
}