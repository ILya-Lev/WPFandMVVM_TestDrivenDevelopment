using Autofac;
using FriendStorage.UI.Startup;
using FriendStorage.UI.View;
using System.Windows;

namespace FriendStorage.UI
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			MainWindow = Bootstrapper.Container.Resolve<MainWindow>();
			MainWindow.Show();
		}
	}
}
