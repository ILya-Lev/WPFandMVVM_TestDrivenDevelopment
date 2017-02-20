using FriendStorage.Model;
using System.ComponentModel.DataAnnotations;
using FriendStorage.UI.Wrappers.Base;

namespace FriendStorage.UI.Wrappers
{
	public class AddressWrapper : ModelWrapper<Address>
	{
		public AddressWrapper(Address model) : base(model) { }

		public int Id => Model.Id;
		public int IdOriginalValue => GetOriginalValue<int>(nameof(Id));
		public bool IdIsChanged => GetIsChanged(nameof(Id));

		[Required(ErrorMessage = "City is a required property")]
		public string City { get { return Model.City; } set { SetValue(value); } }
		public string CityOriginalValue => GetOriginalValue<string>(nameof(City));
		public bool CityIsChanged => GetIsChanged(nameof(City));

		public string Street { get { return Model.Street; } set { SetValue(value); } }
		public string StreetOriginalValue => GetOriginalValue<string>(nameof(Street));
		public bool StreetIsChanged => GetIsChanged(nameof(Street));

		public string Streetnumber { get { return Model.Streetnumber; } set { SetValue(value); } }
		public string StreetnumberOriginalValue => GetOriginalValue<string>(nameof(Streetnumber));
		public bool StreetnumberIsChanged => GetIsChanged(nameof(Streetnumber));
	}
}