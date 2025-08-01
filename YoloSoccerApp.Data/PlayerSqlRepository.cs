using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public class PlayerSqlRepository : IPlayerRepository
	{
        private readonly string _connectionString;
        private readonly ILogger<PlayerSqlRepository> _logger;


		public PlayerSqlRepository(string connectionString, ILogger<PlayerSqlRepository> logger)
		{
            this._logger = logger;
            this._connectionString = connectionString;
		}

        public async Task AddPlayerAsync(Player player)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            Console.WriteLine($"userId: {player._userId._id}");
            string query = @"INSERT INTO [yolo].[player] (userId, nickname, playerNumber, playerRoleId)
            VALUES (@userId, @nickname, @playerNumber, @playerRoleId);";
            using SqlCommand cmd = new SqlCommand(query,connection);
            cmd.Parameters.AddWithValue("@userId", player._userId._id);
            cmd.Parameters.AddWithValue("@nickname", player._nickname);
            cmd.Parameters.AddWithValue("@playerNumber", player._playerNumber);
            cmd.Parameters.AddWithValue("@playerRoleId", player._playerRole?._id);
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            _logger.LogInformation("Executed AddPlayerAsync");
        }

        public async Task<bool> CheckPlayerExistsAsync(int userId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [book].[player] WHERE userId=@userId;";
            using SqlCommand cmd = new SqlCommand(query,connection);
            cmd.Parameters.AddWithValue("@userId", userId);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            bool result = false;
            if (reader.HasRows)
            {
                result = true;
            }
            await connection.CloseAsync(); 
            return result;
        }

        public async Task<bool> DeletePlayerAsync(int userId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"DELETE FROM [yolo].[player] WHERE userId=@userId;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@userId", userId);
            int rowAffected = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            return rowAffected>0;
        }

        public async Task<IEnumerable<Player>> GetAllPlayerAsync()
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[player];";
            using SqlCommand cmd = new SqlCommand(query, connection);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            List<Player> players = new List<Player>();
            while (await reader.ReadAsync())
            {
                int Id = (int)reader["id"];
                int userId = (int)reader["userId"];
                Users user = new Users(userId);
                string nickname = reader["nickname"].ToString() ?? "";
                int playerNumber = (int)reader["playerNumber"];
                int pRoleId = (int)reader["playerRoleId"];
                PlayerRole pRole = new PlayerRole(pRoleId);
                players.Add(new Player(Id, user, nickname, playerNumber, pRole));
            }
            await connection.CloseAsync();
            return players;
        }

        public async Task UpdatePlayerAsync(int userId, Player player)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"UPDATE [yolo].[player]
            SET nickname=@nickname, playerNumber=@playerNumber WHERE userId=@condition;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            if (player._nickname != null)
            {
                cmd.Parameters.AddWithValue("@nickname", player._nickname);
            }
            if (player._playerNumber > 0)
            {
                cmd.Parameters.AddWithValue("@playerNumber", player._playerNumber);
            }
            
            cmd.Parameters.AddWithValue("@condition", userId);
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
    }
}

