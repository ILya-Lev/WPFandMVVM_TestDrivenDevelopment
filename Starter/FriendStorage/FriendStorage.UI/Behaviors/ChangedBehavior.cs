using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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

			_defaultProperties = new Dictionary<Type, DependencyProperty>
			{
				[typeof(TextBox)] = TextBox.TextProperty
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

		private static void OnIsActivePropertyChanged(DependencyObject dependencyObject,
														DependencyPropertyChangedEventArgs e)
		{
			if(_defaultProperties.ContainsKey(dependencyObject.GetType()))
			{
				var defaultProperty = _defaultProperties[dependencyObject.GetType()];
				if((bool) e.NewValue)
				{
					var binding = BindingOperations.GetBinding(dependencyObject, defaultProperty);
					if(binding == null) return;

					var bindingPath = binding.Path.Path;
					BindingOperations.SetBinding(dependencyObject, IsChangedProperty,
						new Binding($"{bindingPath}IsChanged"));
					BindingOperations.SetBinding(dependencyObject, OriginalValueProperty,
						new Binding($"{bindingPath}OriginalValue"));
				}
				else
				{
					BindingOperations.ClearBinding(dependencyObject, IsChangedProperty);
					BindingOperations.ClearBinding(dependencyObject, OriginalValueProperty);
				}
			}
		}
	}
}
