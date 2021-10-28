using System;
using System.Collections.Generic;
using SimplyMeetShared.Models;

namespace SimplyMeetShared.ResponseModels
{
	public class HomeGetDataResponseModel : ResponseModelBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public IEnumerable<CardModel> Cards { get; set; }
		public Int32 TotalActiveAccounts { get; set; }
		public Int32 TotalActiveMatches { get; set; }
		#endregion
	}
}