using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using Moq;
using Prism.Events;
using System.Linq;
using Xunit;

namespace FriendStorage.UI.ViewModel.Tests
{
	public class NavigationViewModelTests
	{
		private NavigationViewModel _navigationViewModel;
		private static readonly LookupItem[] _friends = new[]
		{
			new LookupItem { Id = 1, DisplayMember = "alan jons"},
			new LookupItem { Id = 2, DisplayMember = "krage morrison"}
		};

		// in xUnit is an analogue of [TestInitialize] decorated MSTest method
		public NavigationViewModelTests()
		{
			var mockedService = new Mock<INavigationDataProvider>(MockBehavior.Strict);
			mockedService.Setup(ds => ds.GetAllFriends()).Returns(_friends);
			var mockedEventAggregator = new Mock<IEventAggregator>();
			_navigationViewModel = new NavigationViewModel(mockedService.Object, mockedEventAggregator.Object);
		}

		[Fact]
		public void Load_ShouldReturnExpectedCollection()
		{
			_navigationViewModel.Load();

			_navigationViewModel.Friends.Select(item => item.Id)
				.Should().BeEquivalentTo(_friends.Select(f => f.Id));
		}
		[Fact]
		public void Load_CallTwice_ShouldKeepFriendIdUnique()
		{
			_navigationViewModel.Load();
			_navigationViewModel.Load();

			_navigationViewModel.Friends.SingleOrDefault(f => f.Id == _friends.First().Id);
			_navigationViewModel.Friends.Select(item => item.Id)
				.Should().BeEquivalentTo(_friends.Select(f => f.Id));
		}
	}
}