namespace SimplyMeetWasm.Components;

public partial class ImageModalComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Parameter] public String Title { get; set; }
	[Parameter] public String CssClass { get; set; }
	[Parameter] public String ImageSrc { get; set; }
	[Parameter] public String Alt { get; set; }
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
}
