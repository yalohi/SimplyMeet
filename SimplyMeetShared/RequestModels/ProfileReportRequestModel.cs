using System;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Constants;

namespace SimplyMeetShared.RequestModels
{
	public class ProfileReportRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		[MinLength(AccountConstants.PUBLIC_ID_LENGTH)]
		[MaxLength(AccountConstants.PUBLIC_ID_LENGTH)]
		public String AccountPublicId { get; set; }

		[Required]
		[Range(0, Int32.MaxValue)]
		public Int32 ReportReasonId { get; set; }
		#endregion
	}
}