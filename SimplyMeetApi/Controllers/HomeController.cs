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
	public class HomeController : ControllerBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Fields
		private readonly AuthorizationService _AuthorizationService;
		private readonly HomeService _HomeService;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public HomeController(AuthorizationService InAuthorizationService, HomeService InHomeService)
		{
			_AuthorizationService = InAuthorizationService;
			_HomeService = InHomeService;
		}

		[HttpPost]
		[Route(nameof(GetData))]
		[Throttle(Group = EThrottleGroup.General)]
		[AllowAnonymous]
		public async Task<IActionResult> GetData(HomeGetDataRequestModel InRequestModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var Model = new ServiceModel<HomeGetDataRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
			var Response = await _HomeService.GetDataAsync(Model);
			return Ok(Response);
		}
	}
}