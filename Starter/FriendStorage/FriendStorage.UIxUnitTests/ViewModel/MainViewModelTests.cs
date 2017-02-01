using Moq;
using System;
using System.Linq.Expressions;
using Xunit;

namespace FriendStorage.UI.ViewModel.Tests
{
	public class MainViewModelTests
	{
		[Fact()]
		public void Load_ShouldDelegateCallToNavigationVM()
		{
			Expression<Action<INavigationViewModel>> setupLoad = vm => vm.Load();
			var navigationVM = new Mock<INavigationViewModel>(MockBehavior.Strict);
			navigationVM.Setup(setupLoad);
			var viewModel = new MainViewModel(navigationVM.Object);

			viewModel.Load();

			navigationVM.Verify(setupLoad, Times.Once);
		}
	}
}