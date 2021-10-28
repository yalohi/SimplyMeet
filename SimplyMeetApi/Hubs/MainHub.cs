using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SimplyMeetApi.Attributes;
using SimplyMeetApi.Enums;
using SimplyMeetApi.Models;
using SimplyMeetApi.Services;
using SimplyMeetShared.Constants;
using SimplyMeetShared.RequestModels;

namespace SimplyMeetApi.Hubs
{
	public class MainHub : Hub
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Fields
		private readonly AuthorizationService _AuthorizationService;
		private readonly MainHubService _MainHubService;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public MainHub(AuthorizationService InAuthorizationService, MainHubService InMainHubService)
		{
			_AuthorizationService = InAuthorizationService;
			_MainHubService = InMainHubService;
		}

		public override async Task OnConnectedAsync()
		{
			await base.OnConnectedAsync();
			await _MainHubService.OnConnectedAsync(Context);

		}
		public override async Task OnDisconnectedAsync(Exception InException)
		{
			await _MainHubService.OnDisconnectedAsync(Context);
			await base.OnDisconnectedAsync(InException);
		}

		[Authorize]
		[HubMethodName(nameof(MainHubConstants.REQUEST_CHAT_GET_HISTORY))]
		[Throttle(Group = EThrottleGroup.General)]
		public async Task ChatGetHistory(ChatGetHistoryRequestModel InRequestModel)
		{
			await _MainHubService.ChatGetHistoryAsync(new ServiceHubModel<ChatGetHistoryRequestModel> { Auth = await _AuthorizationService.GetHubAuthAsync(Context), Request = InRequestModel });
		}

		[Authorize]
		[HubMethodName(nameof(MainHubConstants.REQUEST_CHAT_SEND))]
		[Throttle(Group = EThrottleGroup.Chat)]
		public async Task ChatSend(ChatSendRequestModel InRequestModel)
		{
			await _MainHubService.ChatSendAsync(new ServiceHubModel<ChatSendRequestModel> { Auth = await _AuthorizationService.GetHubAuthAsync(Context), Request = InRequestModel });
		}
	}
}