﻿using FriendStorage.Model;
using FriendStorage.UI.Command;
using FriendStorage.UI.DataProvider;
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
		private FriendWrapper _friend;

		public FriendWrapper Friend
		{
			get { return _friend; }
			private set { _friend = value; OnPropertyChanged(); }
		}

		public ICommand SaveCommand { get; }

		public FriendEditViewModel(IFriendDataProvider friendDataProvider,
									IEventAggregator eventAggregator)
		{
			_friendDataProvider = friendDataProvider;
			_eventAggregator = eventAggregator;
			SaveCommand = new DelegateCommand(OnSaveExecute, OnCanSaveExecute);
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
		}
	}
}
