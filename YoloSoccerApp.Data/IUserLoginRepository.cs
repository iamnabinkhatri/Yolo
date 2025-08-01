using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public interface IUserLoginRepository
	{

		public Task<IEnumerable<UserLogin>> GetAllUserLoginAsync();
		//retreiving all the login details of specific user
		public Task<IEnumerable<UserLogin>> GetUserLoginByUserIdAsync(int userId);
		public Task<bool> AddNewUserLoginAsync(UserLogin ul);
		public Task<bool> UpdateUserLoginDetailsLogOutTime(int id, int userId);
	}
}

