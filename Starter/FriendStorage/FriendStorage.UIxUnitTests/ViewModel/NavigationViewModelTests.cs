using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using Moq;
using System.Linq;
using Xunit;

namespace FriendStorage.UI.ViewModel.Tests
{
	public class NavigationViewModelTests
	{
		private static readonly LookupItem[] _friends = new[]
		{
			new LookupItem { Id = 1, DisplayMember = "alan jons"},
			new LookupItem { Id = 2, DisplayMember = "krage morrison"}
		};

		[Fact]
		public void Load_ShouldReturnExpectedCollection()
		{
			var mockedService = new Mock<INavigationDataProvider>(MockBehavior.Strict);
			mockedService.Setup(ds => ds.GetAllFriends()).Returns(_friends);
			var viewModel = new NavigationViewModel(mockedService.Object);

			viewModel.Load();

			viewModel.Friends.Should().BeEquivalentTo(_friends);
		}
		[Fact]
		public void Load_CallTwice_ShouldKeepFriendIdUnique()
		{
			var mockedService = new Mock<INavigationDataProvider>(MockBehavior.Strict);
			mockedService.Setup(ds => ds.GetAllFriends()).Returns(_friends);
			var viewModel = new NavigationViewModel(mockedService.Object);

			viewModel.Load();
			viewModel.Load();

			viewModel.Friends.SingleOrDefault(f => f.Id == _friends.First().Id);
			viewModel.Friends.Should().BeEquivalentTo(_friends);
		}
	}
}