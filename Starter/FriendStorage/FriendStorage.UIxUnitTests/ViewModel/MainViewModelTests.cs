using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Dialogs;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using FriendStorage.UI.Wrappers;
using FriendStorage.UIxUnitTests.Extensions;
using Moq;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FriendStorage.UIxUnitTests.ViewModel
{
	public class MainViewModelTests
	{
		private readonly Mock<IEventAggregator> _eventAggregator;
		private readonly List<Mock<IFriendEditViewModel>> _friendEditVmMocks;
		private readonly OpenFriendEditViewEvent _openFriendEvent;
		private readonly FriendDeletedEvent _friendDeletedEvent;
		private MainViewModel _mainViewModel;
		private Mock<INavigationViewModel> _navigationVm;
		private Mock<IMessageDialogService> _messageDialogService;

		public MainViewModelTests()
		{
			_openFriendEvent = new OpenFriendEditViewEvent();
			_friendDeletedEvent = new FriendDeletedEvent();
			_eventAggregator = new Mock<IEventAggregator>();
			_eventAggregator.Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
							.Returns(_openFriendEvent);
			_eventAggregator.Setup(ea => ea.GetEvent<FriendDeletedEvent>())
							.Returns(_friendDeletedEvent);

			_friendEditVmMocks = new List<Mock<IFriendEditViewModel>>();

			_navigationVm = new Mock<INavigationViewModel>(MockBehavior.Strict);
			_navigationVm.Setup(vm => vm.Load());
			_messageDialogService = new Mock<IMessageDialogService>();

			_mainViewModel = GenerateMainViewModel(_navigationVm.Object);
		}

		private MainViewModel GenerateMainViewModel(INavigationViewModel navigationVm)
		{
			return new MainViewModel(navigationVm,
				() =>
				{
					var friendEditVm = new Mock<IFriendEditViewModel>();
					friendEditVm.Setup(vm => vm.Load(It.IsAny<int>()))
					.Callback<int?>(id => friendEditVm.Setup(vm => vm.Friend)
										.Returns(new FriendWrapper(
													new Friend
													{
														Id = id.Value,
														Address = new Address()
													})
												)
									);
					_friendEditVmMocks.Add(friendEditVm);
					return friendEditVm.Object;
				},
				_eventAggregator.Object, _messageDialogService.Object);
		}

		[Fact]
		public void Load_ShouldDelegateCallToNavigationVM()
		{
			_mainViewModel.Load();

			_navigationVm.Verify(vm => vm.Load(), Times.Once);
		}
		[Fact]
		public void SelectFriend_ShouldOpenFriendEditTab()
		{
			const int friendId = 7;
			_openFriendEvent.Publish(friendId);

			_mainViewModel.FriendEditViewModels.Count.Should()
				.Be(1, "published only one open friend edit event");

			var friendEditVm = _mainViewModel.FriendEditViewModels.First();
			friendEditVm.Should().Be(_mainViewModel.SelectedFriendEditViewModel,
				"selected the only one friend edit view model");

			_friendEditVmMocks.First().Verify(vm => vm.Load(friendId), Times.Once);
		}
		[Fact]
		public void SelectFriendTwice_ShouldLoadOnce()
		{
			const int friendId = 7;
			_openFriendEvent.Publish(friendId);
			_openFriendEvent.Publish(friendId);

			_mainViewModel.FriendEditViewModels.Count.Should()
				.Be(1, "published only one open friend edit event");

			var friendEditVm = _mainViewModel.FriendEditViewModels.First();
			friendEditVm.Should().Be(_mainViewModel.SelectedFriendEditViewModel,
				"selected the only one friend edit view model");

			_friendEditVmMocks.First().Verify(vm => vm.Load(friendId), Times.Once);
		}

		[Fact]
		public void SelectFriend_ShouldRaisePropertyChangedEvent()
		{
			Action action = () =>
			{
				var friendEditVm = new Mock<IFriendEditViewModel>();
				_mainViewModel.SelectedFriendEditViewModel = friendEditVm.Object;
			};

			var fiered = _mainViewModel.IsPropertyChangedFired(action,
										nameof(_mainViewModel.SelectedFriendEditViewModel));

			fiered.Should().BeTrue("we've already selected a friend");
		}

		[Fact]
		public void CloseFriendTabCommand_ShouldRemoveFriendEditViewModel()
		{
			_openFriendEvent.Publish(7);
			_messageDialogService.Setup(d => d.Show(It.IsAny<string>(), It.IsAny<string>()))
								.Returns(false);
			var friendEditVm = _friendEditVmMocks.Single(vm => vm.Object.Friend.Id == 7);

			_mainViewModel.CloseFriendTabCommand.Execute(friendEditVm.Object);

			_mainViewModel.FriendEditViewModels.Should()
				.NotContain(vm => vm.Friend.Id == 7, "we executed close friend tab command already");
		}
		[Fact]
		public void AddFriend_ShouldOpenCreatedFriend()
		{
			_mainViewModel.AddFriendCommand.Execute(null);

			_mainViewModel.FriendEditViewModels.Count.Should().Be(1, "created only one friend");

			var friendEditVm = _mainViewModel.FriendEditViewModels.First();
			friendEditVm.Should().Be(_mainViewModel.SelectedFriendEditViewModel,
										"created friend should be selected");
			_friendEditVmMocks.First().Verify(vm => vm.Load(null), Times.Once);
		}

		[Fact]
		public void Delete_ExistingFriend_ShouldCloseFriendEditTab()
		{
			_openFriendEvent.Publish(7);

			_mainViewModel.FriendEditViewModels.Count.Should().Be(1);
			_mainViewModel.FriendEditViewModels.First().Friend.Id.Should().Be(7);

			_friendDeletedEvent.Publish(7);

			_mainViewModel.FriendEditViewModels.Count.Should().Be(0);
		}
	}
}