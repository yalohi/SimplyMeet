namespace SimplyMeetWasm.Services;

public class LocalStorageService
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Fields
	private readonly IJSRuntime _JavaScriptRuntime;
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public LocalStorageService(IJSRuntime InJavaScriptRuntime)
	{
		_JavaScriptRuntime = InJavaScriptRuntime;
	}

	public async Task<T> GetItemAsync<T>(String InKey)
	{
		if (InKey == null) throw new ArgumentNullException(nameof(InKey));

		var Json = await _JavaScriptRuntime.InvokeAsync<String>("localStorage.getItem", InKey);
		return Json != null ? JsonSerializer.Deserialize<T>(Json) : default;
	}
	public async Task SetItemAsync<T>(String InKey, T InValue)
	{
		if (InKey == null) throw new ArgumentNullException(nameof(InKey));
		if (InValue == null) throw new ArgumentNullException(nameof(InValue));

		await _JavaScriptRuntime.InvokeVoidAsync("localStorage.setItem", InKey, JsonSerializer.Serialize(InValue));
	}
	public async Task RemoveItemAsync(String InKey)
	{
		if (InKey == null) throw new ArgumentNullException(nameof(InKey));

		await _JavaScriptRuntime.InvokeVoidAsync("localStorage.removeItem", InKey);
	}
}