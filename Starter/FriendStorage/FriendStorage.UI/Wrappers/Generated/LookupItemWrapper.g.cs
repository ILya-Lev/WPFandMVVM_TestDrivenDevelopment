using System;
using System.Linq;
using FriendStorage.Model;
using FriendStorage.UI.Wrappers.Base;

namespace FriendStorage.UI.Wrappers
{
	public partial class LookupItemWrapper : ModelWrapper<LookupItem>
	{
		public LookupItemWrapper(LookupItem model) : base(model)
		{
		}

		public System.Int32 Id { get { return Model.Id; } set { SetValue(value); } }
		public System.Int32 IdOriginalValue => GetOriginalValue<System.Int32>(nameof(Id));
		public bool IdIsChanged => GetIsChanged(nameof(Id));

		public System.String DisplayMember { get { return Model.DisplayMember; } set { SetValue(value); } }
		public System.String DisplayMemberOriginalValue => GetOriginalValue<System.String>(nameof(DisplayMember));
		public bool DisplayMemberIsChanged => GetIsChanged(nameof(DisplayMember));


	}
}
