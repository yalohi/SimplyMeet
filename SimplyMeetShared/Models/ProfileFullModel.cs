using System;
using System.Collections.Generic;
using SimplyMeetShared.Enums;
using SimplyMeetShared.Models;

namespace SimplyMeetShared.ResponseModels
{
	public class ProfileFullModel : ResponseModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public EAccountFlags AccountFlags { get; set; }
		public ProfileDataModel Data { get; set; }

		public AccountModel Account { get; set; }
		public ProfileModel Profile { get; set; }
		public FilterModel Filter { get; set; }

		public IEnumerable<TagModel> Tags { get; set; }
		public IEnumerable<SexualityModel> Sexualities { get; set; }

		public IEnumerable<ReportReasonModel> ReportReasons { get; set; }
		public Boolean Reported { get; set; }
		#endregion
	}
}