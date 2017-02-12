using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers;
using FriendStorage.UIxUnitTests.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FriendStorage.UIxUnitTests.Wrappers
{
	public class ChangeTrackingSimplePropertiesTests
	{
		private readonly Friend _expectedFriend;
		private List<FriendEmail> _expectedFriendEmails;
		private FriendWrapper _wrapper;

		public ChangeTrackingSimplePropertiesTests()
		{
			_expectedFriend = new Friend
			{
				FirstName = "Alan",
				LastName = "Johns",
				Address = new Address(),
				Emails = new List<FriendEmail>()
			};

			_wrapper = new FriendWrapper(_expectedFriend);
		}

		[Fact]
		public void FirstNameOriginalValue_FirstNameChanged_ShouldReturnOriginalValue()
		{
			var originalValue = _expectedFriend.FirstName;
			_wrapper.FirstNameOriginalValue.Should().Be(originalValue);

			_wrapper.FirstName = new string(originalValue.Reverse().ToArray());
			_wrapper.FirstNameOriginalValue.Should().Be(originalValue);
		}

		[Fact]
		public void FirstNameIsCanged_FirstNameChanged_ShouldBeTrue()
		{
			_wrapper.FirstNameIsChanged.Should().BeFalse();
			_wrapper.IsChanged.Should().BeFalse();

			_wrapper.FirstName = _expectedFriend.FirstName + _expectedFriend.LastName;
			_wrapper.FirstNameIsChanged.Should().BeTrue();
			_wrapper.IsChanged.Should().BeTrue();

			_wrapper.FirstName = _wrapper.FirstNameOriginalValue;
			_wrapper.FirstNameIsChanged.Should().BeFalse();
			_wrapper.IsChanged.Should().BeFalse();

			_wrapper.FirstName = _wrapper.FirstNameOriginalValue;
			_wrapper.FirstNameIsChanged.Should().BeFalse();
			_wrapper.IsChanged.Should().BeFalse();
		}
		[Fact]
		public void FirstNameIsCanged_WithTheSameValue_ShouldBeTrue()
		{
			_wrapper.FirstName = _expectedFriend.FirstName;
			_wrapper.FirstNameIsChanged.Should().BeFalse();
		}

		[Fact]
		public void FirstNameIsChanged_OnChange_ShouldRaisePropertyChangedEvent()
		{
			var fired = _wrapper.IsPropertyChangedFired
			(
				action: () => _wrapper.FirstName = "Julia",
				propertyName: nameof(_wrapper.FirstNameIsChanged)
			);

			fired.Should().BeTrue();
		}
		[Fact]
		public void IsChanged_OnChange_ShouldRaisePropertyChangedEvent()
		{
			var fired = _wrapper.IsPropertyChangedFired
			(
				action: () => _wrapper.FirstName = "Julia",
				propertyName: nameof(_wrapper.IsChanged)
			);

			fired.Should().BeTrue();
		}

		[Fact]
		public void AcceptChanges_ChangedObject_ShouldResetIsChangedAndMakeNewValueAsOriginal()
		{
			_wrapper.FirstName = "Julia";
			_wrapper.FirstNameOriginalValue.Should().Be("Alan");
			_wrapper.FirstNameIsChanged.Should().BeTrue();
			_wrapper.IsChanged.Should().BeTrue();

			_wrapper.AcceptChanges();

			_wrapper.FirstNameOriginalValue.Should().Be("Julia");
			_wrapper.FirstNameIsChanged.Should().BeFalse();
			_wrapper.IsChanged.Should().BeFalse();
		}
		[Fact]
		public void RejectChanges_ChangedObject_ShouldResetIsChangedAndRestoreOriginalValues()
		{
			_wrapper.FirstName = "Julia";
			_wrapper.FirstName.Should().Be("Julia");
			_wrapper.FirstNameOriginalValue.Should().Be("Alan");
			_wrapper.FirstNameIsChanged.Should().BeTrue();
			_wrapper.IsChanged.Should().BeTrue();

			_wrapper.RejectChanges();

			_wrapper.FirstName.Should().Be("Alan");
			_wrapper.FirstNameOriginalValue.Should().Be("Alan");
			_wrapper.FirstNameIsChanged.Should().BeFalse();
			_wrapper.IsChanged.Should().BeFalse();
		}
	}
}
