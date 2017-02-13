using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers;
using FriendStorage.UIxUnitTests.Extensions;
using System.Collections.Generic;
using Xunit;

namespace FriendStorage.UIxUnitTests.Wrappers
{
	public class ChangeTrackingComplexPropertiesTests
	{
		private FriendWrapper _wrapper;
		private Friend _expectedFriend;

		public ChangeTrackingComplexPropertiesTests()
		{
			_expectedFriend = new Friend
			{
				FirstName = "Alan",
				LastName = "Johns",
				Address = new Address { City = "Munich" },
				Emails = new List<FriendEmail>()
			};

			_wrapper = new FriendWrapper(_expectedFriend);
		}

		[Fact]
		public void City_Changed_ShouldMarkFriendWrapperAsChanged()
		{
			_wrapper.IsChanged.Should().BeFalse("no changes yet");

			_wrapper.Address.City = "London";

			_wrapper.IsChanged.Should().BeTrue($"city is changed from {_wrapper.Address.CityOriginalValue} to {_wrapper.Address.City}");

			_wrapper.Address.City = "Munich";
			_wrapper.IsChanged.Should().BeFalse($"city value is restored to {_wrapper.Address.City} - its original value");
		}
		[Fact]
		public void City_Changed_ShouldRaisePropertyChangedForWholeFriendWrapper()
		{
			var fired = _wrapper.IsPropertyChangedFired
			(
				action: () => _wrapper.Address.City = "London",
				propertyName: nameof(_wrapper.IsChanged)
			);

			fired.Should().BeTrue("wrapper has been changed");
		}

		[Fact]
		public void City_AccteptChanges_ShouldMarkAsUnmodified()
		{
			_wrapper.IsChanged.Should().BeFalse("no changes yet!");
			_wrapper.Address.City = "London";
			_wrapper.IsChanged.Should()
				.BeTrue($"city changed from {_wrapper.Address.CityOriginalValue} to {_wrapper.Address.City}");

			_wrapper.AcceptChanges();

			_wrapper.IsChanged.Should().BeFalse("changes are accepted");
			_wrapper.Address.CityOriginalValue.Should().Be("London", "changes are accepted");
		}

		[Fact]
		public void City_RejectChanges_ShouldMarkAsUnmodified()
		{
			_wrapper.IsChanged.Should().BeFalse("no changes yet!");
			_wrapper.Address.City = "London";
			_wrapper.IsChanged.Should()
				.BeTrue($"city changed from {_wrapper.Address.CityOriginalValue} to {_wrapper.Address.City}");

			_wrapper.RejectChanges();

			_wrapper.IsChanged.Should().BeFalse("object is reset");
			_wrapper.Address.CityOriginalValue.Should().Be("Munich", "object is reset");
		}
	}
}
