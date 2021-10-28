using Microsoft.AspNetCore.Http;

namespace SimplyMeetApi.Models
{
	public class ProfileEditAvatarModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public AuthModel Auth { get; set; }
		public IFormFile File { get; set; }
		#endregion
	}
}