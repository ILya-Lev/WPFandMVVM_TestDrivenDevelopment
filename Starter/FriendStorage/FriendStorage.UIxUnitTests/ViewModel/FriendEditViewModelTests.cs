using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.ViewModel;
using FriendStorage.UIxUnitTests.Extensions;
using Moq;
using Xunit;

namespace FriendStorage.UIxUnitTests.ViewModel
{
	public class FriendEditViewModelTests
	{
		private Mock<IFriendDataProvider> _friendDataProvider;
		private FriendEditViewModel _viewModel;
		private const int _friendId = 7;

		public FriendEditViewModelTests()
		{
			_friendDataProvider = new Mock<IFriendDataProvider>();
			_friendDataProvider.Setup(dp => dp.GetFriendById(_friendId))
								.Returns(new Friend { Id = _friendId, FirstName = "Julia" });

			_viewModel = new FriendEditViewModel(_friendDataProvider.Object);
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
			var fiered = _viewModel.IsPropertyChangedFiered(() => _viewModel.Load(_friendId),
															nameof(_viewModel.Friend));
			fiered.Should().BeTrue("view model's property should have been changed on load");
		}
	}
}