using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimplyMeetWasm.Enums;

namespace SimplyMeetWasm.Services
{
	public class NotificationService : INotifyPropertyChanged
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Properties
		public String Text { get; private set; }
		public ENotificationType Type { get; private set; }
		#endregion
		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public void ClearMainNotification() => SetMainNotification(String.Empty, ENotificationType.None);
		public void SetMainNotification(String InText, ENotificationType InType)
		{
			if (InText == null) throw new ArgumentNullException(nameof(InText));

			Text = InText;
			Type = InType;
			OnPropertyChanged();
		}

		//===========================================================================================
		// Protected Methods
		//===========================================================================================
		protected void OnPropertyChanged([CallerMemberName] String InPropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(InPropertyName));
	}
}