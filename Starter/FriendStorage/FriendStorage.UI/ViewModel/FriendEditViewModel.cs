using FriendStorage.Model;
using FriendStorage.UI.Command;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Wrappers;
using System.Windows.Input;

namespace FriendStorage.UI.ViewModel
{
	public interface IFriendEditViewModel
	{
		FriendWrapper Friend { get; }
		void Load(int friendId);
	}

	public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
	{
		private readonly IFriendDataProvider _friendDataProvider;
		private FriendWrapper _friend;

		public FriendWrapper Friend
		{
			get { return _friend; }
			private set { _friend = value; OnPropertyChanged(); }
		}

		public ICommand SaveCommand { get; }

		public FriendEditViewModel(IFriendDataProvider friendDataProvider)
		{
			_friendDataProvider = friendDataProvider;
			SaveCommand = new DelegateCommand(OnSaveExecute, OnCanSaveExecute);
		}

		private bool OnCanSaveExecute(object arg)
		{
			return Friend?.IsChanged ?? false;
		}

		private void OnSaveExecute(object obj)
		{
			var friend = (Friend) obj;
			_friendDataProvider.SaveFriend(Friend.Model);
		}

		public void Load(int friendId)
		{
			var friend = _friendDataProvider.GetFriendById(friendId);
			Friend = new FriendWrapper(friend);
		}
	}
}
