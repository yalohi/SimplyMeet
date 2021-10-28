using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SimplyMeetShared.Attributes;
using SimplyMeetShared.Constants;
using SimplyMeetShared.Enums;
using SimplyMeetShared.Models;

namespace SimplyMeetShared.RequestModels
{
	public class ProfileEditRequestModel : RequestModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Required]
		[MaxLength(ProfileConstants.MAX_DISPLAY_NAME_LENGTH)]
		public String DisplayName { get; set; }

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

		[Age(Min = ProfileConstants.MIN_AGE, Max = ProfileConstants.MAX_AGE)]
		public DateTime? BirthDate { get; set; }

		public IEnumerable<TagModel> Tags { get; set; }
		public IEnumerable<SexualityModel> Sexualities { get; set; }

		[Required]
		public ELookingFor LookingFor { get; set; }

		[MaxLength(ProfileConstants.MAX_ABOUT_LENGTH)]
		public String AboutMe { get; set; }

		[MaxLength(ProfileConstants.MAX_ABOUT_LENGTH)]
		public String AboutYou { get; set; }
		#endregion
	}
}