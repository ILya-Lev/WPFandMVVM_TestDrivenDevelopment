using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FriendStorage.UI.Behaviors
{
	public static class DataGridChangedBehavior
	{
		public static DependencyProperty IsActiveProperty { get; }

		static DataGridChangedBehavior()
		{
			IsActiveProperty = DependencyProperty.Register("IsActive",
								typeof(bool), typeof(DataGridChangedBehavior),
								new PropertyMetadata(default(bool), OnIsActivePropertyChanged));
		}

		public static bool GetIsActive(DependencyObject obj)
		{
			return (bool) obj.GetValue(IsActiveProperty);
		}

		public static void SetIsActive(DependencyObject obj, bool value)
		{
			obj.SetValue(IsActiveProperty, value);
		}
		private static void OnIsActivePropertyChanged(DependencyObject d,
													DependencyPropertyChangedEventArgs e)
		{
			var dataGrid = d as DataGrid;
			if (dataGrid == null) return;

			if ((bool) e.NewValue) dataGrid.Loaded += DataGrid_Loaded;
			else dataGrid.Loaded -= DataGrid_Loaded;
		}

		private static void DataGrid_Loaded(object sender, RoutedEventArgs e)
		{
			var dataGrid = sender as DataGrid;
			foreach (var textColumn in dataGrid.Columns.OfType<DataGridTextColumn>())
			{
				var binding = textColumn.Binding as Binding;
				if (binding == null) continue;

				textColumn.EditingElementStyle = CreateEditingElementStyle(dataGrid, binding.Path.Path);
				textColumn.ElementStyle = CreateElementStyle(dataGrid, binding.Path.Path);
			}
		}

		private static Style CreateElementStyle(DataGrid dataGrid, string path)
		{
			var baseStyle = dataGrid.FindResource("TextBlockBaseStyle") as Style;
			var style = new Style(typeof(TextBox), baseStyle);
			AddSetters(style, path, dataGrid);
			return style;
		}

		private static Style CreateEditingElementStyle(DataGrid dataGrid, string path)
		{
			var baseStyle = dataGrid.FindResource(typeof(TextBox)) as Style;
			var style = new Style(typeof(TextBox), baseStyle);
			AddSetters(style, path, dataGrid);
			return style;
		}

		private static void AddSetters(Style style, string path, DataGrid dataGrid)
		{
			style.Setters.Add(new Setter(ChangedBehavior.IsActiveProperty, false));
			style.Setters.Add(new Setter(ChangedBehavior.IsChangedProperty,
								new Binding($"{path}IsChanged")));
			style.Setters.Add(new Setter(ChangedBehavior.OriginalValueProperty,
								new Binding($"{path}OriginalValue")));
			style.Setters.Add(new Setter(Validation.ErrorTemplateProperty,
								dataGrid.FindResource("ErrorInsideErrorTemplate")));
		}
	}
}