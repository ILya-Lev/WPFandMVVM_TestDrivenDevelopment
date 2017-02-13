using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers;
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
	}
}
