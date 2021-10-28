using System;
using System.Net;

namespace SimplyMeetWasm.Constants
{
	public static class TokenConstants
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Const Fields
		public const String TYPE = "Bearer";
		public const String WS_QUERY = "access_token";
		#endregion
		#region Static Fields
		public static readonly String HEADER = HttpRequestHeader.Authorization.ToString();
		#endregion
	}
}