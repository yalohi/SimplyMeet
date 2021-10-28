using System;

namespace SimplyMeetShared.Models
{
	public class ReportModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 Id { get; set; }
		public Int32 ReporterAccountId { get; set; }
		public Int32 ReportedAccountId { get; set; }
		public Int32 ReportReasonId { get; set; }
		#endregion
	}
}