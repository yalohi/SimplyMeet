using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SimplyMeetApi.Middleware
{
	public class ExceptionHandlerMiddleware
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Fields
		private readonly RequestDelegate _Next;
		private readonly ILogger _Logger;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public ExceptionHandlerMiddleware(RequestDelegate InNext, ILogger<ExceptionHandlerMiddleware> InLogger)
		{
			_Next = InNext;
			_Logger = InLogger;
		}

		public async Task Invoke(HttpContext InContext)
		{
			try
			{
				await _Next.Invoke(InContext);
			}

			catch (Exception Ex)
			{
				_Logger.LogError(Ex, Ex.Message);
				InContext.Response.StatusCode = (Int32)HttpStatusCode.InternalServerError;
			}
		}
	}
}