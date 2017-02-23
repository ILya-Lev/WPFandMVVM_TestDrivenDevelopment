using System;
using System.Linq;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers.Base;

namespace FriendStorage.UI.Wrappers
{
	public class FriendEmailWrapper : ModelWrapper<FriendEmail>
	{
		public FriendEmailWrapper(FriendEmail model) : base(model)
		{
		}

		
		public System.Int32 Id { get { return Model.Id; } set { SetValue(value); } }
		public System.Int32 IdOriginalValue => GetOriginalValue<System.Int32>(nameof(Id));
		public bool IdIsChanged => GetIsChanged(nameof(Id));


		public System.String Email { get { return Model.Email; } set { SetValue(value); } }
		public System.String EmailOriginalValue => GetOriginalValue<System.String>(nameof(Email));
		public bool EmailIsChanged => GetIsChanged(nameof(Email));


		public System.String Comment { get { return Model.Comment; } set { SetValue(value); } }
		public System.String CommentOriginalValue => GetOriginalValue<System.String>(nameof(Comment));
		public bool CommentIsChanged => GetIsChanged(nameof(Comment));


	}
}
