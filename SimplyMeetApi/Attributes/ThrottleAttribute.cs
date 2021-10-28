using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SimplyMeetApi.Enums;
using SimplyMeetApi.Services;
using SimplyMeetShared.Constants;
using SimplyMeetShared.ResponseModels;

namespace SimplyMeetApi.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ThrottleAttribute : ActionFilterAttribute
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public EThrottleGroup Group { get; set; }
		public EThrottleGroup ErrorGroup { get; set; }
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public override async Task OnActionExecutionAsync(ActionExecutingContext InContext, ActionExecutionDelegate InNext)
		{
			ActionExecutedContext ExecutedContext = null;
			var ThrottleService = InContext.HttpContext.RequestServices.GetService<ThrottleService>();

			if (!ThrottleService.IsThrottled(InContext.HttpContext.Connection.RemoteIpAddress, Group)) ExecutedContext = await InNext();

			var Result = ExecutedContext?.Result;
			if (Result == null) Result = InContext.Result = new ObjectResult(new ErrorResponseModel { Error = ErrorConstants.ERROR_TOO_MANY_REQUESTS });

			var ResponseModel = (Result as ObjectResult)?.Value as ResponseModelBase;
			var ThrottleGroup = (ResponseModel != null && String.IsNullOrEmpty(ResponseModel.Error)) ? Group : ErrorGroup;
			ThrottleService.IncrementRequestCount(InContext.HttpContext.Connection.RemoteIpAddress, ThrottleGroup);
		}
	}
}