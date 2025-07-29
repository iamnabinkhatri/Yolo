using System;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public interface IPlayerRoleRepository
	{

		public Task<IEnumerable<PlayerRole>> GetAllPlayerRoles();
		public Task<PlayerRole> GetPlayerRoleById(int roleId);
		public Task<bool> AddPlayerRole(PlayerRole pRole);
		public Task<bool> UpdatePlayerRole(int roleId, PlayerRole pRole);
		public Task<bool> DeletePlayerRole(int roleId);

	}
}

