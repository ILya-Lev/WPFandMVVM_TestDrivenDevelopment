using FriendStorage.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FriendStorage.UI.Wrappers
{
	public class ModelWrapper<TModel> : ViewModelBase
	{
		private bool _isChanged;

		public TModel Model { get; }

		public bool IsChanged
		{
			get { return _isChanged; }
			protected set { _isChanged = value; OnPropertyChanged(); }
		}

		public ModelWrapper(TModel model)
		{
			if(model == null) throw new ArgumentNullException(nameof(model));
			Model = model;
		}

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if(propertyName != nameof(IsChanged))
				IsChanged = true;
		}

		protected void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = null)
		{
			var propertyInfo = Model.GetType().GetProperty(propertyName);
			var oldValue = propertyInfo.GetValue(Model);
			if(!Equals(oldValue, value))
			{
				propertyInfo.SetValue(Model, value);
				OnPropertyChanged(propertyName);
			}
		}

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