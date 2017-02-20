using System.ComponentModel;

namespace FriendStorage.UI.Wrappers.Base
{
	public interface IValidatableTrackingObject : IRevertibleChangeTracking, INotifyPropertyChanged
	{
		bool IsValid { get; }
	}
}