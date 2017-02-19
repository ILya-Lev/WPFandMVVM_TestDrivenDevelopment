using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers;
using System.Collections.Generic;
using Xunit;

namespace FriendStorage.UIxUnitTests.Wrappers
{
	public class ValidationClassLevelTests
	{
		private readonly Friend _friend;

		public ValidationClassLevelTests()
		{
			_friend = new Friend
			{
				FirstName = "Alan",
				Address = new Address { City = "London" },
				Emails = new List<FriendEmail>
				{
					new FriendEmail {Email = "user@domain.com"},
					new FriendEmail {Email = "admin@domain.com"},
				}
			};
		}

		[Fact]
		public void Creation_DeveloperWithoutEmails_ShouldBeInvalid()
		{
			_friend.IsDeveloper = true;
			_friend.Emails.Clear();
			var wrapper = new FriendWrapper(_friend);

			wrapper.IsValid.Should().BeFalse("developer must have an email");
		}
		[Fact]
		public void NotDeveloper_WithoutEmails_ShouldBeValid()
		{
			_friend.IsDeveloper = true;
			_friend.Emails.Clear();
			var wrapper = new FriendWrapper(_friend);

			wrapper.IsDeveloper = false;

			wrapper.IsValid.Should().BeTrue("non-developer could have no emails");
		}
		[Fact]
		public void Developer_AddedEmails_ShouldBeValid()
		{
			_friend.IsDeveloper = true;
			_friend.Emails.Clear();
			var wrapper = new FriendWrapper(_friend);

			wrapper.Emails.Add(new FriendEmailWrapper(new FriendEmail { Email = "added@domain.com" }));

			wrapper.IsValid.Should().BeTrue("developer already has an email");
		}
	}
}
