using System;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public interface IUserRepository
	{
        public Task<IEnumerable<Users>> GetAllUsersAsync();
        public Task AddUserAsync(Users user);
        public Task UpdateUserAsync(string username, Users user);
        public Task<bool> ValidatePassword(string username, string password);
        public Task<bool> CheckUserExists(string username);
      
    }
}

