using Autofac;
using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;

namespace FriendStorage.UI.Startup
{
	public static class Bootstrapper
	{
		public static IContainer Container { get; }

		static Bootstrapper()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<FileDataService>().As<IDataService>();
			builder.RegisterType<NavigationDataProvider>().As<INavigationDataProvider>();
			builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
			builder.RegisterType<MainViewModel>().AsSelf();
			builder.RegisterType<MainWindow>().AsSelf();
			//builder.RegisterType<OpenFriendEditViewEvent>().As<IEventAggregator>();
			Container = builder.Build();
		}
	}
}
