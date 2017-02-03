using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Events;
using FriendStorage.UIxUnitTests.Extensions;
using Moq;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FriendStorage.UI.ViewModel.Tests
{
	public class MainViewModelTests
	{
		private readonly Mock<IEventAggregator> _eventAggregator;
		private readonly List<Mock<IFriendEditViewModel>> _friendEditVmMocks;
		private readonly OpenFriendEditViewEvent _openFriendEvent;
		private MainViewModel _mainViewModel;
		private Mock<INavigationViewModel> _navigationVm;

		public MainViewModelTests()
		{
			_openFriendEvent = new OpenFriendEditViewEvent();
			_eventAggregator = new Mock<IEventAggregator>();
			_eventAggregator.Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
							.Returns(_openFriendEvent);

			_friendEditVmMocks = new List<Mock<IFriendEditViewModel>>();

			_navigationVm = new Mock<INavigationViewModel>(MockBehavior.Strict);
			_navigationVm.Setup(vm => vm.Load());
			_mainViewModel = GenerateMainViewModel(_navigationVm.Object);
		}

		private MainViewModel GenerateMainViewModel(INavigationViewModel navigationVm)
		{
			return new MainViewModel(navigationVm,
				() =>
				{
					var friendEditVm = new Mock<IFriendEditViewModel>();
					friendEditVm.Setup(vm => vm.Load(It.IsAny<int>()))
					.Callback<int>(id => friendEditVm.Setup(vm => vm.Friend)
													 .Returns(new Friend { Id = id }));
					_friendEditVmMocks.Add(friendEditVm);
					return friendEditVm.Object;
				},
				_eventAggregator.Object);
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

			var fiered = _mainViewModel.IsPropertyChangedFiered(action,
										nameof(_mainViewModel.SelectedFriendEditViewModel));

			fiered.Should().BeTrue("we've already selected a friend");
		}
	}
}