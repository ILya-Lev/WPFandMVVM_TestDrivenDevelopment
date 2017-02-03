using FriendStorage.DataAccess;
using FriendStorage.Model;

namespace FriendStorage.UI.DataProvider
{
	public class FriendDataProvider : IFriendDataProvider
	{
		private readonly IDataService _dataService;

		public FriendDataProvider(IDataService dataService)
		{
			_dataService = dataService;
		}

		public Friend GetFriendById(int id) => _dataService.GetFriendById(id);

		public void SaveFriend(Friend friend) => _dataService.SaveFriend(friend);

		public void DeleteFriend(int id) => _dataService.DeleteFriend(id);
	}
}