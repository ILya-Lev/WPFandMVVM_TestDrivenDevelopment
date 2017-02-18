using FriendStorage.UI.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FriendStorage.UI.Wrappers
{
	public abstract class NotifyDataErrorInfoBase : ViewModelBase, INotifyDataErrorInfo
	{
		protected readonly Dictionary<string, List<string>> _propertyErrors
			= new Dictionary<string, List<string>>();

		public IEnumerable GetErrors(string propertyName)
		{
			return propertyName != null && _propertyErrors.ContainsKey(propertyName)
				 ? _propertyErrors[propertyName]
				 : Enumerable.Empty<string>();
		}

		public bool HasErrors => _propertyErrors.Any();
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		public void OnErrorsChanged([CallerMemberName] string propertyName = null)
		{
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		protected void ClearErrors()
		{
			foreach(var propertyName in _propertyErrors.Keys.ToList())
			{
				_propertyErrors.Remove(propertyName);
				OnErrorsChanged(propertyName);
			}
		}
	}
}
