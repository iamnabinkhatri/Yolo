
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using YoloSoccerApp.Logic;
using Microsoft.Extensions.Logging;

namespace YoloSoccerApp.Data
{
	public class UserRoleSqlRepository : IUserRoleRepository
	{
        //Fields
        private readonly string _connectionString;
        private readonly ILogger<UserRoleSqlRepository> _logger;

		public UserRoleSqlRepository(string connectionString, ILogger<UserRoleSqlRepository> logger)
		{
            this._connectionString = connectionString;
            this._logger = logger;
		}

        public async Task AddUserRole(UserRole userRole)
        {

            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"INSERT INTO [yolo].[userRole] (roleType) VALUES (@roleType);";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@roleType", userRole._roleType);
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            _logger.LogInformation($"User Role Added {userRole._roleType}");

        }

        public async Task<IEnumerable<UserRole>> GetAllUserRoles()
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string Queryable = @"SELECT * FROM [yolo].[userRole];";
            using SqlCommand cmd = new SqlCommand(Queryable, connection);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            List<UserRole> userRoles = new List<UserRole>();
            while (await reader.ReadAsync())
            {
                int Id = (int)reader["id"];
                string role = reader["roleType"].ToString() ?? "";
                userRoles.Add(new UserRole(Id, role));

            }
            await connection.CloseAsync();
            this._logger.LogInformation("Executed GellAllUserRoles");
            return userRoles;
        }

        public Task GetUserRole(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserRole(int id, string roleType)
        {
            throw new NotImplementedException();
        }
    }
}

