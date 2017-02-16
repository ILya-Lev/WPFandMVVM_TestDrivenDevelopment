using FriendStorage.Model;
using FriendStorage.UI.Command;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Dialogs;
using FriendStorage.UI.Events;
using FriendStorage.UI.Wrappers;
using Prism.Events;
using System.Collections.ObjectModel;
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
		public ICommand ResetCommand { get; }
		public ICommand DeleteCommand { get; }
		public ICommand AddEmailCommand { get; }
		public ICommand RemoveEmailCommand { get; }
		public ObservableCollection<LookupItem> FriendGroupLookup { get; }

		public FriendEditViewModel(IFriendDataProvider friendDataProvider,
									IEventAggregator eventAggregator,
									IMessageDialogService messageDialogService)
		{
			_friendDataProvider = friendDataProvider;
			_eventAggregator = eventAggregator;
			_messageDialogService = messageDialogService;

			var friendGroups = new[]
			{
				new LookupItem {Id = 0, DisplayMember = "Family"},
				new LookupItem {Id = 1, DisplayMember = "School"},
				new LookupItem {Id = 2, DisplayMember = "University"},
				new LookupItem {Id = 3, DisplayMember = "Work"},
			};
			FriendGroupLookup = new ObservableCollection<LookupItem>(friendGroups);

			SaveCommand = new DelegateCommand(OnSaveExecute, OnCanSaveExecute);
			ResetCommand = new DelegateCommand(OnResetExecute, OnCanSaveExecute);
			DeleteCommand = new DelegateCommand(OnDeleteExecute, OnCanDeleteExecute);

			AddEmailCommand = new DelegateCommand(OnAddEmailExecute);
			RemoveEmailCommand = new DelegateCommand(OnRemoveEmailExecute, OnCanRemoveEmailExecute);
		}

		public void Load(int? friendId)
		{
			var friend = friendId.HasValue
				? _friendDataProvider.GetFriendById(friendId.Value)
				: new Friend { Address = new Address() };
			Friend = new FriendWrapper(friend);


			Friend.PropertyChanged += (sender, args) => RaiseCanExecuteChanged();

			RaiseCanExecuteChanged();
		}

		private void RaiseCanExecuteChanged()
		{
			(SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
			(ResetCommand as DelegateCommand).RaiseCanExecuteChanged();
			(DeleteCommand as DelegateCommand).RaiseCanExecuteChanged();
			(RemoveEmailCommand as DelegateCommand).RaiseCanExecuteChanged();
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

		private void OnResetExecute(object obj)
		{
			Friend.RejectChanges();
		}

		private bool OnCanDeleteExecute(object arg)
		{
			return Friend != null && Friend.Id != 0;
		}

		private void OnDeleteExecute(object obj)
		{
			string message = "Do you really want to delete the friend" +
							 $" '{Friend.FirstName} {Friend.LastName}'?";
			const string title = "Delete a friend";

			if (!_messageDialogService.Show(message, title)) return;

			_friendDataProvider.DeleteFriend(Friend.Id);
			_eventAggregator.GetEvent<FriendDeletedEvent>().Publish(Friend.Id);
		}

		private void OnAddEmailExecute(object obj)
		{
			Friend?.Emails?.Add(new FriendEmailWrapper(new FriendEmail()));
			(RemoveEmailCommand as DelegateCommand).RaiseCanExecuteChanged();
		}

		private bool OnCanRemoveEmailExecute(object arg)
		{
			return (Friend?.Emails?.Count ?? 0) > 0;
		}

		private void OnRemoveEmailExecute(object obj)
		{
			Friend.Emails.RemoveAt(Friend.Emails.Count - 1);
		}
	}
}