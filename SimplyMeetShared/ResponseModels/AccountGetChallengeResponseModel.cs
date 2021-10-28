using System;

namespace SimplyMeetShared.ResponseModels
{
	public class AccountGetChallengeResponseModel : ResponseModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Byte[] Nonce { get; set; }
		public Byte[] ServerPublicKey { get; set; }
		public Byte[] Challenge { get; set; }
		#endregion
	}
}