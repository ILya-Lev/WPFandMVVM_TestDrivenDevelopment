using FriendStorage.UI.ViewModel;
using System;
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
			if (model == null) throw new ArgumentNullException(nameof(model));
			Model = model;
		}

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName != nameof(IsChanged))
				IsChanged = true;
		}

		protected void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = null)
		{
			var propertyInfo = Model.GetType().GetProperty(propertyName);
			var oldValue = propertyInfo.GetValue(Model);
			if (!Equals(oldValue, value))
			{
				propertyInfo.SetValue(Model, value);
				OnPropertyChanged(propertyName);
			}
		}
	}
}