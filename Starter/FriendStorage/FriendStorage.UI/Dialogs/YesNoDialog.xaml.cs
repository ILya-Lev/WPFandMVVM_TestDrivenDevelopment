using System.Windows;

namespace FriendStorage.UI.Dialogs
{
	/// <summary>
	/// Interaction logic for YesNoDialog.xaml
	/// </summary>
	public partial class YesNoDialog : Window
	{
		public YesNoDialog(string message, string title)
		{
			InitializeComponent();
			Title = title;
			_textBlock.Text = message;
		}

		private void OnClickNoButton(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void OnClickYesButton(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
