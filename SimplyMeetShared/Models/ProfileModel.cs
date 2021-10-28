
using System;
using SimplyMeetShared.Enums;

namespace SimplyMeetShared.Models
{
	public class ProfileModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Int32 Id { get; set; }

		public String Avatar { get; set; }
		public String DisplayName { get; set; }

		public Int32? PronounsId { get; set; }
		public Int32? SexId { get; set; }
		public Int32? GenderId { get; set; }
		public Int32? RegionId { get; set; }
		public Int32? CountryId { get; set; }

		public DateTime? BirthDate { get; set; }

		public ELookingFor LookingFor { get; set; }

		public String AboutMe { get; set; }
		public String AboutYou { get; set; }
		#endregion
	}
}