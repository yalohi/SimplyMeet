using System;

namespace SimplyMeetShared.ResponseModels
{
	public class AccountLoginResponseModel : ResponseModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public String Token { get; set; }
		#endregion
	}
}