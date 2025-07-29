using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public class PlayerRoleSqlRepository : IPlayerRoleRepository
	{
        //fields or property
        private readonly string? _connectionString;
        private readonly ILogger<PlayerRoleSqlRepository>? _logger;

        //Parameterized constructor
        public PlayerRoleSqlRepository(string connectionString, ILogger<PlayerRoleSqlRepository> logger)
		{
            this._connectionString = connectionString;
            this._logger = logger;
		}

        public  async Task<bool> AddPlayerRole(PlayerRole pRole)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"INSERT INTO [yolo].[playerRole] (playerRole) VALUES (@playerRole);";
            using SqlCommand cmd = new SqlCommand(query,connection);
            cmd.Parameters.AddWithValue("@playerRole",pRole._playerRole);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation("Add Player Role successfully");
                return true;
            }
            else
            {
                _logger.LogInformation("Failed Add Player Role");
                return false;
            }
        }

        public async Task<bool> DeletePlayerRole(int roleId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"DELETE FROM [yolo].[playerRole] WHERE id=@roleId;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@roleId", roleId);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation($"Successfully Deleted Player Role {roleId}");
                return true;
            }
            else
            {
                _logger.LogInformation("Failed to Delete Player Role");
                return false;
            }
        }

        public async Task<IEnumerable<PlayerRole>> GetAllPlayerRoles()
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[playerRole];";
            using SqlCommand cmd = new SqlCommand(query, connection);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            List<PlayerRole> pRoles = new List<PlayerRole>();
            while (await reader.ReadAsync())
            {
                int id = (int)reader["id"];
                string playerRole = reader["playerRole"].ToString() ?? "";
                pRoles.Add(new PlayerRole(id, playerRole));
            }
            await connection.CloseAsync();
            _logger.LogInformation("Retrieved all the Player Role");
            return pRoles;
        }

        public async Task<PlayerRole> GetPlayerRoleById(int roleId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[playerRole] WHERE id=@roleId;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@roleId", roleId);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            PlayerRole pRole = new PlayerRole();
            while (await reader.ReadAsync())
            {
                int id = (int)reader["id"];
                string playerRole = reader["playerRole"].ToString() ?? "";
                pRole = new PlayerRole(id, playerRole);
            }
            await connection.CloseAsync();
            _logger.LogInformation($"Retrieved the Player Role {roleId}");
            return pRole;
        }

        public async Task<bool> UpdatePlayerRole(int roleId, PlayerRole pRole)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"UPDATE [yolo].[playerRole] SET playerRole=@playerRole WHERE id=@roleId;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@playerRole",pRole._playerRole);
            cmd.Parameters.AddWithValue("@roleId", roleId);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if(affectedRows > 0)
            {
                _logger.LogInformation($"Successfully Updated the Player Role {roleId}");
                return true;
            }
            else
            {
                _logger.LogInformation($"Failed to Update the Player Role {roleId}");
                return false;
            }
        }
    }
}

