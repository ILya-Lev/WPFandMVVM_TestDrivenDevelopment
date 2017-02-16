using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace FriendStorage.UI.Behaviors
{
	/// <summary>
	/// class to wrap up attached properties behavior
	/// </summary>
	public static class ChangedBehavior
	{
		public static DependencyProperty OriginalValueProperty { get; }
		public static DependencyProperty IsChangedProperty { get; }
		public static DependencyProperty IsActiveProperty { get; }
		public static DependencyProperty OriginalValueConverterProperty { get; }

		private static readonly Dictionary<Type, DependencyProperty> _defaultProperties;

		static ChangedBehavior()
		{
			OriginalValueProperty = DependencyProperty.RegisterAttached("OriginalValue",
										typeof(object), typeof(ChangedBehavior),
										new PropertyMetadata(null));
			IsChangedProperty = DependencyProperty.RegisterAttached(name: "IsChanged",
								propertyType: typeof(bool), ownerType: typeof(ChangedBehavior),
								defaultMetadata: new PropertyMetadata(false));
			IsActiveProperty = DependencyProperty.RegisterAttached(name: "IsActive",
								propertyType: typeof(bool), ownerType: typeof(ChangedBehavior),
								defaultMetadata: new PropertyMetadata(false, OnIsActivePropertyChanged));

			OriginalValueConverterProperty = DependencyProperty.RegisterAttached("OriginalValueConverter",
									typeof(IValueConverter), typeof(ChangedBehavior),
									new PropertyMetadata(null, OnOriginalValueConverterPropertyChanged));

			_defaultProperties = new Dictionary<Type, DependencyProperty>
			{
				[typeof(TextBox)] = TextBox.TextProperty,
				[typeof(CheckBox)] = ToggleButton.IsCheckedProperty,
				[typeof(DatePicker)] = DatePicker.SelectedDateProperty,
				[typeof(ComboBox)] = Selector.SelectedValueProperty,
			};
		}

		public static object GetOriginalValue(DependencyObject dependencyObject)
		{
			return (object) dependencyObject.GetValue(OriginalValueProperty);
		}

		public static void SetOriginalValue(DependencyObject dependencyObject, object value)
		{
			dependencyObject.SetValue(OriginalValueProperty, value);
		}

		public static bool GetIsChanged(DependencyObject dependencyObject)
		{
			return (bool) dependencyObject.GetValue(IsChangedProperty);
		}

		public static void SetIsChanged(DependencyObject dependencyObject, bool value)
		{
			dependencyObject.SetValue(IsChangedProperty, value);
		}

		public static bool GetIsActive(DependencyObject dependencyObject)
		{
			return (bool) dependencyObject.GetValue(IsActiveProperty);
		}

		public static void SetIsActive(DependencyObject dependencyObject, bool value)
		{
			dependencyObject.SetValue(IsActiveProperty, value);
		}

		public static IValueConverter GetOriginalValueConverter(DependencyObject dependencyObject)
		{
			return (IValueConverter) dependencyObject.GetValue(OriginalValueConverterProperty);
		}

		public static void SetOriginalValueConverter(DependencyObject dependencyObject,
													IValueConverter value)
		{
			dependencyObject.SetValue(OriginalValueConverterProperty, value);
		}

		private static void OnIsActivePropertyChanged(DependencyObject d,
														DependencyPropertyChangedEventArgs e)
		{
			if (!_defaultProperties.ContainsKey(d.GetType()))
				return;

			if (!(bool) e.NewValue)
			{
				BindingOperations.ClearBinding(d, IsChangedProperty);
				BindingOperations.ClearBinding(d, OriginalValueProperty);
				return;
			}

			var defaultProperty = _defaultProperties[d.GetType()];
			var binding = BindingOperations.GetBinding(d, defaultProperty);
			if (binding == null) return;

			var path = binding.Path.Path;
			BindingOperations.SetBinding(d, IsChangedProperty, new Binding($"{path}IsChanged"));
			CreateOriginalValueBinding(d, $"{path}OriginalValue");
		}

		private static void OnOriginalValueConverterPropertyChanged(DependencyObject d,
													DependencyPropertyChangedEventArgs e)
		{
			var originalBinding = BindingOperations.GetBinding(d, OriginalValueProperty);
			if (originalBinding == null) return;

			CreateOriginalValueBinding(d, originalBinding.Path.Path);
		}

		private static void CreateOriginalValueBinding(DependencyObject d, string path)
		{
			var newBinding = new Binding(path)
			{
				Converter = GetOriginalValueConverter(d),
				ConverterParameter = d
			};
			BindingOperations.SetBinding(d, OriginalValueProperty, newBinding);
		}
	}
}
