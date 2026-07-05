namespace SimplyMeetWasm.Components;

public partial class ConfirmModalComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Parameter] public String Title { get; set; }
	[Parameter] public String Content { get; set; }
	[Parameter] public String ConfirmText { get; set; }
	[Parameter] public String CloseText { get; set; }
	[Parameter] public String ConfirmClass { get; set; }
	[Parameter] public EventCallback OnConfirm { get; set; }
	#endregion
	#region Fields
	private String _ModalDisplay;
	private String _ModalClass;
	private Boolean _ShowBackdrop;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public void Open()
	{
		_ModalDisplay = "block";
		_ModalClass = "show";
		_ShowBackdrop = true;
		StateHasChanged();
	}
	public void Close()
	{
		_ModalDisplay = "none";
		_ModalClass = "";
		_ShowBackdrop = false;
		StateHasChanged();
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async Task Confirm()
	{
		Close();
		await OnConfirm.InvokeAsync();
	}
}
