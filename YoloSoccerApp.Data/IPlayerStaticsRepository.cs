using System;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public interface IPlayerStaticsRepository
	{

		public Task<IEnumerable<PlayerStatics>> GetAllPlayerStaticsAsync();
		public Task<bool> AddPlayerStaticsAsync(PlayerStatics playerStatics);
		public Task UpdatePlayerStaticsAsync(int playerId, PlayerStatics playerStatics); //playerId and userId are different
		public Task<PlayerStatics> GetPlayerStaticById(int playerId);
		public Task<bool> DeletePlayerStaticById(int playerId);
	}
}