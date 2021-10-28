using System;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Constants;
using SimplyMeetShared.Enums;

namespace SimplyMeetShared.RequestModels
{
	public class MatchGetChoicesRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		public EMatchChoice Choice { get; set; }

		[Required]
		[Range(0, Int32.MaxValue)]
		public Int32 Offset { get; set; }

		[Required]
		[Range(1, ApiRequestConstants.MATCH_GET_CHOICES_MAX_LOAD_COUNT)]
		public Int32 Count { get; set; }
		#endregion
	}
}