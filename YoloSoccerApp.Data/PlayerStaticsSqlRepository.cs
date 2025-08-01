using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public class PlayerStaticsSqlRepository : IPlayerStaticsRepository
	{
        private readonly string _connectionString;
        private readonly ILogger<PlayerStaticsSqlRepository> _logger; 

		public PlayerStaticsSqlRepository(string connectionString, ILogger<PlayerStaticsSqlRepository> logger)
		{
            this._connectionString = connectionString;
            this._logger = logger;
		}

        public async Task<bool> AddPlayerStaticsAsync(PlayerStatics ps)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"INSERT INTO [yolo].[playerStatics] (playerId, goals, assists, save, attendance) VALUES
            (@playerId, @goals, @assists, @save, @attendance)
            ;";
            using SqlCommand cmd= new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@playerId",ps._player._id);
            cmd.Parameters.AddWithValue("@goals",ps._goals);
            cmd.Parameters.AddWithValue("@assists",ps._assists);
            cmd.Parameters.AddWithValue("@save",ps._save);
            cmd.Parameters.AddWithValue("@attendance",ps._attendance);
            int affectedRows=await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation($"Executed AddPlayerStaticsAsync function");
                return true;
            }
            else
            {
                _logger.LogInformation($"Failed to Add AddPlayerStaticsAsync function");
                return false;
            }
        }

        public async Task<bool> DeletePlayerStaticById(int playerId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"DELETE FROM [yolo].[playerStatics] WHERE playerId=@playerId;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@playerId", playerId);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation($"Deleted Player Statics successfully Executing DeletePlayerStatic Function");
                return true;
            }
            else
            {
                _logger.LogInformation($"Player cannot be Deleted executing function DeletePlayerStatic");
                return false;
            }

        }

        public async Task<IEnumerable<PlayerStatics>> GetAllPlayerStaticsAsync()
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[playerStatics];";
            using SqlCommand cmd = new SqlCommand(query, connection);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            List<PlayerStatics> ps = new List<PlayerStatics>();
            while (await reader.ReadAsync()) {
                int Id = (int)reader["id"];
                int playerId = (int)reader["playerId"];
                int goals = (int)reader["goals"];
                int assists = (int)reader["assists"];
                int save = (int)reader["save"];
                char attendance = reader["attendance"] != DBNull.Value && reader["attendance"].ToString().Length > 0
    ? reader["attendance"].ToString()[0]
    : 'N'; ;
                ps.Add(new PlayerStatics(Id, new Player(playerId), goals, assists, save, attendance));
            }
            await connection.CloseAsync();
            return ps.ToList();
        }

        public async Task<PlayerStatics> GetPlayerStaticById(int playerId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[playerStatics] WHERE playerId=@playerId;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@playerId", playerId);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            PlayerStatics pst = new PlayerStatics();
            while(await reader.ReadAsync())
            {
                int Id = (int)reader["Id"];
                int pId = (int)reader["playerId"];
                int goals = (int)reader["goals"];
                int assists = (int)reader["assists"];
                int save = (int)reader["save"];
                char attendance = reader["attendance"] != DBNull.Value && reader["attendance"].ToString().Length > 0
    ? reader["attendance"].ToString()[0]
    : 'N'; ;
                pst = new PlayerStatics(Id, new(pId), goals, assists, save, attendance);
            }
            await connection.CloseAsync();
            return pst;
        }

        public async Task UpdatePlayerStaticsAsync(int playerId, PlayerStatics ps)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"UPDATE [yolo].[playerStatics] SET goals=@goals, assists=@assists, save=@save, attendance=@attendance;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@goals", ps._goals);
            cmd.Parameters.AddWithValue("@assists", ps._assists);
            cmd.Parameters.AddWithValue("@save", ps._save);
            cmd.Parameters.AddWithValue("@attendance", ps._attendance);
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
    }
}

