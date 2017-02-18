using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers;
using FriendStorage.UIxUnitTests.Extensions;
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

		[Fact]
		public void IsValid_ChangeFirstName_ShouldReflectErrorsAbsence()
		{
			var wrapper = new FriendWrapper(_friend);
			wrapper.IsValid.Should().BeTrue("first name is set");

			wrapper.FirstName = "";
			wrapper.IsValid.Should().BeFalse("first name is empty, i.e. invalid");

			wrapper.FirstName = "Alan";
			wrapper.IsValid.Should().BeTrue("first name is restored");
		}

		[Fact]
		public void IsValid_ChangeFirstName_ShouldRaisePropertyChangedForIsValid()
		{
			var wrapper = new FriendWrapper(_friend);
			var fired = wrapper.IsPropertyChangedFired
			(
				action: () => wrapper.FirstName = "",
				propertyName: nameof(wrapper.IsValid)
			);

			fired.Should().BeTrue();

			fired = wrapper.IsPropertyChangedFired
			(
				action: () => wrapper.FirstName = "Alan",
				propertyName: nameof(wrapper.IsValid)
			);

			fired.Should().BeTrue();
		}

		[Fact]
		public void FriendWrapper_Creation_ShouldSetErrors()
		{
			var wrapper = new FriendWrapper(new Friend
			{
				Address = new Address { City = "Mulhiem" },
			});
			wrapper.IsValid.Should().BeFalse("first name is empty");
			wrapper.GetErrors(nameof(wrapper.FirstName)).Should().HaveCount(1, "there is an error");
		}

		[Fact]
		public void FriendWrapper_ResetAfterBeingInvalid_ShouldBeValidAndRaiseErrorChangedEvent()
		{
			var wrapper = new FriendWrapper(_friend);

			wrapper.FirstName = "";

			var fired = false;
			wrapper.ErrorsChanged += (sender, args) =>
			{
				if(args.PropertyName == nameof(wrapper.FirstName))
					fired = true;
			};

			wrapper.RejectChanges();

			wrapper.IsValid.Should().BeTrue("restored initial valid value");
			fired.Should().BeTrue();
		}
	}
}