using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FriendStorage.UI.Wrappers
{
	public partial class AddressWrapper
	{
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (string.IsNullOrWhiteSpace(City))
			{
				yield return new ValidationResult("City is required", new[] { nameof(City) });
			}
		}
	}
}