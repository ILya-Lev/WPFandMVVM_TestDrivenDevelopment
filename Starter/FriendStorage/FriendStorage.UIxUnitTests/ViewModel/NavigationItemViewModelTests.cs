using FluentAssertions;
using FriendStorage.UI.Events;
using FriendStorage.UIxUnitTests.Extensions;
using Moq;
using Prism.Events;
using Xunit;
namespace FriendStorage.UI.ViewModel.Tests
{
	public class NavigationItemViewModelTests
	{
		private Mock<OpenFriendEditViewEvent> _openFriendEvent;
		private Mock<IEventAggregator> _eventAggregator;

		public NavigationItemViewModelTests()
		{
			_openFriendEvent = new Mock<OpenFriendEditViewEvent>();
			_eventAggregator = new Mock<IEventAggregator>();
			_eventAggregator.Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
				.Returns(_openFriendEvent.Object);
		}

		[Fact]
		public void NavigationItemViewModel_ShouldPublishOpenFriendEditViewEvent()
		{
			const int id = 7;
			var navigationItemVM = new NavigationItemViewModel(id, "hello", _eventAggregator.Object);

			navigationItemVM.OpenFriendEditViewCommand.Execute(null);

			_openFriendEvent.Verify(e => e.Publish(id), Times.Once);
		}

		[Fact]
		public void DisplayMember_Set_ShouldRaisePropertyChangedEvent()
		{
			var navigationItemVM = new NavigationItemViewModel(1, "hello", _eventAggregator.Object);

			var isFired = navigationItemVM.IsPropertyChangedFired(
				action: () => navigationItemVM.DisplayMember = "new value",
				propertyName: nameof(navigationItemVM.DisplayMember)
			);

			isFired.Should().BeTrue($"{nameof(navigationItemVM.DisplayMember)} has been changed");
		}

	}
}