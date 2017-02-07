using FriendStorage.UI.Command;
using FriendStorage.UI.Events;
using Prism.Events;
using System.Windows.Input;

namespace FriendStorage.UI.ViewModel
{
	public class NavigationItemViewModel : ViewModelBase
	{
		private readonly IEventAggregator _eventAggregator;
		private string _displayMember;
		public int Id { get; }

		public string DisplayMember
		{
			get { return _displayMember; }
			set { _displayMember = value; OnPropertyChanged(); }
		}

		public ICommand OpenFriendEditViewCommand { get; set; }

		public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;
			Id = id;
			DisplayMember = displayMember;
			OpenFriendEditViewCommand = new DelegateCommand(OnFriendEditViewExecute);
		}

		private void OnFriendEditViewExecute(object obj)
		{
			_eventAggregator.GetEvent<OpenFriendEditViewEvent>().Publish(Id);
		}
	}
}