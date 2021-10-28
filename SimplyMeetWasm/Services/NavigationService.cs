using System;
using Microsoft.AspNetCore.Components;

namespace SimplyMeetWasm.Services
{
	public class NavigationService
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public String Uri => _Nav.Uri;
		#endregion
		#region Fields
		private readonly NavigationManager _Nav;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public NavigationService(NavigationManager InNav)
		{
			_Nav = InNav;
		}

		public Uri ToAbsoluteUri(String InRelativeUri)
		{
			return _Nav.ToAbsoluteUri(InRelativeUri);
		}
		public String ToBaseRelativePath(String InUri)
		{
			return _Nav.ToBaseRelativePath(InUri);
		}
		public void NavigateTo(String InUri)
		{
			_Nav.NavigateTo(InUri);
		}
	}
}