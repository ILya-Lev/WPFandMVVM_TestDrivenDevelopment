using FriendStorage.Model;
using FriendStorage.UI.DataProvider;

namespace FriendStorage.UI.ViewModel
{
	public interface IFriendEditViewModel
	{
		Friend Friend { get; }
		void Load(int friendId);
	}

	public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
	{
		private readonly IFriendDataProvider _friendDataProvider;
		private Friend _friend;

		public Friend Friend
		{
			get { return _friend; }
			private set { _friend = value; OnPropertyChanged(); }
		}

		public FriendEditViewModel(IFriendDataProvider friendDataProvider)
		{
			_friendDataProvider = friendDataProvider;
		}

		public void Load(int friendId)
		{
			Friend = _friendDataProvider.GetFriendById(friendId);
		}
	}
}
