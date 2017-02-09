using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using Moq;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FriendStorage.UIxUnitTests.ViewModel
{
	public class NavigationViewModelTests
	{
		private static readonly List<LookupItem> _friends = new List<LookupItem>
		{
			new LookupItem { Id = 1, DisplayMember = "alan jons"},
			new LookupItem { Id = 2, DisplayMember = "krage morrison"}
		};

		private NavigationViewModel _navigationViewModel;
		private FriendSavedEvent _friendSavedEvent;
		private FriendDeletedEvent _friendDeletedEvent;
		private Mock<IEventAggregator> _eventAggregator;

		// in xUnit is an analogue of [TestInitialize] decorated MSTest method
		public NavigationViewModelTests()
		{
			_friendSavedEvent = new FriendSavedEvent();
			_friendDeletedEvent = new FriendDeletedEvent();
			_eventAggregator = new Mock<IEventAggregator>();
			_eventAggregator.Setup(ea => ea.GetEvent<FriendSavedEvent>())
							.Returns(_friendSavedEvent);
			_eventAggregator.Setup(ea => ea.GetEvent<FriendDeletedEvent>())
							.Returns(_friendDeletedEvent);

			var mockedService = new Mock<INavigationDataProvider>(MockBehavior.Strict);
			mockedService.Setup(ds => ds.GetAllFriends()).Returns(_friends);
			_navigationViewModel = new NavigationViewModel(mockedService.Object,
															_eventAggregator.Object);
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

		[Fact]
		public void Save_LookupItem_ShouldUpdateNavigationItem()
		{
			_navigationViewModel.Load();
			var navigationItem = _navigationViewModel.Friends.First();
			var payload = new Friend
			{
				Id = navigationItem.Id,
				FirstName = "FriendFirstName"
			};

			_friendSavedEvent.Publish(payload);

			navigationItem.DisplayMember.Should()
				.Be($"{payload.FirstName} {payload.LastName}",
					"navigation with the specified id should be updated");
		}

		[Fact]
		public void Save_NewFriend_ShouldCreateNavigationItemAddToList()
		{
			var beforeNewFriendSaving = _navigationViewModel.Friends.Count;
			var payload = new Friend
			{
				Id = _friends.Max(f => f.Id) + 1,
				FirstName = "FriendFirstName"
			};
			_friends.Add(new LookupItem { Id = payload.Id, DisplayMember = payload.FirstName });

			_friendSavedEvent.Publish(payload);

			_navigationViewModel.Friends.Count.Should()
						.Be(beforeNewFriendSaving + 1, "another one friend has been added");

			var addedItem = _navigationViewModel.Friends.Single(item => item.Id == payload.Id);
			addedItem.DisplayMember.Should().Be($"{payload.FirstName} ");
		}

		[Fact]
		public void Delete_ExistingFriend_ShouldRemoveItemFromList()
		{
			_navigationViewModel.Load();
			_friendDeletedEvent.Publish(_friends[0].Id);
			_navigationViewModel.Friends.Should()
				.NotContain(item => item.Id == _friends[0].Id,
					$"We've just deleted the friend with id ={_friends[0].Id}");
		}
	}
}