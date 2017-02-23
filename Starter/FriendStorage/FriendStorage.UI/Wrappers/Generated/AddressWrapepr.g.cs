using System;
using System.Linq;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers.Base;

namespace FriendStorage.UI.Wrappers
{
	public class AddressWrapper : ModelWrapper<Address>
	{
		public AddressWrapper(Address model) : base(model)
		{
		}

		
		public System.Int32 Id { get { return Model.Id; } set { SetValue(value); } }
		public System.Int32 IdOriginalValue => GetOriginalValue<System.Int32>(nameof(Id));
		public bool IdIsChanged => GetIsChanged(nameof(Id));


		public System.String City { get { return Model.City; } set { SetValue(value); } }
		public System.String CityOriginalValue => GetOriginalValue<System.String>(nameof(City));
		public bool CityIsChanged => GetIsChanged(nameof(City));


		public System.String Street { get { return Model.Street; } set { SetValue(value); } }
		public System.String StreetOriginalValue => GetOriginalValue<System.String>(nameof(Street));
		public bool StreetIsChanged => GetIsChanged(nameof(Street));


		public System.String Streetnumber { get { return Model.Streetnumber; } set { SetValue(value); } }
		public System.String StreetnumberOriginalValue => GetOriginalValue<System.String>(nameof(Streetnumber));
		public bool StreetnumberIsChanged => GetIsChanged(nameof(Streetnumber));


	}
}
