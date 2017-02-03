using System;
using System.ComponentModel;

namespace FriendStorage.UIxUnitTests.Extensions
{
	public static class NotifyPropertyChangedExtensions
	{
		public static bool IsPropertyChangedFiered(this INotifyPropertyChanged sender,
													Action action,
													string propertyName)
		{
			var fiered = false;
			sender.PropertyChanged += (s, args) =>
			{
				if (args.PropertyName == propertyName)
					fiered = true;
			};

			action();

			return fiered;
		}
	}
}