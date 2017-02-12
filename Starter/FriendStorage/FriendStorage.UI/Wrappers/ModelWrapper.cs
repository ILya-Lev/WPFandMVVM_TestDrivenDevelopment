using FriendStorage.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FriendStorage.UI.Wrappers
{
	public class ModelWrapper<TModel> : ViewModelBase, IRevertibleChangeTracking
	{
		protected readonly Dictionary<string, object> _originalValues = new Dictionary<string, object>();

		public TModel Model { get; }
		public ModelWrapper(TModel model)
		{
			if(model == null) throw new ArgumentNullException(nameof(model));
			Model = model;
		}

		public bool IsChanged => _originalValues.Any();

		public void RejectChanges()
		{
			foreach(var pair in _originalValues)
			{
				typeof(TModel).GetProperty(pair.Key).SetValue(Model, pair.Value);
			}
			AcceptChanges();
		}

		public void AcceptChanges()
		{
			_originalValues.Clear();
			//WPF specific trick - forces the framework to call get over all object's properties
			//all the data binding will update reread the values
			OnPropertyChanged("");
		}

		protected void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = null)
		{
			var propertyInfo = Model.GetType().GetProperty(propertyName);
			var oldValue = propertyInfo.GetValue(Model);
			if(!Equals(oldValue, value))
			{
				UpdateOriginalValue(oldValue, value, propertyName);
				propertyInfo.SetValue(Model, value);
				OnPropertyChanged(propertyName);
				OnPropertyChanged(propertyName + "IsChanged");
			}
		}

		private void UpdateOriginalValue<TValue>(TValue oldValue, TValue value, string propertyName)
		{
			if(!_originalValues.ContainsKey(propertyName))
			{
				_originalValues.Add(propertyName, oldValue);
				OnPropertyChanged(nameof(IsChanged));
			}
			else if(Equals(_originalValues[propertyName], value))
			{
				_originalValues.Remove(propertyName);
				OnPropertyChanged(nameof(IsChanged));
			}
		}

		protected TValue GetOriginalValue<TValue>(string propertyName)
		{
			return _originalValues.ContainsKey(propertyName)
				? (TValue) _originalValues[propertyName]
				: GetValue<TValue>(propertyName);
		}

		protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
		{
			var propertyInfo = Model.GetType().GetProperty(propertyName);
			return (TValue) propertyInfo.GetValue(Model);
		}

		protected bool GetIsChanged(string propertyName) => _originalValues.ContainsKey(propertyName);

		protected static void RegisterCollection<TWrapper, TM>(
													ObservableCollection<TWrapper> wrapperCollection,
													List<TM> modelCollection)
			where TWrapper : ModelWrapper<TM>
		{
			wrapperCollection.CollectionChanged += (sender, e) =>
			{
				if(e.Action == NotifyCollectionChangedAction.Add)
					modelCollection.AddRange(e.NewItems.Cast<TWrapper>().Select(ew => ew.Model));
				else if(e.Action == NotifyCollectionChangedAction.Remove)
				{
					foreach(var email in e.OldItems.Cast<TWrapper>().Select(ew => ew.Model))
						modelCollection.Remove(email);
				}
				else if(e.Action == NotifyCollectionChangedAction.Reset)
					modelCollection.Clear();
			};
		}
	}
}