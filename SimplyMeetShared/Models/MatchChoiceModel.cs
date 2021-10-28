using System;
using SimplyMeetShared.Enums;

namespace SimplyMeetShared.Models
{
	public class MatchChoiceModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 Id { get; set; }
		public Int32 AccountId { get; set; }
		public Int32 MatchAccountId { get; set; }
		public EMatchChoice Choice { get; set; }
		#endregion
	}
}