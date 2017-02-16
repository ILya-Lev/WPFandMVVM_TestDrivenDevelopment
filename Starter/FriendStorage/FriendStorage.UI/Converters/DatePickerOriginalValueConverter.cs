using System;
using System.Globalization;
using System.Windows.Data;

namespace FriendStorage.UI.Converters
{
	public class DatePickerOriginalValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((DateTime?) value)?.ToString("D", CultureInfo.InvariantCulture)
					?? "No value has been specified";
		}

		/// <summary> implement it for from UI to model information transferring </summary>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}