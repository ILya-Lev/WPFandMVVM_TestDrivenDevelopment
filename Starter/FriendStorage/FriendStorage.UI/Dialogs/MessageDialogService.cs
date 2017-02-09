using System.Windows;

namespace FriendStorage.UI.Dialogs
{
	public class MessageDialogService : IMessageDialogService
	{
		public bool Show(string message, string title)
		{
			//return MessageBoxResult.Yes == MessageBox.Show(message, title, MessageBoxButton.YesNo);
			return new YesNoDialog(message, title)
			{
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
				Owner = Application.Current.MainWindow
			}.ShowDialog().GetValueOrDefault();
		}
	}
}