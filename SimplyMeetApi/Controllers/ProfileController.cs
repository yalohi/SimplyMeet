namespace SimplyMeetApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly AuthorizationService _AuthorizationService;
	private readonly ProfileService _ProfileService;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public ProfileController(AuthorizationService InAuthorizationService, ProfileService InProfileService)
	{
		_AuthorizationService = InAuthorizationService;
		_ProfileService = InProfileService;
	}

	[HttpPost]
	[Route(nameof(Get))]
	[Throttle(Group = EThrottleGroup.General)]
	public async Task<IActionResult> Get(ProfileGetRequestModel InRequestModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ServiceModel<ProfileGetRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
		var Response = await _ProfileService.GetAsync(Model);
		return Ok(Response);
	}

	[HttpPost]
	[Route(nameof(GetCompact))]
	[Throttle(Group = EThrottleGroup.General)]
	public async Task<IActionResult> GetCompact(ProfileGetCompactRequestModel InRequestModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ServiceModel<ProfileGetCompactRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
		var Response = await _ProfileService.GetCompactAsync(Model);
		return Ok(Response);
	}

	[HttpPost]
	[Route(nameof(GetEditData))]
	[Throttle(Group = EThrottleGroup.General)]
	public async Task<IActionResult> GetEditData(ProfileGetEditDataRequestModel InRequestModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ServiceModel<ProfileGetEditDataRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
		var Response = await _ProfileService.GetEditDataAsync(Model);
		return Ok(Response);
	}

	[HttpPost]
	[Route(nameof(Edit))]
	[Throttle(Group = EThrottleGroup.General)]
	public async Task<IActionResult> Edit(ProfileEditRequestModel InRequestModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ServiceModel<ProfileEditRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
		var Response = await _ProfileService.EditAsync(Model);
		return Ok(Response);
	}

	[HttpPost]
	[Route(nameof(EditAvatar))]
	[Throttle(Group = EThrottleGroup.General)]
	public async Task<IActionResult> EditAvatar(IFormFile InFile)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ProfileEditAvatarModel { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), File = InFile };
		var Response = await _ProfileService.EditAvatarAsync(Model);
		return Ok(Response);
	}

	[HttpPost]
	[Route(nameof(ResetAvatar))]
	[Throttle(Group = EThrottleGroup.General)]
	public async Task<IActionResult> ResetAvatar(ProfileResetAvatarRequestModel InRequestModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ServiceModel<ProfileResetAvatarRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
		var Response = await _ProfileService.ResetAvatarAsync(Model);
		return Ok(Response);
	}

	[HttpPost]
	[Route(nameof(Report))]
	[Throttle(Group = EThrottleGroup.Report)]
	public async Task<IActionResult> Report(ProfileReportRequestModel InRequestModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var Model = new ServiceModel<ProfileReportRequestModel> { Auth = await _AuthorizationService.GetControllerAuthAsync(HttpContext), Request = InRequestModel };
		var Response = await _ProfileService.ReportAsync(Model);
		return Ok(Response);
	}
}