namespace SimplyMeetWasm.Components;

public partial class TableListComponent : ComponentBase
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	[CascadingParameter]
	public ContainerComponent<TableListComponent> Container { get; set; }
	[Parameter]
	public String Title { get; set; }
	[Parameter]
	public String HeaderColorClass { get; set; }
	[Parameter]
	public Boolean ShowRemoveRowButton { get; set; }
	[Parameter]
	public IReadOnlyList<String> HeaderList { get; set; }
	[Parameter]
	public IReadOnlyList<IReadOnlyList<String>> RowList { get; set; }
	[Parameter]
	public EventCallback<TableListComponent> SelectedIndexChanged { get; set; }
	[Parameter]
	public EventCallback<(TableListComponent, Int32)> RemoveRowClick { get; set; }

	public Int32 SelectedIndex { get; set; }
	public Int32 ColumnCount => (HeaderList?.Count ?? 1) + (ShowRemoveRowButton ? 1 : 0);
	#endregion

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Container?.ChildComponentList.Add(this);
		SelectedIndex = -1;
	}

	//===========================================================================================
	// Private Methods
	//===========================================================================================
	private async void OnRowClickAsync(Int32 InIndex)
	{
		if (SelectedIndex == InIndex) return;

		SelectedIndex = InIndex;
		await SelectedIndexChanged.InvokeAsync(this);
	}
	private async void OnRemoveRowClickAsync(Int32 InIndex)
	{
		await RemoveRowClick.InvokeAsync((this, InIndex));
		if (SelectedIndex == -1 || SelectedIndex < InIndex) return;

		SelectedIndex = SelectedIndex == InIndex ? -1 : (SelectedIndex - 1);
		await SelectedIndexChanged.InvokeAsync(this);
	}
}
