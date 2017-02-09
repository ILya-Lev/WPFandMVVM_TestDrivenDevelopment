using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
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

			_eventAggregator.GetEvent<FriendSavedEvent>().Subscribe(OnFriendSaved);
			_eventAggregator.GetEvent<FriendDeletedEvent>().Subscribe(OnFriendDeleted);

			Friends = new ObservableCollection<NavigationItemViewModel>();
		}

		private void OnFriendDeleted(int friendId)
		{
			var navigationItem = Friends.Single(item => item.Id == friendId);
			Friends.Remove(navigationItem);
		}

		private void OnFriendSaved(Friend friend)
		{
			var theItem = Friends.SingleOrDefault(item => item.Id == friend.Id);
			if (theItem == null)
			{
				var lookupItem = _friendsDataService.GetAllFriends()
								.Single(item => item.Id == friend.Id);
				theItem = new NavigationItemViewModel(lookupItem.Id, lookupItem.DisplayMember,
														_eventAggregator);
				Friends.Add(theItem);
			}
			theItem.DisplayMember = $"{friend.FirstName} {friend.LastName}";
		}
		//private void OnFriendSaved(Friend friend)
		//{
		//	var theFriendIdx = Friends.Select((item, idx) => new { item, idx })
		//								.Where(indexedItem => indexedItem.item.Id == friend.Id)
		//								.Select(indexedItem => indexedItem.idx).Single();
		//	var lookupItem = _friendsDataService.GetAllFriends().Single(item => item.Id == friend.Id);
		//	Friends.RemoveAt(theFriendIdx);
		//	Friends.Insert(theFriendIdx, new NavigationItemViewModel(lookupItem.Id,
		//										lookupItem.DisplayMember, _eventAggregator));
		//}

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
