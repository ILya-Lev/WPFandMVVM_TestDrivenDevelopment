using FluentAssertions;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers;
using System.Collections.Generic;
using System.Linq;
using FriendStorage.UI.Wrappers.Base;
using Xunit;

namespace FriendStorage.UIxUnitTests.Wrappers
{
	public class ChangeTrackingCollectionTests
	{
		private List<FriendEmailWrapper> _emails;
		private ChangeTrackingCollection<FriendEmailWrapper> _collection;

		public ChangeTrackingCollectionTests()
		{
			_emails = new[]
			{
				new FriendEmailWrapper(new FriendEmail {Email = "user@domain.com"}),
				new FriendEmailWrapper(new FriendEmail {Email = "admin@domain.com"}),
			}.ToList();

			_collection = new ChangeTrackingCollection<FriendEmailWrapper>(_emails);
		}

		[Fact]
		public void Add_NewItem_ShouldTrackAddedItems()
		{
			_collection.IsChanged.Should().BeFalse("has just been initialized");

			var newEmail = new FriendEmailWrapper(new FriendEmail { Email = "boss@domain.com" });

			_collection.Add(newEmail);

			_collection.IsChanged.Should().BeTrue("new item has been added");
			_collection.AddedItems.Count.Should().Be(1);
			_collection.RemovedItems.Count.Should().Be(0);
			_collection.ModifiedItems.Count.Should().Be(0);
			_collection.AddedItems.Should().Contain(newEmail);

			_collection.Remove(newEmail);

			_collection.IsChanged.Should().BeFalse("the item has been removed");
			_collection.AddedItems.Count.Should().Be(0);
			_collection.RemovedItems.Count.Should().Be(0);
			_collection.ModifiedItems.Count.Should().Be(0);

		}

		[Fact]
		public void Remove_ExistingItem_ShouldTrackRemovedItems()
		{
			_collection.IsChanged.Should().BeFalse("has just been initialized");

			var removedEmail = _emails.First();
			_collection.Remove(removedEmail);

			_collection.IsChanged.Should().BeTrue("an item has been removed");
			_collection.RemovedItems.Count.Should().Be(1);
			_collection.RemovedItems.Should().Contain(removedEmail);
		}

		[Fact]
		public void Modify_ExistingItem_ShouldTrackModifiedItems()
		{
			_collection.IsChanged.Should().BeFalse("has just been initialized");

			var newEmail = new FriendEmailWrapper(new FriendEmail { Email = "boss@domain.com" });
			_collection.Add(newEmail);

			_collection.IsChanged.Should().BeTrue("new item has been added");
			_collection.AddedItems.Count.Should().Be(1);
			_collection.AddedItems.Should().Contain(newEmail);
		}

		[Fact]
		public void Add_NewItem_ShouldNotTrackAsModified()
		{
			_collection.IsChanged.Should().BeFalse("has just been initialized");

			var newEmail = new FriendEmailWrapper(new FriendEmail { Email = "boss@domain.com" });
			_collection.Add(newEmail);

			_collection.IsChanged.Should().BeTrue("new item has been added");
			_collection.ModifiedItems.Count.Should().Be(0);
		}

		[Fact]
		public void Remove_ExistingItem_ShouldNotTrackAsModified()
		{
			_collection.IsChanged.Should().BeFalse("has just been initialized");

			var removedItem = _emails.First();
			_collection.Remove(removedItem);

			_collection.IsChanged.Should().BeTrue("new item has been deleted");
			_collection.ModifiedItems.Count.Should().Be(0);
		}

		[Fact]
		public void AcceptChanges_AfterModification_ShouldMarkAsNotChanged()
		{
			_collection.IsChanged.Should().BeFalse("has just been initialized");

			var newEmail = new FriendEmailWrapper(new FriendEmail { Email = "boss@domain.com" });
			_collection.Add(newEmail);

			_collection.AcceptChanges();

			_collection.IsChanged.Should().BeFalse("changes are accepted");
			_collection.AddedItems.Count.Should().Be(0);
			_collection.RemovedItems.Count.Should().Be(0);
			_collection.ModifiedItems.Count.Should().Be(0);
		}

		[Fact]
		public void RejectChanges_AfterModification_ShouldRestoreInitialState()
		{
			_collection.IsChanged.Should().BeFalse("has just been initialized");

			var newEmail = new FriendEmailWrapper(new FriendEmail { Email = "boss@domain.com" });
			_collection.Add(newEmail);

			_collection.RejectChanges();

			_collection.IsChanged.Should().BeFalse("changes are rejected");
			_collection.AddedItems.Count.Should().Be(0);
			_collection.RemovedItems.Count.Should().Be(0);
			_collection.ModifiedItems.Count.Should().Be(0);
		}
	}
}
