using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers;
using Xunit;

namespace FriendStorage.UIxUnitTests.Wrappers
{
	public class ValidationSimplePropertyTests
	{
		private Friend _friend;

		public ValidationSimplePropertyTests()
		{
			_friend = new Friend
			{
				FirstName = "Thomas",
				Address = new Address { City = "Mulhiem" },
			};
		}

		[Fact]
		public void FirstName_IsEmpty_ShouldReturnValidationError()
		{
			var wrapper = new FriendWrapper(_friend);

			bool fired = false;
			wrapper.ErrorsChanged += (sender, args)
				=> fired = args.PropertyName == nameof(wrapper.FirstName);

			wrapper.FirstName = "";

			wrapper.HasErrors.Should().BeTrue("first name should not be empty");
			wrapper.GetErrors(nameof(wrapper.FirstName)).Should().HaveCount(1, "there is one error");
			fired.Should().BeTrue("error state has been changed");

			wrapper.FirstName = "Alan";
			wrapper.HasErrors.Should().BeFalse("first name is valid");
			wrapper.GetErrors(nameof(wrapper.FirstName)).Should().HaveCount(0, "there is no errors");
			fired.Should().BeTrue("error state has been changed");
		}
	}
}