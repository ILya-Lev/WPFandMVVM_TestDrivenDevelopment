using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers;
using FriendStorage.UIxUnitTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FriendStorage.UIxUnitTests.Wrappers
{
	public class FriendWrapperTests
	{
		private readonly Friend _expectedFriend;
		private List<FriendEmail> _expectedFriendEmails;

		public FriendWrapperTests()
		{
			_expectedFriend = new Friend
			{
				FirstName = "Alan",
				LastName = "Johns",
				Address = new Address(),
				Emails = new List<FriendEmail>()
			};

			_expectedFriendEmails = new[]
			{
				new FriendEmail {Email = "user@domain.com", Comment = "some data"},
				new FriendEmail {Email = "user@domain.gov", Comment = "top secret"},
				new FriendEmail {Email = "user@domain.net", Comment = "general data"},
			}.ToList();

		}

		[Fact]
		public void Constructed_ShouldContainFriendInModelProperty()
		{
			var wrapper = new FriendWrapper(_expectedFriend);

			wrapper.Model.Should().Be(_expectedFriend);
		}

		[Fact]
		public void Constructed_NullFriend_ShouldThrowException()
		{
			Action constructioning = () =>
			{
				var wrapper = new FriendWrapper(null);
			};

			constructioning.ShouldThrow<ArgumentNullException>("provided friend instance is null");
		}
		[Fact]
		public void Constructed_NullEmail_ShouldThrowException()
		{
			Action constructioning = () =>
			{
				_expectedFriend.Emails = null;
				var wrapper = new FriendWrapper(_expectedFriend);
			};

			constructioning.ShouldThrow<ArgumentNullException>("provided friend email list is null");
		}

		[Fact]
		public void GetFirstName_RandomString_ShouldProvideValueFromUnderlyingModel()
		{
			var wrapper = new FriendWrapper(_expectedFriend);

			wrapper.FirstName.Should().Be(_expectedFriend.FirstName, $"{wrapper.FirstName} != {_expectedFriend.FirstName}");
		}

		[Fact]
		public void SetFirstName_RandomString_ShouldUpdateUnderlyingModel()
		{
			var wrapper = new FriendWrapper(_expectedFriend);
			var newFristName = "Angus";

			newFristName.Should().NotBe(_expectedFriend.FirstName,
				"new first name should differ from expected one before test action invocation");

			wrapper.FirstName = newFristName;

			wrapper.FirstName.Should().Be(_expectedFriend.FirstName, $"{wrapper.FirstName} != {_expectedFriend.FirstName}");
		}

		[Fact]
		public void SetClassProperty_NewValue_ShouldRaisePropertyChangeEvent()
		{
			var wrapper = new FriendWrapper(_expectedFriend);

			var fired = wrapper.IsPropertyChangedFired
			(
				action: () => wrapper.FirstName += "A",
				propertyName: nameof(wrapper.FirstName)
			);

			fired.Should().BeTrue("we've changed wrappers property value");
		}
		[Fact]
		public void SetClassProperty_TheSameValue_ShouldNotRaisePropertyChangeEvent()
		{
			var wrapper = new FriendWrapper(_expectedFriend);

			var fired = wrapper.IsPropertyChangedFired
			(
				action: () => wrapper.FirstName = wrapper.FirstName,
				propertyName: nameof(wrapper.FirstName)
			);

			fired.Should().BeFalse("we've set the same wrappers property value");
		}
		[Fact]
		public void SetStructProperty_NewValue_ShouldRaisePropertyChangeEvent()
		{
			var wrapper = new FriendWrapper(_expectedFriend);

			var fired = wrapper.IsPropertyChangedFired
			(
				action: () => wrapper.IsDeveloper = !_expectedFriend.IsDeveloper,
				propertyName: nameof(wrapper.IsDeveloper)
			);

			fired.Should().BeTrue("we've changed wrappers property value");
		}
		[Fact]
		public void SetStructProperty_TheSameValue_ShouldNotRaisePropertyChangeEvent()
		{
			var wrapper = new FriendWrapper(_expectedFriend);

			var fired = wrapper.IsPropertyChangedFired
			(
				action: () => wrapper.IsDeveloper = _expectedFriend.IsDeveloper,
				propertyName: nameof(wrapper.IsDeveloper)
			);

			fired.Should().BeFalse("we've set the same wrappers property value");
		}
		[Fact]
		public void SetNullableProperty_NewValue_ShouldRaisePropertyChangeEvent()
		{
			var wrapper = new FriendWrapper(_expectedFriend);

			var fired = wrapper.IsPropertyChangedFired
			(
				action: () => wrapper.Birthday = DateTime.MaxValue,
				propertyName: nameof(wrapper.Birthday)
			);

			fired.Should().BeTrue("we've changed wrappers property value");
		}
		[Fact]
		public void SetNullableProperty_TheSameValue_ShouldNotRaisePropertyChangeEvent()
		{
			var wrapper = new FriendWrapper(_expectedFriend);

			var fired = wrapper.IsPropertyChangedFired
			(
				action: () => wrapper.Birthday = _expectedFriend.Birthday,
				propertyName: nameof(wrapper.Birthday)
			);

			fired.Should().BeFalse("we've set the same wrappers property value");
		}

		[Fact]
		public void ComplexProperty_OnCreation_ShouldInitialize()
		{
			var wrapper = new FriendWrapper(_expectedFriend);

			wrapper.Address.Should().NotBeNull("is initialized in ctor");
		}

		[Fact]
		public void Initialize_EmailNotEmpty_ShouldContainAllInitialOnes()
		{
			_expectedFriend.Emails = _expectedFriendEmails;

			var wrapper = new FriendWrapper(_expectedFriend);

			CheckIfEmailsAreSynchronized(wrapper);
		}

		[Fact]
		public void Add_Email_ShouldUpdateModel()
		{
			_expectedFriend.Emails = _expectedFriendEmails;
			var wrapper = new FriendWrapper(_expectedFriend);
			var additionaEmails = new[] {
				new FriendEmail{Comment = "addtional comment",Email = "add.1.user@domain.com"},
				new FriendEmail{Comment = "addtional 2 comment",Email = "add.2.user@domain.com"},
			};

			wrapper.Emails.Add(new FriendEmailWrapper(additionaEmails[0]));
			wrapper.Emails.Add(new FriendEmailWrapper(additionaEmails[1]));

			CheckIfEmailsAreSynchronized(wrapper);
		}
		[Fact]
		public void Remove_Email_ShouldUpdateModel()
		{
			_expectedFriend.Emails = _expectedFriendEmails;
			var wrapper = new FriendWrapper(_expectedFriend);

			wrapper.Emails.RemoveAt(0);

			CheckIfEmailsAreSynchronized(wrapper);
		}
		[Fact]
		public void Clear_Email_ShouldUpdateModel()
		{
			_expectedFriend.Emails = _expectedFriendEmails;
			var wrapper = new FriendWrapper(_expectedFriend);

			wrapper.Emails.Clear();

			CheckIfEmailsAreSynchronized(wrapper);
		}

		private void CheckIfEmailsAreSynchronized(FriendWrapper wrapper)
		{
			wrapper.Emails.Count.Should().Be(_expectedFriend.Emails.Count);
			wrapper.Emails.All(e => _expectedFriend.Emails.Any(fe => fe == e.Model))
				.Should().BeTrue("wrapper should contain all initial emails");
		}
	}
}