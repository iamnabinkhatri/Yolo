using System;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public interface IPlayerRepository
	{
        public Task<IEnumerable<Player>> GetAllPlayerAsync();
        public Task AddPlayerAsync(Player player);
        public Task UpdatePlayerAsync(int userId, Player player);
        public Task<bool> CheckPlayerExistsAsync(int userId);
        public Task<bool> DeletePlayerAsync(int userId);
    }
}