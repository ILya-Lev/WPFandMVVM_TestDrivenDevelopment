using FriendStorage.DataAccess;
using FriendStorage.Model;
using System;
using System.Collections.Generic;

namespace FriendStorage.UI.DataProvider
{
	public class NavigationDataProvider : INavigationDataProvider
	{
		private readonly Func<IDataService> _dataServiceCreator;

		public NavigationDataProvider(Func<IDataService> dataServiceCreator)
		{
			_dataServiceCreator = dataServiceCreator;
		}

		public IEnumerable<Friend> GetAllFriends()
		{
			using (var dataService = _dataServiceCreator())
			{
				return dataService.GetAllFriends();
			}
		}
	}
}