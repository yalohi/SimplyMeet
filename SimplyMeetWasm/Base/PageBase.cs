using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SimplyMeetWasm.Constants;
using SimplyMeetWasm.Services;

namespace SimplyMeetWasm.Base
{
	public class PageBase : ComponentBase
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		[Inject]
		public IJSRuntime JS { get; set; }
		[Inject]
		public AppState AppState { get; set; }
		[Inject]
		public MainHubService MainHubService { get; set; }
		[Inject]
		public NotificationService NotificationService { get; set; }
		#endregion

		//===========================================================================================
		// Protected Methods
		//===========================================================================================
		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			AppState.ShowNavBar = true;
			AppState.ShowFooter = false;

			await MainHubService.SetupLazyAsync();
			NotificationService.ClearMainNotification();

			await JS.InvokeVoidAsync(JSHelperConstants.SCROLL_ELEMENT_TO, IdConstants.SCROLL_CONTAINER_ID, 0);
		}
	}
}