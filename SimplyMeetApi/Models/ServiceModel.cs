using SimplyMeetShared.RequestModels;

namespace SimplyMeetApi.Models
{
	public class ServiceModel<TRequest> where TRequest : IRequestModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public AuthModel Auth { get; set; }
		public TRequest Request { get; set; }
		#endregion
	}
}