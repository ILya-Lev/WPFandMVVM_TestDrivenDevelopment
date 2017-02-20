using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FriendStorage.UI.Wrappers.Base
{
	public class ModelWrapper<TModel> : NotifyDataErrorInfoBase, IValidatableTrackingObject, IValidatableObject
	{
		private readonly Dictionary<string, object> _originalValues = new Dictionary<string, object>();
		private List<IValidatableTrackingObject> _trackingObjects
			= new List<IValidatableTrackingObject>();

		public TModel Model { get; }

		protected ModelWrapper(TModel model)
		{
			if(model == null) throw new ArgumentNullException(nameof(model));
			Model = model;

			InitializeComplexProperty(model);
			InitializeCollectionProperty(model);

			Validate();
		}

		protected virtual void InitializeCollectionProperty(TModel model)
		{
		}

		protected virtual void InitializeComplexProperty(TModel friend)
		{
		}

		public bool IsChanged => _originalValues.Any() || _trackingObjects.Any(t => t.IsChanged);
		public bool IsValid => !HasErrors && _trackingObjects.All(t => t.IsValid);

		public void RejectChanges()
		{
			foreach(var pair in _originalValues)
			{
				typeof(TModel).GetProperty(pair.Key).SetValue(Model, pair.Value);
			}
			_trackingObjects.ForEach(t => t.RejectChanges());

			_originalValues.Clear();
			Validate();
			OnPropertyChanged("");
		}

		public void AcceptChanges()
		{
			_originalValues.Clear();
			_trackingObjects.ForEach(t => t.AcceptChanges());
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
				Validate();
				OnPropertyChanged(propertyName);
				OnPropertyChanged(propertyName + "IsChanged");
			}
		}

		private void Validate()
		{
			ClearErrors();
			var results = new List<ValidationResult>();
			var context = new ValidationContext(this);
			Validator.TryValidateObject(this, context, results, true);

			if(results.Any()) //will it always match try validate property
			{
				var propertyNames = results.SelectMany(r => r.MemberNames).Distinct().ToList();
				foreach(var propertyName in propertyNames)
				{
					_propertyErrors[propertyName] = results
						.Where(r => r.MemberNames.Contains(propertyName))
						.Select(r => r.ErrorMessage).Distinct().ToList();
					OnErrorsChanged(propertyName);
				}
			}
			OnPropertyChanged(nameof(IsValid));
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

		protected void RegisterCollection<TWrapper, TM>(
													ChangeTrackingCollection<TWrapper> wrapperCollection,
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
				Validate();
			};
			RegisterTrackingObject(wrapperCollection);
		}

		protected void RegisterComplex<TM>(ModelWrapper<TM> complexProperty)
		{
			RegisterTrackingObject(complexProperty);
		}

		private void RegisterTrackingObject(IValidatableTrackingObject complexProperty)
		{
			if(!_trackingObjects.Contains(complexProperty))
			{
				_trackingObjects.Add(complexProperty);
				complexProperty.PropertyChanged += TrackingObjectPropertyChanged;
			}
		}

		private void TrackingObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if(e.PropertyName == nameof(IsChanged))
				OnPropertyChanged(nameof(IsChanged));
			else if(e.PropertyName == nameof(IsValid))
				OnPropertyChanged(nameof(IsValid));
		}

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			yield break;
		}
	}
}