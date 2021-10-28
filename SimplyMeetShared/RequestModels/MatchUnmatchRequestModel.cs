using System;
using System.ComponentModel.DataAnnotations;

namespace SimplyMeetShared.RequestModels
{
	public class MatchUnmatchRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		[Range(0, Int32.MaxValue)]
		public Int32 MatchId { get; set; }
		#endregion
	}
}