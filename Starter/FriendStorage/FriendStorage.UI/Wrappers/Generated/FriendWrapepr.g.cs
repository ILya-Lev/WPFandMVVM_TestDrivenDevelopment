using System;
using System.Linq;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers.Base;

namespace FriendStorage.UI.Wrappers
{
	public class FriendWrapper : ModelWrapper<Friend>
	{
		public FriendWrapper(Friend model) : base(model)
		{
		}

		
		public System.Int32 Id { get { return Model.Id; } set { SetValue(value); } }
		public System.Int32 IdOriginalValue => GetOriginalValue<System.Int32>(nameof(Id));
		public bool IdIsChanged => GetIsChanged(nameof(Id));


		public System.Int32 FriendGroupId { get { return Model.FriendGroupId; } set { SetValue(value); } }
		public System.Int32 FriendGroupIdOriginalValue => GetOriginalValue<System.Int32>(nameof(FriendGroupId));
		public bool FriendGroupIdIsChanged => GetIsChanged(nameof(FriendGroupId));


		public System.String FirstName { get { return Model.FirstName; } set { SetValue(value); } }
		public System.String FirstNameOriginalValue => GetOriginalValue<System.String>(nameof(FirstName));
		public bool FirstNameIsChanged => GetIsChanged(nameof(FirstName));


		public System.String LastName { get { return Model.LastName; } set { SetValue(value); } }
		public System.String LastNameOriginalValue => GetOriginalValue<System.String>(nameof(LastName));
		public bool LastNameIsChanged => GetIsChanged(nameof(LastName));


		public System.Nullable<System.DateTime> Birthday { get { return Model.Birthday; } set { SetValue(value); } }
		public System.Nullable<System.DateTime> BirthdayOriginalValue => GetOriginalValue<System.Nullable<System.DateTime>>(nameof(Birthday));
		public bool BirthdayIsChanged => GetIsChanged(nameof(Birthday));


		public System.Boolean IsDeveloper { get { return Model.IsDeveloper; } set { SetValue(value); } }
		public System.Boolean IsDeveloperOriginalValue => GetOriginalValue<System.Boolean>(nameof(IsDeveloper));
		public bool IsDeveloperIsChanged => GetIsChanged(nameof(IsDeveloper));

		public AddressWrapper Address { get; private set; }
		public ChangeTrackingCollection<FriendEmailWrapper> Emails { get; private set; }

		protected override void InitializeComplexProperty(Friend model)
		{
			if (model.Address == null)
				throw new ArgumentNullException(nameof(Address));

			Address = new AddressWrapper(model.Address);
			RegisterComplex(Address);
		}

		protected override void InitializeCollectionProperty(Friend model)
		{
			if (model.Emails == null)
				throw new ArgumentNullException(nameof(Emails));

			Emails = new ChangeTrackingCollection<FriendEmailWrapper>(
				model.Emails.Select(item => new FriendEmailWrapper(item))
			);
			RegisterCollection(Emails, model.Emails);
		}

	}
}
