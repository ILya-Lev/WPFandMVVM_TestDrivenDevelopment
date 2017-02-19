using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers;
using FriendStorage.UIxUnitTests.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FriendStorage.UIxUnitTests.Wrappers
{
	public class ValidationCollectionPropertyTests
	{
		private readonly Friend _friend;
		private readonly List<FriendEmail> _emails;

		public ValidationCollectionPropertyTests()
		{
			_emails = new List<FriendEmail>
			{
				new FriendEmail {Email = "user@domain.com"},
				new FriendEmail {Email = "admin@doman.com"}
			};

			_friend = new Friend
			{
				FirstName = "Thomas",
				Address = new Address { City = "London" },
				Emails = _emails
			};
		}

		[Fact]
		public void Email_ChangeToEmptyAndBack_ShouldReflectIsValidOfWrapper()
		{
			var wrapper = new FriendWrapper(_friend);

			wrapper.Emails.First().Email = "";
			wrapper.IsValid.Should().BeFalse("email address should not be empty");

			wrapper.Emails.First().Email = "somebody@gmail.com";
			wrapper.IsValid.Should().BeTrue("email address is valid now");

			wrapper.Emails.First().Email = "som@ebody@gmail.com";
			wrapper.IsValid.Should().BeFalse("email address has invalid format");
		}

		[Fact]
		public void Email_Initialization_ShouldSetRootObjetAsInvalid()
		{
			_friend.Emails.First().Email = "";
			var wrapper = new FriendWrapper(_friend);

			wrapper.IsValid.Should().BeFalse("email address is empty");
		}

		[Fact]
		public void Remove_InvalidEmail_ShouldSetIsValidToTrue()
		{
			_friend.Emails.Add(new FriendEmail { Email = "som@ebody@gmail.com" });
			var wrapper = new FriendWrapper(_friend);

			wrapper.IsValid.Should().BeFalse("has invalid email");
			wrapper.GetErrors(nameof(wrapper.Emails)).Should().HaveCount(1);

			var fired = wrapper.IsPropertyChangedFired(
				action: () => wrapper.Emails.RemoveAt(_friend.Emails.Count - 1),
				propertyName: nameof(wrapper.IsValid)
			);

			fired.Should().BeTrue("is valid property has been changed");
			wrapper.IsValid.Should().BeTrue("invalid email has been removed from the collection");
		}
		[Fact]
		public void Add_InvalidEmail_ShouldSetIsValidToFalse()
		{
			var wrapper = new FriendWrapper(_friend);
			wrapper.IsValid.Should().BeTrue();

			var email = new FriendEmail { Email = "som@ebody@gmail.com" };
			var fired = wrapper.IsPropertyChangedFired(
				action: () => wrapper.Emails.Add(new FriendEmailWrapper(email)),
				propertyName: nameof(wrapper.IsValid)
			);

			fired.Should().BeTrue("is valid property has been changed");
			wrapper.IsValid.Should().BeFalse("invalid email has been added to the collection");
			wrapper.GetErrors(nameof(wrapper.Emails)).Should().HaveCount(1);
		}
	}
}