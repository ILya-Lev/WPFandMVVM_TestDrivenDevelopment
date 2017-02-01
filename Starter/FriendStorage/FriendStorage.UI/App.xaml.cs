using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;
using System;
using System.Windows;

namespace FriendStorage.UI
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Func<IDataService> friendsDataServiceCreator = () => new FileDataService();
			var dataProvider = new NavigationDataProvider(friendsDataServiceCreator);
			var navigationViewModel = new NavigationViewModel(dataProvider);
			var viewModel = new MainViewModel(navigationViewModel);

			MainWindow = new MainWindow(viewModel);
			MainWindow.Show();
		}
	}
}
