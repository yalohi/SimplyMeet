using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SimplyMeetWasm.Constants;
using SimplyMeetWasm.Services;

namespace SimplyMeetWasm.Base
{
	public class AppState : INotifyPropertyChanged
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public Boolean ShowNavBar
		{
			get => _ShowNavBar;
			set { if (_ShowNavBar != value) { _ShowNavBar = value; OnPropertyChanged(); } }
		}
		public Boolean ShowFooter
		{
			get => _ShowFooter;
			set { if (_ShowFooter != value) { _ShowFooter = value; OnPropertyChanged(); } }
		}

		public Boolean IsFirstLogin { get; set; }
		#endregion
		#region Fields
		private readonly LocalStorageService _LocalStorageService;

		private Boolean _ShowNavBar;
		private Boolean _ShowFooter;
		#endregion
		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public AppState(LocalStorageService InLocalStorageService)
		{
			_LocalStorageService = InLocalStorageService;
		}

		public async Task<Boolean> HasLoginAsync() => !String.IsNullOrEmpty(await GetLoginTokenAsync());
		public async Task<String> GetLoginTokenAsync() => (await _LocalStorageService.GetItemAsync<String>(LocalStorageConstants.LOGIN_TOKEN_STORAGE_KEY));
		public async Task<String> GetPrivateKeyAsync() => await _LocalStorageService.GetItemAsync<String>(LocalStorageConstants.PRIVATE_KEY_STORAGE_KEY);

		//===========================================================================================
		// Protected Methods
		//===========================================================================================
		protected void OnPropertyChanged([CallerMemberName] String InPropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(InPropertyName));
	}
}