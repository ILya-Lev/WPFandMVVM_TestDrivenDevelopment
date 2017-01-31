using FriendStorage.UI.DataProvider;

namespace FriendStorage.UI.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		public NavigationViewModel NavigationViewModel { get; }

		public MainViewModel(INavigationDataProvider friendsDataService)
		{
			NavigationViewModel = new NavigationViewModel(friendsDataService);
		}

		public void Load()
		{
			NavigationViewModel.Load();
		}
	}
}
