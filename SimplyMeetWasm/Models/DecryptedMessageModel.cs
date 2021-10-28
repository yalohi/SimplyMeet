using System;
using SimplyMeetShared.Models;

namespace SimplyMeetWasm.Models
{
	public class DecryptedMessageModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 Id { get; set; }
		public ProfileCompactModel Sender { get; set; }
		public MessageServerDataModel ServerData { get; set; }
		public MessageClientDataModel ClientData { get; set; }
		#endregion
	}
}