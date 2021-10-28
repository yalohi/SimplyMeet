using SimplyMeetShared.RequestModels;

namespace SimplyMeetApi.Models
{
	public class ServiceHubModel<TRequest> where TRequest : IRequestModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public AuthHubModel Auth { get; set; }
		public TRequest Request { get; set; }
		#endregion
	}
}