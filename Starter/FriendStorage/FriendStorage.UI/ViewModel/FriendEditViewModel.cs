using FriendStorage.Model;
using FriendStorage.UI.Command;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Dialogs;
using FriendStorage.UI.Events;
using FriendStorage.UI.Wrappers;
using Prism.Events;
using System;
using System.Windows.Input;

namespace FriendStorage.UI.ViewModel
{
	public interface IFriendEditViewModel
	{
		FriendWrapper Friend { get; }
		void Load(int? friendId);
	}

	public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
	{
		private readonly IFriendDataProvider _friendDataProvider;
		private readonly IEventAggregator _eventAggregator;
		private readonly IMessageDialogService _messageDialogService;
		private FriendWrapper _friend;

		public FriendWrapper Friend
		{
			get { return _friend; }
			private set { _friend = value; OnPropertyChanged(); }
		}

		public ICommand SaveCommand { get; }
		public ICommand DeleteCommand { get; }

		public FriendEditViewModel(IFriendDataProvider friendDataProvider,
									IEventAggregator eventAggregator,
									IMessageDialogService messageDialogService)
		{
			_friendDataProvider = friendDataProvider;
			_eventAggregator = eventAggregator;
			_messageDialogService = messageDialogService;
			SaveCommand = new DelegateCommand(OnSaveExecute, OnCanSaveExecute);
			DeleteCommand = new DelegateCommand(OnDeleteExecute, OnCanDeleteExecute);
		}

		public void Load(int? friendId)
		{
			var friend = friendId.HasValue
				? _friendDataProvider.GetFriendById(friendId.Value)
				: new Friend();
			Friend = new FriendWrapper(friend);

			Action raiseCanExecuteChanged =
				() => (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();

			Friend.PropertyChanged += (sender, args) => raiseCanExecuteChanged();

			raiseCanExecuteChanged();
		}

		private bool OnCanSaveExecute(object arg)
		{
			return Friend?.IsChanged ?? false;
		}

		private void OnSaveExecute(object obj)
		{
			_friendDataProvider.SaveFriend(Friend.Model);
			Friend.AcceptChanges();
			_eventAggregator.GetEvent<FriendSavedEvent>().Publish(Friend.Model);

			(DeleteCommand as DelegateCommand).RaiseCanExecuteChanged();
		}

		private bool OnCanDeleteExecute(object arg)
		{
			return Friend != null && Friend.Id != 0;
		}

		private void OnDeleteExecute(object obj)
		{
			string message = "Do you really whant to delete the friend" +
							 $" '{Friend.FirstName} {Friend.LastName}'?";
			const string title = "Delete a friend";

			if (!_messageDialogService.Show(message, title)) return;

			_friendDataProvider.DeleteFriend(Friend.Id);
			_eventAggregator.GetEvent<FriendDeletedEvent>().Publish(Friend.Id);
		}
	}
}