using System.ComponentModel;

namespace FriendStorage.UI.Wrappers
{
	public interface IValidatableTrackingObject : IRevertibleChangeTracking, INotifyPropertyChanged
	{
		bool IsValid { get; }
	}
}