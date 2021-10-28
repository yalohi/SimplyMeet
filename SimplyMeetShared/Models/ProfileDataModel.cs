using System.Collections.Generic;

namespace SimplyMeetShared.Models
{
	public class ProfileDataModel
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public IEnumerable<PronounsModel> AllPronouns { get; set; }
		public IEnumerable<SexModel> AllSexes { get; set; }
		public IEnumerable<GenderModel> AllGenders { get; set; }
		public IEnumerable<RegionModel> AllRegions { get; set; }
		public IEnumerable<CountryModel> AllCountries { get; set; }
		public IEnumerable<SexualityModel> AllSexualities { get; set; }
		#endregion
	}
}