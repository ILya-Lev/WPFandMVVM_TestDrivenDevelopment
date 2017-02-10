using FriendStorage.Model;

namespace FriendStorage.UI.Wrappers
{
	public class AddressWrapper : ModelWrapper<Address>
	{
		public AddressWrapper(Address model) : base(model) { }

		public int Id => Model.Id;
		public string City { get { return Model.City; } set { SetValue(value); } }
		public string Street { get { return Model.Street; } set { SetValue(value); } }
		public string Streetnumber { get { return Model.Streetnumber; } set { SetValue(value); } }
	}
}