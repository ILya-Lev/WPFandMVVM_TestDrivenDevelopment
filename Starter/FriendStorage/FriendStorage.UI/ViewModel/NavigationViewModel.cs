using FriendStorage.UI.DataProvider;
using Prism.Events;
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
		private readonly IEventAggregator _eventAggregator;

		/// <summary>
		/// if one assign new collection to the property after it's binded - no data will be displayed
		/// </summary>
		public ObservableCollection<NavigationItemViewModel> Friends { get; }

		public NavigationViewModel(INavigationDataProvider friendsDataService,
									IEventAggregator eventAggregator)
		{
			_friendsDataService = friendsDataService;
			_eventAggregator = eventAggregator;

			Friends = new ObservableCollection<NavigationItemViewModel>();
		}

		public void Load()
		{
			Friends.Clear();
			foreach (var friend in _friendsDataService.GetAllFriends().ToList())
			{
				Friends.Add(new NavigationItemViewModel(friend.Id, friend.DisplayMember, _eventAggregator));
			}
		}
	}
}
