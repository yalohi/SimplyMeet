using System;
using System.Net;

namespace SimplyMeetShared.ResponseModels
{
	public abstract class ResponseModelBase : IResponseModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public String Error { get; set; }
		public HttpStatusCode? ErrorCode { get; set; }
		#endregion
	}
}