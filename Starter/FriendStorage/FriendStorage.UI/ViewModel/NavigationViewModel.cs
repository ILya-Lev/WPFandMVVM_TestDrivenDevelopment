using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using System.Collections.ObjectModel;
using System.Linq;

namespace FriendStorage.UI.ViewModel
{
	public interface INavigationViewModel
	{
		void Load();
	}

	public class NavigationViewModel : ViewModelBase, INavigationViewModel
	{
		private readonly INavigationDataProvider _friendsDataService;

		/// <summary>
		/// if one assign new collection to the property after it's binded - no data will be displayed
		/// </summary>
		public ObservableCollection<LookupItem> Friends { get; }

		public NavigationViewModel(INavigationDataProvider friendsDataService)
		{
			_friendsDataService = friendsDataService;
			Friends = new ObservableCollection<LookupItem>();
		}

		public void Load()
		{
			Friends.Clear();
			foreach (var friend in _friendsDataService.GetAllFriends().ToList())
			{
				Friends.Add(friend);
			}
		}
	}
}
