using Autofac;
using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;
using Prism.Events;

namespace FriendStorage.UI.Startup
{
	public static class Bootstrapper
	{
		public static IContainer Container { get; }

		static Bootstrapper()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

			builder.RegisterType<FileDataService>().As<IDataService>();
			builder.RegisterType<NavigationDataProvider>().As<INavigationDataProvider>();
			builder.RegisterType<FriendDataProvider>().As<IFriendDataProvider>();

			builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
			builder.RegisterType<FriendEditViewModel>().As<IFriendEditViewModel>();
			builder.RegisterType<MainViewModel>().AsSelf();

			builder.RegisterType<MainWindow>().AsSelf();

			Container = builder.Build();
		}
	}
}
