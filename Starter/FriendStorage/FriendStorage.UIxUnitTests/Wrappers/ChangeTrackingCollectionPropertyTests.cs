using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers;
using FriendStorage.UIxUnitTests.Extensions;
using System.Collections.Generic;
using Xunit;

namespace FriendStorage.UIxUnitTests.Wrappers
{
	public class ChangeTrackingCollectionPropertyTests
	{
		private Friend _friend;
		private FriendWrapper _friendWrapper;

		public ChangeTrackingCollectionPropertyTests()
		{
			_friend = new Friend
			{
				FirstName = "Alan",
				Address = new Address(),
				Emails = new List<FriendEmail>()
				{
					new FriendEmail {Email = "user@domain.com"},
					new FriendEmail { Email = "admin@domain.com" },
				}
			};
			_friendWrapper = new FriendWrapper(_friend);
		}

		[Fact]
		public void Add_Email_ShouldMarkAsChangedFriendWrapper()
		{
			var newEmail = new FriendEmailWrapper(new FriendEmail { Email = "boss@domain.com" });

			_friendWrapper.Emails.Add(newEmail);

			_friendWrapper.IsChanged.Should().BeTrue("new email has been added");
		}

		[Fact]
		public void RaisePropertyChangeEvent_EmailCollectionChange_ShouldRaiseTheEventForWrapper()
		{
			var newEmail = new FriendEmailWrapper(new FriendEmail { Email = "boss@domain.com" });
			var fired = _friendWrapper.IsPropertyChangedFired
			(
				action: () => _friendWrapper.Emails.Add(newEmail),
				propertyName: nameof(_friendWrapper.IsChanged)
			);

			fired.Should().BeTrue("event should be fired on wrapper change");
		}

		[Fact]
		public void Accept_OnWrapper_ShouldAcceptOnEmails()
		{
			var newEmail = new FriendEmailWrapper(new FriendEmail { Email = "boss@domain.com" });
			_friendWrapper.Emails.Add(newEmail);

			_friendWrapper.AcceptChanges();

			_friendWrapper.Emails.IsChanged.Should().BeFalse("changes are accepted");
			_friendWrapper.IsChanged.Should().BeFalse("changes are accepted");
		}
		[Fact]
		public void Reject_OnWrapper_ShouldAcceptOnEmails()
		{
			var newEmail = new FriendEmailWrapper(new FriendEmail { Email = "boss@domain.com" });
			_friendWrapper.Emails.Add(newEmail);

			_friendWrapper.RejectChanges();

			_friendWrapper.Emails.IsChanged.Should().BeFalse("changes are rejected");
			_friendWrapper.IsChanged.Should().BeFalse("changes are rejected");
		}
	}
}