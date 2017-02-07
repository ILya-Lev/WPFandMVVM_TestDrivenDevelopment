﻿using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using FriendStorage.UIxUnitTests.Extensions;
using Moq;
using Prism.Events;
using Xunit;

namespace FriendStorage.UIxUnitTests.ViewModel
{
	public class FriendEditViewModelTests
	{
		private Mock<IFriendDataProvider> _friendDataProvider;
		private FriendEditViewModel _viewModel;
		private Mock<FriendSavedEvent> _friendSavedEvent;
		private Mock<IEventAggregator> _eventAggregator;
		private const int _friendId = 7;

		public FriendEditViewModelTests()
		{
			_friendSavedEvent = new Mock<FriendSavedEvent>();
			_eventAggregator = new Mock<IEventAggregator>();
			_eventAggregator.Setup(ea => ea.GetEvent<FriendSavedEvent>())
							.Returns(_friendSavedEvent.Object);

			_friendDataProvider = new Mock<IFriendDataProvider>();
			_friendDataProvider.Setup(dp => dp.GetFriendById(_friendId))
								.Returns(new Friend { Id = _friendId, FirstName = "Julia" });

			_viewModel = new FriendEditViewModel(_friendDataProvider.Object, _eventAggregator.Object);
		}

		[Fact]
		public void Load_WithDefaultId_ShouldProvideExpectedFriend()
		{
			_viewModel.Load(_friendId);

			_viewModel.Friend.Id.Should().Be(_friendId);
			_friendDataProvider.Verify(dp => dp.GetFriendById(It.IsAny<int>()), Times.Once);
		}

		[Fact]
		public void Load_WithDefaultId_ShouldRaisePropertyChangedEvent()
		{
			var fired = _viewModel.IsPropertyChangedFired(() => _viewModel.Load(_friendId),
															nameof(_viewModel.Friend));
			fired.Should().BeTrue("view model's property should have been changed on load");
		}

		[Fact]
		public void SaveCommand_FriendIsJustLoaded_ShouldBeDisabled()
		{
			_viewModel.Load(_friendId);

			_viewModel.SaveCommand.CanExecute(null).Should().BeFalse("there is no change yet");
		}

		[Fact]
		public void SaveCommand_BeforeAnyFriendIsLoaded_ShouldBeDisabled()
		{
			_viewModel.SaveCommand.CanExecute(null).Should().BeFalse("no friends has been loaded yet");
		}

		[Fact]
		public void SaveCommand_ChangeFriend_ShouldBeEnabled()
		{
			_viewModel.Load(_friendId);
			_viewModel.Friend.FirstName = "Jessica";
			_viewModel.SaveCommand.CanExecute(null).Should().BeTrue("the record has been edited");
		}

		[Fact]
		public void CanExecuteChange_OnFriendChange_ShouldBeRaised()
		{
			_viewModel.Load(_friendId);
			var fired = false;
			_viewModel.SaveCommand.CanExecuteChanged += (sender, args) => fired = true;

			_viewModel.Friend.FirstName = "Changed name";

			fired.Should().BeTrue("the record has been edited");
		}

		[Fact]
		public void CanExecuteChange_OnLoad_ShouldBeRaised()
		{
			var fired = false;
			_viewModel.SaveCommand.CanExecuteChanged += (sender, args) => fired = true;

			_viewModel.Load(_friendId);

			fired.Should().BeTrue("the record has been edited");
		}

		[Fact]
		public void Save_AFriend_ShouldCallProvidersSaveMethod()
		{
			_viewModel.Load(_friendId);
			_viewModel.Friend.FirstName = "changed";

			_viewModel.SaveCommand.Execute(null);

			_friendDataProvider.Verify(p => p.SaveFriend(It.IsAny<Friend>()), Times.Once);
		}

		[Fact]
		public void Save_AFriend_ShouldAcceptChanges()
		{
			_viewModel.Load(_friendId);
			_viewModel.Friend.FirstName = "changed";

			_viewModel.SaveCommand.Execute(null);

			_viewModel.Friend.IsChanged.Should().BeFalse("changes has been already saved");
		}

		[Fact]
		public void Save_AFriend_ShouldPublishFriendSavedEvent()
		{
			_viewModel.Load(_friendId);
			_viewModel.Friend.FirstName = "changed";

			_viewModel.SaveCommand.Execute(null);

			_friendSavedEvent.Verify(e => e.Publish(It.IsAny<Friend>()), Times.Once);
			_friendSavedEvent.Verify(e => e.Publish(_viewModel.Friend.Model), Times.Once);
			_friendSavedEvent.Verify(e => e.Publish(null), Times.Never);
		}

		[Fact]
		public void Load_NullId_ShouldFireFriendChanged()
		{
			var fired = _viewModel.IsPropertyChangedFired(
				() => _viewModel.Load(null),
				nameof(_viewModel.Friend)
			);
			fired.Should().BeTrue("view model's property should have been changed on load");
		}
		[Fact]
		public void Load_NullId_ShouldCreateFriend()
		{
			_viewModel.Load(null);

			_viewModel.Friend.Should().NotBeNull("new friend is created");
			_viewModel.Friend.Id.Should().Be(0, "friend has not been saved yet");
			_viewModel.Friend.FirstName.Should().BeNullOrEmpty("no value is already assinged");
			_viewModel.Friend.LastName.Should().BeNullOrEmpty("no value is already assinged");
			_viewModel.Friend.Birthday.Should().BeNull("no DOB is assigned yet");
			_viewModel.Friend.IsDeveloper.Should().BeFalse("friend is not a developer by default");

			_friendDataProvider.Verify(p => p.GetFriendById(It.IsAny<int>()), Times.Never,
				"friend edit view model should not call get friend by id of the provider when " +
				"its load method is invoked with null value");
		}
	}
}