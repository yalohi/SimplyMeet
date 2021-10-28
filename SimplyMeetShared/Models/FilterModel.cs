using System;

namespace SimplyMeetShared.Models
{
	public class FilterModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 Id { get; set; }

		public Int32? PronounsId { get; set; }
		public Int32? SexId { get; set; }
		public Int32? GenderId { get; set; }
		public Int32? RegionId { get; set; }
		public Int32? CountryId { get; set; }

		public Int32 FromAge { get; set; }
		public Int32 ToAge { get; set; }
		public Boolean AgeEnabled { get; set; }
		#endregion
	}
}