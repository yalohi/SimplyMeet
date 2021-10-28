using System;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Constants;

namespace SimplyMeetShared.RequestModels
{
	public class MatchEditFilterRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Range(0, Int32.MaxValue)]
		public Int32? PronounsId { get; set; }
		[Range(0, Int32.MaxValue)]
		public Int32? SexId { get; set; }
		[Range(0, Int32.MaxValue)]
		public Int32? GenderId { get; set; }
		[Range(0, Int32.MaxValue)]
		public Int32? RegionId { get; set; }
		[Range(0, Int32.MaxValue)]
		public Int32? CountryId { get; set; }

		[Required]
		[Range(ProfileConstants.MIN_AGE, ProfileConstants.MAX_AGE)]
		public Int32 FromAge { get; set; }
		[Required]
		[Range(ProfileConstants.MIN_AGE, ProfileConstants.MAX_AGE)]
		public Int32 ToAge { get; set; }
		[Required]
		public Boolean AgeEnabled { get; set; }
		#endregion
	}
}