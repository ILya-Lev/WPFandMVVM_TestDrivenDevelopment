namespace FriendStorage.UI.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		public INavigationViewModel NavigationViewModel { get; }

		public MainViewModel(INavigationViewModel navigationViewModel)
		{
			NavigationViewModel = navigationViewModel;
		}

		public void Load()
		{
			NavigationViewModel.Load();
		}
	}
}
