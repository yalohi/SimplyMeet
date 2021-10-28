using System;
using Microsoft.AspNetCore.Authentication;

namespace SimplyMeetApi.Models
{
	public class TokenModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public DateTime Created { get; set; }
		public AuthenticationTicket Ticket { get; set; }
		#endregion
	}
}