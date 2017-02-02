using Prism.Events;

namespace FriendStorage.UI.Events
{
	/// <summary>
	/// inherit PubSubEvent to utilize Prism event aggregator
	/// type parameter int stands for Friend.Id
	/// </summary>
	public class OpenFriendEditViewEvent : PubSubEvent<int>
	{
	}
}