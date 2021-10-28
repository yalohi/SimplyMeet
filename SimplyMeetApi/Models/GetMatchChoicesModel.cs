using System;
using SimplyMeetShared.Enums;

namespace SimplyMeetApi.Models
{
	public class GetMatchChoicesModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 AccountId { get; set; }
		public Int32 Offset { get; set; }
		public Int32 Count { get; set; }
		public EMatchChoice Choice { get; set; }
		#endregion
	}
}