using System;
using System.Diagnostics;

namespace FriendStorage.Model
{
	[DebuggerDisplay(value:
		"Id={" + nameof(Id) + "}; Label={" + nameof(FirstName) + "} {" + nameof(LastName) + "}")]
	public class Friend
	{
		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTime? Birthday { get; set; }

		public bool IsDeveloper { get; set; }
	}
}
