
using System.Collections.Generic;
using System.Threading.Tasks;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public interface IUserRoleRepository
	{
		public Task<IEnumerable<UserRole>> GetAllUserRoles();
		public Task AddUserRole(UserRole userRole);
		public Task GetUserRole(int id);
		public Task UpdateUserRole(int id, string roleType);
	}
}

