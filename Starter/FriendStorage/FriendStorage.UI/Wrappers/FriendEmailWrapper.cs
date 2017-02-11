using FriendStorage.Model;

namespace FriendStorage.UI.Wrappers
{
	public class FriendEmailWrapper : ModelWrapper<FriendEmail>
	{
		public FriendEmailWrapper(FriendEmail model) : base(model)
		{
		}

		public int Id => Model.Id;
		public string Email { get { return Model.Email; } set { SetValue(value); } }
		public string Comment { get { return Model.Comment; } set { SetValue(value); } }
	}
}
