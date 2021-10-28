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
	public class MatchController : ControllerBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Fields
		private readonly AuthorizationService _AuthorizationService;
		private readonly MatchService _MatchService;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public MatchController(AuthorizationService InAuthorizationService, MatchService InMatchService)
		{
			_AuthorizationService = InAuthorizationService;
			_MatchService = InMatchService;
		}

		[HttpPost]
		[Route(nameof(GetNew))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> GetNew(MatchGetNewRequestModel InRequestModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<MatchGetNewRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
			var Response = await _MatchService.GetNewAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(GetChoices))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> GetChoices(MatchGetChoicesRequestModel InRequestModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<MatchGetChoicesRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
			var Response = await _MatchService.GetChoicesAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(GetFilter))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> GetFilter(MatchGetFilterRequestModel InRequestModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<MatchGetFilterRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
			var Response = await _MatchService.GetFilterAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(EditFilter))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> EditFilter(MatchEditFilterRequestModel InRequestModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<MatchEditFilterRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
			var Response = await _MatchService.EditFilterAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(Choose))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> Choose(MatchChooseRequestModel InRequestModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<MatchChooseRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
			var Response = await _MatchService.ChooseAsync(Model);
			return Ok(Response);
		}

		[HttpPost]
		[Route(nameof(Unmatch))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task<IActionResult> Unmatch(MatchUnmatchRequestModel InRequestModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<MatchUnmatchRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
			var Response = await _MatchService.UnmatchAsync(Model);
			return Ok(Response);
		}
	}
}