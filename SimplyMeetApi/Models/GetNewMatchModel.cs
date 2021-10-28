using System;
using SimplyMeetShared.Enums;

namespace SimplyMeetApi.Models
{
	public class GetNewMatchModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 AccountId { get; set; }

		public Int32? Profile_PronounsId { get; set; }
		public Int32? Profile_SexId { get; set; }
		public Int32? Profile_GenderId { get; set; }
		public Int32? Profile_RegionId { get; set; }
		public Int32? Profile_CountryId { get; set; }
		public Int32? Profile_Age { get; set; }
		public ELookingFor Profile_LookingFor { get; set; }

		public Int32? Filter_PronounsId { get; set; }
		public Int32? Filter_SexId { get; set; }
		public Int32? Filter_GenderId { get; set; }
		public Int32? Filter_RegionId { get; set; }
		public Int32? Filter_CountryId { get; set; }
		public Int32 Filter_FromAge { get; set; }
		public Int32 Filter_ToAge { get; set; }
		public Boolean Filter_AgeEnabled { get; set; }
		#endregion
	}
}