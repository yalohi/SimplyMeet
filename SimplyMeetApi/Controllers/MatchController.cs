namespace SimplyMeetApi.Controllers;

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

		var Model = new ServiceModel<MatchGetNewRequestModel>(await _AuthorizationService.GetControllerAuthAsync(HttpContext), InRequestModel);
		var Response = await _MatchService.GetNewAsync(Model);
		return Ok(Response);
	}

	[HttpPost]
	[Route(nameof(GetChoices))]
	[Throttle(Group = EThrottleGroup.General)]
	public async Task<IActionResult> GetChoices(MatchGetChoicesRequestModel InRequestModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ServiceModel<MatchGetChoicesRequestModel>(await _AuthorizationService.GetControllerAuthAsync(HttpContext), InRequestModel);
		var Response = await _MatchService.GetChoicesAsync(Model);
		return Ok(Response);
	}

	[HttpPost]
	[Route(nameof(GetFilter))]
	[Throttle(Group = EThrottleGroup.General)]
	public async Task<IActionResult> GetFilter(MatchGetFilterRequestModel InRequestModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ServiceModel<MatchGetFilterRequestModel>(await _AuthorizationService.GetControllerAuthAsync(HttpContext), InRequestModel);
		var Response = await _MatchService.GetFilterAsync(Model);
		return Ok(Response);
	}

	[HttpPost]
	[Route(nameof(EditFilter))]
	[Throttle(Group = EThrottleGroup.General)]
	public async Task<IActionResult> EditFilter(MatchEditFilterRequestModel InRequestModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ServiceModel<MatchEditFilterRequestModel>(await _AuthorizationService.GetControllerAuthAsync(HttpContext), InRequestModel);
		var Response = await _MatchService.EditFilterAsync(Model);
		return Ok(Response);
	}

	[HttpPost]
	[Route(nameof(Choose))]
	[Throttle(Group = EThrottleGroup.General)]
	public async Task<IActionResult> Choose(MatchChooseRequestModel InRequestModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ServiceModel<MatchChooseRequestModel>(await _AuthorizationService.GetControllerAuthAsync(HttpContext), InRequestModel);
		var Response = await _MatchService.ChooseAsync(Model);
		return Ok(Response);
	}

	[HttpPost]
	[Route(nameof(Unmatch))]
	[Throttle(Group = EThrottleGroup.General)]
	public async Task<IActionResult> Unmatch(MatchUnmatchRequestModel InRequestModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ServiceModel<MatchUnmatchRequestModel>(await _AuthorizationService.GetControllerAuthAsync(HttpContext), InRequestModel);
		var Response = await _MatchService.UnmatchAsync(Model);
		return Ok(Response);
	}
}