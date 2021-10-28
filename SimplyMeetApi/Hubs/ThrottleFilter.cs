using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SimplyMeetApi.Attributes;
using SimplyMeetApi.Services;
using SimplyMeetShared.Constants;

namespace SimplyMeetApi.Hubs
{
	public class ThrottleFilter : IHubFilter
	{
		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public async ValueTask<Object> InvokeMethodAsync(HubInvocationContext InContext, Func<HubInvocationContext, ValueTask<Object>> InNext)
		{
			var ThrottleAttribute = Attribute.GetCustomAttribute(InContext.HubMethod, typeof(ThrottleAttribute)) as ThrottleAttribute;
			var HttpContext = InContext.Context.GetHttpContext();

			Object Result = null;
			if (ThrottleAttribute != null && HttpContext != null)
			{
				var ThrottleService = HttpContext.RequestServices.GetService<ThrottleService>();
				if (!ThrottleService.IsThrottled(HttpContext.Connection.RemoteIpAddress, ThrottleAttribute.Group)) Result = await InNext(InContext);
				else await InContext.Hub.Clients.Client(InContext.Context.ConnectionId).SendAsync(MainHubConstants.RECEIVE_THROTTLE);
				ThrottleService.IncrementRequestCount(HttpContext.Connection.RemoteIpAddress, ThrottleAttribute.Group);
			}

			return Result;
		}
	}
}