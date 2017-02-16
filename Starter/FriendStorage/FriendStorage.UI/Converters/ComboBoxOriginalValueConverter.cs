using FriendStorage.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace FriendStorage.UI.Converters
{
	public class ComboBoxOriginalValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var id = (int) value;
			var comboBox = parameter as ComboBox;
			if (comboBox?.ItemsSource == null) return value;

			return comboBox.ItemsSource.OfType<LookupItem>()
							.SingleOrDefault(item => item.Id == id)
							?.DisplayMember ?? value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}