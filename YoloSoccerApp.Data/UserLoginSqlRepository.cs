using System;
using System.Data.SqlTypes;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public class UserLoginSqlRepository : IUserLoginRepository
	{
        private readonly string _connectionString;
        private readonly ILogger<UserLoginSqlRepository> _logger;

		public UserLoginSqlRepository(string connectionString, ILogger<UserLoginSqlRepository> logger)
		{
            this._connectionString = connectionString;
            this._logger = logger;
		}

        public async Task<bool> AddNewUserLoginAsync(UserLogin ul)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"INSERT INTO [yolo].[user_login] (userId, is_loggedIn, login_started_at)
            VALUES (@userId, @isLoggedIn, @loginTime);";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@userId", ul._userId?._id);
            cmd.Parameters.AddWithValue("@isLoggedIn", ul._isLoggedIn);
            cmd.Parameters.AddWithValue("@loginTime", ul._loginStartedAt);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation("Successfully Executed adding UserLogin details");
                return true;
            }
            else
            {
                _logger.LogError("Failed to Execute adding UserLogin details");
                return false;
            }
        }

        public async Task<IEnumerable<UserLogin>> GetAllUserLoginAsync()
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[user_login];";
            using SqlCommand cmd = new SqlCommand(query, connection);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            List<UserLogin> ul = new List<UserLogin>();
            while (await reader.ReadAsync())
            {
                int id = (int)reader["id"];
                int userId = (int)reader["userId"];
                char isLoggedIn = 'N';
                if (reader["is_loggedIn"] != DBNull.Value)
                {
                    string flag = reader["is_loggedIn"].ToString();
                    if (!string.IsNullOrEmpty(flag))
                    {
                        isLoggedIn = flag[0];
                    }
                }
                DateTime loginTime = reader["login_started_at"] != DBNull.Value ? Convert.ToDateTime(reader["login_started_at"]): DateTime.MinValue;
                DateTime logoutTime = reader["login_ends_at"] !=DBNull.Value ? Convert.ToDateTime(reader["login_ends_at"]): DateTime.MinValue;
                ul.Add(new UserLogin(id, new Users(userId), isLoggedIn, loginTime, logoutTime));
                _logger.LogInformation("All the user Login Details retrieved successfully");
            }
            await connection.CloseAsync();
            return ul;
        }

        public async Task<IEnumerable<UserLogin>> GetUserLoginByUserIdAsync(int userId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[user_login] WHERE userId=@userId;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@userId",userId);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            List<UserLogin> ulbyId = new List<UserLogin>();
            while (await reader.ReadAsync())
            {
                int id = (int)reader["id"];
                int uId = (int)reader["userId"];
                char isLoggedIn = 'N';
                if (reader["is_loggedIn"] != DBNull.Value)
                {
                    string flag = reader["is_loggedIn"].ToString();
                    if (!string.IsNullOrEmpty(flag))
                    {
                        isLoggedIn = flag[0];
                    }
                }
                DateTime loginTime = reader["login_started_at"] != DBNull.Value ? Convert.ToDateTime(reader["login_started_at"]) : DateTime.MinValue;
                DateTime logoutTime = reader["login_ends_at"] != DBNull.Value ? Convert.ToDateTime(reader["login_ends_at"]) : DateTime.MinValue;
                ulbyId.Add(new UserLogin(id, new Users(uId), isLoggedIn, loginTime, logoutTime));
                _logger.LogInformation($"All the user Login of user {uId} Details retrieved successfully");
            }
            await connection.CloseAsync();
            return ulbyId;
        }

        public async Task<bool> UpdateUserLoginDetailsLogOutTime(int id, int userId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"UPDATE [yolo].[user_login] SET login_ends_at=@logoutTime
            WHERE id=@id AND userId=@userId;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@logoutTime",DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@id",id);
            cmd.Parameters.AddWithValue("@userId", userId);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation($"Successfully Updated the logout time of UserId: {userId}");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to update the logout time for UserId: {userId}");
                return false;
            }
        }
    }
}

