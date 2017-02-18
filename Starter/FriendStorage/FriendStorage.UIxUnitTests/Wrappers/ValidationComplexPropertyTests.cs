using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers;
using FriendStorage.UIxUnitTests.Extensions;
using Xunit;

namespace FriendStorage.UIxUnitTests.Wrappers
{
	public class ValidationComplexPropertyTests
	{
		private Friend _friend;

		public ValidationComplexPropertyTests()
		{
			_friend = new Friend
			{
				FirstName = "John",
				Address = new Address { City = "Munich" }
			};
		}

		[Fact]
		public void Address_ValidationChanged_ShouldChangeIsValidForWholeWrapper()
		{
			var wrapper = new FriendWrapper(_friend);

			wrapper.Address.City = "";

			wrapper.IsValid.Should().BeFalse();
			wrapper.Address.IsValid.Should().BeFalse();

			wrapper.Address.City = "London";

			wrapper.IsValid.Should().BeTrue();
			wrapper.Address.IsValid.Should().BeTrue();
		}

		[Fact]
		public void Address_Initialization_ShouldValidateCity()
		{
			_friend.Address.City = "";
			var wrapper = new FriendWrapper(_friend);

			wrapper.IsValid.Should().BeFalse("initialized with invalid city");

			wrapper.Address.City = "Austin";

			wrapper.IsValid.Should().BeTrue("valid city value has been set");
		}

		[Fact]
		public void Address_Changed_ShouldRaiseIsValidPropertyChangedEvent()
		{
			var wrapper = new FriendWrapper(_friend);
			var fired = wrapper.IsPropertyChangedFired(
				action: () => wrapper.Address.City = "",
				propertyName: nameof(wrapper.IsValid)
			);

			fired.Should().BeTrue();
		}
	}
}