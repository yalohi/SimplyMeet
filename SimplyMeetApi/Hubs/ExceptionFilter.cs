using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace SimplyMeetApi.Hubs
{
	public class ExceptionFilter : IHubFilter
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Fields
		private readonly ILogger _Logger;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public ExceptionFilter(ILogger<ExceptionFilter> InLogger)
		{
			_Logger = InLogger;
		}

		public async ValueTask<Object> InvokeMethodAsync(HubInvocationContext InContext, Func<HubInvocationContext, ValueTask<Object>> InNext)
		{
			try
			{
				return await InNext(InContext);
			}

			catch (Exception Ex)
			{
				_Logger.LogError(Ex, Ex.Message);
				throw;
			}
		}
	}
}