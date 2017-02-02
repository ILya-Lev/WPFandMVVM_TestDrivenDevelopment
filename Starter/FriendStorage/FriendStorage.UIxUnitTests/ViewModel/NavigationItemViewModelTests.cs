using FriendStorage.UI.Events;
using Moq;
using Prism.Events;
using Xunit;

namespace FriendStorage.UI.ViewModel.Tests
{
	public class NavigationItemViewModelTests
	{
		[Fact()]
		public void NavigationItemViewModel_ShouldPublishOpenFriendEditViewEvent()
		{
			const int id = 7;

			var openFriendEvent = new Mock<OpenFriendEditViewEvent>();
			var eventAggregator = new Mock<IEventAggregator>();
			eventAggregator.Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
							.Returns(openFriendEvent.Object);

			var navigationItemVM = new NavigationItemViewModel(id, "hello", eventAggregator.Object);

			navigationItemVM.OpenFriendEditViewCommand.Execute(null);

			openFriendEvent.Verify(e => e.Publish(id), Times.Once);
		}
	}
}