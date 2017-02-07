using System;
using System.ComponentModel;

namespace FriendStorage.UIxUnitTests.Extensions
{
	public static class NotifyPropertyChangedExtensions
	{
		public static bool IsPropertyChangedFired(this INotifyPropertyChanged sender,
													Action action,
													string propertyName)
		{
			var fired = false;
			sender.PropertyChanged += (s, args) =>
			{
				if (args.PropertyName == propertyName)
					fired = true;
			};

			action();

			return fired;
		}
	}
}