using System;

namespace SimplyMeetShared.Models
{
	public class MessageModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 Id { get; set; }
		public Int32 MatchId { get; set; }
		// public Int32 ToPublicKeyId { get; set; }
		public String FromPublicId { get; set; }

		public String ServerDataJson { get; set; }
		public String ClientDataJson_Encrypted_Base64 { get; set; }
		public String ClientDataJson_Nonce_Base64 { get; set; }
		#endregion
	}
}