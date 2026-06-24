namespace SimplyMeetApi.Controllers;

[ApiController]
public class RootController : ControllerBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly HomeConfiguration _HomeConfig;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public RootController(IOptions<HomeConfiguration> InHomeConfig)
	{
		_HomeConfig = InHomeConfig.Value;
	}

	[HttpGet]
	[Route("")]
	public IActionResult Index()
	{
		return Redirect(_HomeConfig.RootRedirectUrl);
	}
}
