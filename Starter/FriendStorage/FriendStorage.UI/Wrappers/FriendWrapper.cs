using FriendStorage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FriendStorage.UI.Wrappers
{
	public class FriendWrapper : ModelWrapper<Friend>
	{
		public FriendWrapper(Friend friend) : base(friend)
		{

		}

		protected override void InitializeCollectionProperty(Friend friend)
		{
			Emails = new ChangeTrackingCollection<FriendEmailWrapper>
			(
				friend.Emails.Select(e => new FriendEmailWrapper(e))
			);
			RegisterCollection(Emails, Model.Emails);
		}

		protected override void InitializeComplexProperty(Friend friend)
		{
			Address = new AddressWrapper(friend.Address);
			RegisterComplex(Address);
		}

		public int Id { get { return Model.Id; } set { SetValue(value); } }
		public int IdOriginalValue => GetOriginalValue<int>(nameof(Id));
		public bool IdIsChanged => GetIsChanged(nameof(Id));

		public int FriendGroupId { get { return Model.FriendGroupId; } set { SetValue(value); } }
		public int FriendGroupIdOriginalValue => GetOriginalValue<int>(nameof(FriendGroupId));
		public bool FriendGroupIdIsChanged => GetIsChanged(nameof(FriendGroupId));

		public string FirstName { get { return Model.FirstName; } set { SetValue(value); } }
		public string FirstNameOriginalValue => GetOriginalValue<string>(nameof(FirstName));
		public bool FirstNameIsChanged => GetIsChanged(nameof(FirstName));

		public string LastName { get { return Model.LastName; } set { SetValue(value); } }
		public string LastNameOriginalValue => GetOriginalValue<string>(nameof(LastName));
		public bool LastNameIsChanged => GetIsChanged(nameof(LastName));

		public DateTime? Birthday { get { return Model.Birthday; } set { SetValue(value); } }
		public DateTime? BirthdayOriginalValue => GetOriginalValue<DateTime?>(nameof(Birthday));
		public bool BirthdayIsChanged => GetIsChanged(nameof(Birthday));

		public bool IsDeveloper { get { return Model.IsDeveloper; } set { SetValue(value); } }
		public bool IsDeveloperOriginalValue => GetOriginalValue<bool>(nameof(IsDeveloper));
		public bool IsDeveloperIsChanged => GetIsChanged(nameof(IsDeveloper));

		public AddressWrapper Address { get; private set; }

		public ChangeTrackingCollection<FriendEmailWrapper> Emails { get; private set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if(string.IsNullOrWhiteSpace(FirstName))
			{
				yield return new ValidationResult("Firstname is required", new[] { nameof(FirstName) });
			}
			if(IsDeveloper && Emails.Count == 0)
			{
				yield return new ValidationResult("Developer should have at least one email",
					new[] { nameof(IsDeveloper), nameof(Emails) });
			}
		}
	}
}
