using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FriendStorage.UI.Wrappers
{
	public partial class FriendEmailWrapper
	{
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (string.IsNullOrWhiteSpace(Email))
			{
				yield return new ValidationResult("Email is required", new[] { nameof(Email) });
			}
			else
			{
				if (!new EmailAddressAttribute().IsValid(Email))
					yield return new ValidationResult("Invalid email address format",
																new[] { nameof(Email) });
			}
		}
	}
}