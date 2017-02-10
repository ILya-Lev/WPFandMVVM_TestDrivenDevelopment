using FriendStorage.Model;
using System;

namespace FriendStorage.UI.Wrappers
{
	public class FriendWrapper : ModelWrapper<Friend>
	{
		public FriendWrapper(Friend friend) : base(friend)
		{
			Address = new AddressWrapper(friend.Address);
		}

		public void AcceptChanges() => IsChanged = false;

		public int Id => Model.Id;

		public int FriendGroupId { get { return Model.FriendGroupId; } set { SetValue(value); } }

		public string FirstName { get { return Model.FirstName; } set { SetValue(value); } }

		public string LastName { get { return Model.LastName; } set { SetValue(value); } }

		public DateTime? Birthday { get { return Model.Birthday; } set { SetValue(value); } }

		public bool IsDeveloper { get { return Model.IsDeveloper; } set { SetValue(value); } }

		public AddressWrapper Address { get; }
	}
}
