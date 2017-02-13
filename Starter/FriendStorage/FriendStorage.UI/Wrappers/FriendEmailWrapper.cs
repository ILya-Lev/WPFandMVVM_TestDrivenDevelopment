using FriendStorage.Model;

namespace FriendStorage.UI.Wrappers
{
	public class FriendEmailWrapper : ModelWrapper<FriendEmail>
	{
		public FriendEmailWrapper(FriendEmail model) : base(model)
		{
		}

		public int Id => Model.Id;
		public int IdOriginalValue => GetOriginalValue<int>(nameof(Id));
		public bool IdIsChanged => GetIsChanged(nameof(Id));

		public string Email { get { return Model.Email; } set { SetValue(value); } }
		public string EmailOriginalValue => GetOriginalValue<string>(nameof(Email));
		public bool EmailIsChanged => GetIsChanged(nameof(Email));

		public string Comment { get { return Model.Comment; } set { SetValue(value); } }
		public string CommentOriginalValue => GetOriginalValue<string>(nameof(Comment));
		public bool CommentIsChanged => GetIsChanged(nameof(Comment));
	}
}
