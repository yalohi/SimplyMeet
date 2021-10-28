using SimplyMeetShared.Models;

namespace SimplyMeetShared.ResponseModels
{
	public class AdminGetProfileDataResponseModel : ResponseModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public ProfileDataModel ProfileData { get; set; }
		#endregion
	}
}