using System;
using System.Collections.Generic;
using SimplyMeetShared.Models;

namespace SimplyMeetShared.SendModels
{
	public class ChatHistorySendModel : SendModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public IEnumerable<MessageModel> Messages { get; set; }
		public Int32 MatchId { get; set; }
		public Int32 RemainingMessageCount { get; set; }
		public Boolean Forward { get; set; }
		#endregion
	}
}