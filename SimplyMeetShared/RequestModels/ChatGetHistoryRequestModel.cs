using System;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Constants;

namespace SimplyMeetShared.RequestModels
{
	public class ChatGetHistoryRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		[Range(0, Int32.MaxValue)]
		public Int32 MatchId { get; set; }

		[Required]
		[Range(0, Int32.MaxValue)]
		public Int32 StartingMessageId { get; set; }

		[Required]
		[Range(1, MainHubConstants.CHAT_MAX_LOAD_MESSAGE_COUNT)]
		public Int32 MessageCount { get; set; }

		[Required]
		public Boolean Forward { get; set; }
		#endregion
	}
}