using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FriendStorage.Model
{
	[DebuggerDisplay(value:
		"Id={" + nameof(Id) + "}; Label={" + nameof(FirstName) + "} {" + nameof(LastName) + "}")]
	public class Friend
	{
		public int Id { get; set; }

		public int FriendGroupId { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTime? Birthday { get; set; }

		public bool IsDeveloper { get; set; }

		public Address Address { get; set; }

		public List<FriendEmail> Emails { get; set; } = new List<FriendEmail>();
	}
}
