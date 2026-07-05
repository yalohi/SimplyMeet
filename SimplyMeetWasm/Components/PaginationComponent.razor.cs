namespace SimplyMeetWasm.Components;

public partial class PaginationComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[Parameter] public String PageLink { get; set; }
	[Parameter] public Int32 PageIndex { get; set; }
	[Parameter] public Int32 MaxPageButtonRange { get; set; }
	[Parameter] public Int32 ItemsPerPage { get; set; }
	[Parameter] public Int32 ItemCount { get; set; }
	[Parameter] public Boolean ShowStartEndButtons { get; set; }
	[Parameter] public Boolean ShowPreviousNextButtons { get; set; }
	[Parameter] public Boolean ShowMorePagesIndicators { get; set; }

	public Int32 PageCount => (ItemCount - 1) / ItemsPerPage + 1;
	public Int32 PageButtonCount => Math.Min(MaxPageButtonRange * 2 + 1, PageCount);
	public Int32 PageButtonStart => Math.Clamp(PageIndex - MaxPageButtonRange, 0, Math.Max(PageCount - PageButtonCount, 0));
	public Boolean IsPreviousButtonDisabled => PageIndex <= 0;
	public Boolean IsNextButtonDisabled => PageIndex >= PageCount - 1;
	#endregion
}
