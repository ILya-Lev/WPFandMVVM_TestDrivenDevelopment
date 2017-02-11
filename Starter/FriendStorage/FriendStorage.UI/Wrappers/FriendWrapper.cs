using FriendStorage.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FriendStorage.UI.Wrappers
{
	public class FriendWrapper : ModelWrapper<Friend>
	{
		public FriendWrapper(Friend friend) : base(friend)
		{
			Address = new AddressWrapper(friend.Address);
			//Address.PropertyChanged += (sender, args) => friend.Address = (Address) sender;

			Emails = new ObservableCollection<FriendEmailWrapper>
			(
				friend.Emails.Select(e => new FriendEmailWrapper(e))
			);
			RegisterCollection(Emails, Model.Emails);
		}

		public void AcceptChanges() => IsChanged = false;

		public int Id => Model.Id;

		public int FriendGroupId { get { return Model.FriendGroupId; } set { SetValue(value); } }

		public string FirstName { get { return Model.FirstName; } set { SetValue(value); } }

		public string LastName { get { return Model.LastName; } set { SetValue(value); } }

		public DateTime? Birthday { get { return Model.Birthday; } set { SetValue(value); } }

		public bool IsDeveloper { get { return Model.IsDeveloper; } set { SetValue(value); } }

		public AddressWrapper Address { get; }

		public ObservableCollection<FriendEmailWrapper> Emails { get; }
	}
}
