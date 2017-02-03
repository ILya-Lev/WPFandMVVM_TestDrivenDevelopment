using FriendStorage.UI.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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

		public MainViewModel(INavigationViewModel navigationViewModel,
							Func<IFriendEditViewModel> friendEditVmCreator,
							IEventAggregator eventAggregator)
		{
			_friendEditVmCreator = friendEditVmCreator;

			NavigationViewModel = navigationViewModel;
			FriendEditViewModels = new ObservableCollection<IFriendEditViewModel>();

			eventAggregator.GetEvent<OpenFriendEditViewEvent>().Subscribe(OnOpenFriendEditView);
		}

		private void OnOpenFriendEditView(int friendId)
		{
			var friendEditVm = FriendEditViewModels.SingleOrDefault(vm => vm.Friend.Id == friendId);

			if (friendEditVm == null)
			{
				friendEditVm = _friendEditVmCreator();
				friendEditVm.Load(friendId);
				FriendEditViewModels.Add(friendEditVm);
			}

			SelectedFriendEditViewModel = friendEditVm;
		}

		public void Load()
		{
			NavigationViewModel.Load();
		}
	}
}
