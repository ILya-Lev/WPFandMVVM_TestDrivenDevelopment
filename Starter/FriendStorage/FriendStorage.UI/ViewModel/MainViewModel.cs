using FriendStorage.UI.Command;
using FriendStorage.UI.Dialogs;
using FriendStorage.UI.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace FriendStorage.UI.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		private readonly Func<IFriendEditViewModel> _friendEditVmCreator;
		private readonly IMessageDialogService _messageDialogService;
		private IFriendEditViewModel _selectedFriendEditViewModel;

		public INavigationViewModel NavigationViewModel { get; }
		public ObservableCollection<IFriendEditViewModel> FriendEditViewModels { get; }

		public IFriendEditViewModel SelectedFriendEditViewModel
		{
			get { return _selectedFriendEditViewModel; }
			set { _selectedFriendEditViewModel = value; OnPropertyChanged(); }
		}
		public ICommand CloseFriendTabCommand { get; }
		public ICommand AddFriendCommand { get; }

		public MainViewModel(INavigationViewModel navigationViewModel,
							Func<IFriendEditViewModel> friendEditVmCreator,
							IEventAggregator eventAggregator,
							IMessageDialogService messageDialogService)
		{
			_friendEditVmCreator = friendEditVmCreator;
			_messageDialogService = messageDialogService;

			NavigationViewModel = navigationViewModel;
			FriendEditViewModels = new ObservableCollection<IFriendEditViewModel>();

			eventAggregator.GetEvent<OpenFriendEditViewEvent>().Subscribe(OnOpenFriendEditView);
			eventAggregator.GetEvent<FriendDeletedEvent>().Subscribe(OnFriendDeletedEvent);
			CloseFriendTabCommand = new DelegateCommand(OnCloseFriendTabExecute);
			AddFriendCommand = new DelegateCommand(OnAddFriendExecute);
		}

		public void Load()
		{
			NavigationViewModel.Load();
		}

		public void OnClosing(CancelEventArgs cancelEventArgs)
		{
			var unsavedFriend = FriendEditViewModels.FirstOrDefault(vm => vm.Friend.IsChanged)
								?.Friend;
			if (unsavedFriend != null)
			{
				var message = "Do you want to close without saving at least" +
							  $" '{unsavedFriend.FirstName} {unsavedFriend.LastName}'?\n" +
							  "All unsaved changes will be lost!";
				var title = "Closing the application warning";
				cancelEventArgs.Cancel = !_messageDialogService.Show(message, title);
			}
		}

		private void OnCloseFriendTabExecute(object obj)
		{
			var friendEditVm = (IFriendEditViewModel) obj;
			if (friendEditVm.Friend.IsChanged)
			{
				string message = "Do you want to close the tab for " +
						$"'{friendEditVm.Friend.FirstName} {friendEditVm.Friend.LastName}'?\n" +
						"All unsaved changes will be lost";
				const string title = "Close the tab!";

				if (!_messageDialogService.Show(message, title))
					return;
			}
			// SelectedFriendEditViewModel is changed by the View control
			FriendEditViewModels.Remove(friendEditVm);
		}

		private void OnOpenFriendEditView(int friendId)
		{
			SelectedFriendEditViewModel = FriendEditViewModels
											.SingleOrDefault(vm => vm.Friend.Id == friendId)
										?? LoadFriend(friendId);
		}

		private void OnAddFriendExecute(object obj)
		{
			SelectedFriendEditViewModel = LoadFriend(null);
		}

		private IFriendEditViewModel LoadFriend(int? friendId)
		{
			var friendEditVm = _friendEditVmCreator();
			friendEditVm.Load(friendId);
			FriendEditViewModels.Add(friendEditVm);
			return friendEditVm;
		}

		private void OnFriendDeletedEvent(int friendId)
		{
			var vm = FriendEditViewModels.Single(item => item.Friend.Id == friendId);
			FriendEditViewModels.Remove(vm);
		}
	}
}
