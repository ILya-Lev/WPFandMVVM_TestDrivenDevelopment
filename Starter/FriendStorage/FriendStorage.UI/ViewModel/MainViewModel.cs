using FriendStorage.UI.Command;
using FriendStorage.UI.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace FriendStorage.UI.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		private readonly Func<IFriendEditViewModel> _friendEditVmCreator;
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
							IEventAggregator eventAggregator)
		{
			_friendEditVmCreator = friendEditVmCreator;

			NavigationViewModel = navigationViewModel;
			FriendEditViewModels = new ObservableCollection<IFriendEditViewModel>();

			eventAggregator.GetEvent<OpenFriendEditViewEvent>().Subscribe(OnOpenFriendEditView);
			CloseFriendTabCommand = new DelegateCommand(OnCloseFriendTabExecute);
			AddFriendCommand = new DelegateCommand(OnAddFriendExecute);
		}

		public void Load()
		{
			NavigationViewModel.Load();
		}

		private void OnCloseFriendTabExecute(object obj)
		{
			var friendEditVm = (IFriendEditViewModel) obj;

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
	}
}
