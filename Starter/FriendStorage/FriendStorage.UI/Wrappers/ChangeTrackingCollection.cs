﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace FriendStorage.UI.Wrappers
{
	public class ChangeTrackingCollection<T> : ObservableCollection<T>, IRevertibleChangeTracking
		where T : IRevertibleChangeTracking, INotifyPropertyChanged
	{
		private IList<T> _originalItems;

		private ObservableCollection<T> _addedItems = new ObservableCollection<T>();
		private ObservableCollection<T> _removedItems = new ObservableCollection<T>();
		private ObservableCollection<T> _modifiedItems = new ObservableCollection<T>();

		public ChangeTrackingCollection(IEnumerable<T> items) : base(items)
		{
			_originalItems = this.ToList();
			AttachItemPropertyChangedHandler(_originalItems);

			AddedItems = new ReadOnlyObservableCollection<T>(_addedItems);
			RemovedItems = new ReadOnlyObservableCollection<T>(_removedItems);
			ModifiedItems = new ReadOnlyObservableCollection<T>(_modifiedItems);
		}

		public ReadOnlyObservableCollection<T> AddedItems { get; }
		public ReadOnlyObservableCollection<T> RemovedItems { get; }
		public ReadOnlyObservableCollection<T> ModifiedItems { get; }

		//public bool IsChanged => _originalItems.Any(t => t.IsChanged);
		public bool IsChanged => AddedItems.Any() || ModifiedItems.Any() || RemovedItems.Any();

		public void AcceptChanges()
		{
			_addedItems.Clear();
			_modifiedItems.Clear();
			_removedItems.Clear();

			//_originalItems.ForEach(t => t.AcceptChanges());
			foreach (var item in this)
			{
				item.AcceptChanges();
			}

			_originalItems = this.ToList();
			OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
		}

		public void RejectChanges()
		{
			for (var i = 0; i < _addedItems.Count; i++)
			{
				Remove(_addedItems[i]);
			}
			foreach (var removedItem in _removedItems)
			{
				Add(removedItem);
			}
			foreach (var modifiedItem in _modifiedItems)
			{
				modifiedItem.RejectChanges();
			}
			OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			var added = this.Except(_originalItems).ToList();
			var removed = _originalItems.Except(this).ToList();
			var modified = this.Except(added).Except(removed)
								.Where(current => current.IsChanged).ToList();

			AttachItemPropertyChangedHandler(added);
			DetachItemPropertyChangedHandler(removed);

			UpdateObservableCollection(_addedItems, added);
			UpdateObservableCollection(_removedItems, removed);
			UpdateObservableCollection(_modifiedItems, modified);

			base.OnCollectionChanged(e);
			OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
		}

		private void UpdateObservableCollection(ObservableCollection<T> collection, List<T> list)
		{
			collection.Clear();
			foreach (var item in list)
			{
				collection.Add(item);
			}
		}

		private void AttachItemPropertyChangedHandler(IList<T> originalItems)
		{
			foreach (var originalItem in originalItems)
			{
				originalItem.PropertyChanged += ItemPropertyChanged;
			}
		}

		private void DetachItemPropertyChangedHandler(IList<T> originalItems)
		{
			foreach (var originalItem in originalItems)
			{
				originalItem.PropertyChanged -= ItemPropertyChanged;
			}
		}

		private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var item = (T) sender;
			if (_addedItems.Contains(item)) return;
			if (item.IsChanged)
			{
				if (!_modifiedItems.Contains(item)) _modifiedItems.Add(item);
			}
			else
			{
				if (_modifiedItems.Contains(item)) _modifiedItems.Remove(item);
			}
			OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
		}
	}
}