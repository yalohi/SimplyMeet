using System;
using Microsoft.Extensions.Localization;
using SimplyMeetWasm.Resources;

namespace SimplyMeetWasm.Services
{
	public class LocalizationService
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public LocalizedString this[String InIdentifier]
		{
			get { return _Loc[InIdentifier]; }
		}
		#endregion
		#region Fields
		private readonly IStringLocalizer<SharedResource> _Loc;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public LocalizationService(IStringLocalizer<SharedResource> InLoc)
		{
			_Loc = InLoc;
		}
	}
}