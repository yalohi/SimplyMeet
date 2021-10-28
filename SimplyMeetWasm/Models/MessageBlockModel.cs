using System;
using System.Collections.Generic;

namespace SimplyMeetWasm.Models
{
	public class MessageBlockModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public List<DecryptedMessageModel> MessageList { get; set; }
		public Boolean IsNewDate { get; set; }
		#endregion
	}
}