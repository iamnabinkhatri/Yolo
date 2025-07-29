using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public class PollSqlRepository : IPollRepository
	{
        private readonly string _connectionString;
        private readonly ILogger<PollSqlRepository> _logger;

		public PollSqlRepository(string connectionString, ILogger<PollSqlRepository> logger)
		{
            this._connectionString = connectionString;
            this._logger = logger;
		}

        public async Task<int> AddPollAsync(Poll poll)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"INSERT INTO [yolo].[poll] (title, description, created_by, created_at, close_at,
            is_closed, latitude, longitude, is_shareable) VALUES (@title, @description, @created_by,
            @created_at, @close_at, @is_closed, @latitude, @longitude, @is_shareable);
            SELECT CAST(SCOPE_IDENTITY() AS int);";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@title", poll._title);
            cmd.Parameters.AddWithValue("@description", poll._description);
            cmd.Parameters.AddWithValue("@created_by", poll._created_by?._id);
            cmd.Parameters.AddWithValue("@created_at", poll._created_at);
            cmd.Parameters.AddWithValue("@close_at", poll._close_at);
            cmd.Parameters.AddWithValue("@is_closed", poll._is_closed);
            cmd.Parameters.AddWithValue("@latitude", poll._latitude);
            cmd.Parameters.AddWithValue("@longitude", poll._longitude);
            cmd.Parameters.AddWithValue("@is_shareable", poll._is_shareable);
            //int affectedRows = await cmd.ExecuteNonQueryAsync();
            //int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            object result = await cmd.ExecuteScalarAsync();
            Console.WriteLine($"New poll id is {Convert.ToInt32(result)}");
            await connection.CloseAsync();
            if (result is not null)
            {
                _logger.LogInformation($"Successfully added the Poll by {poll._created_by?._id} at {DateTime.UtcNow}");
                return Convert.ToInt32(result);
                
            }
            else
            {
                _logger.LogError($"Failed to Add the Poll at {DateTime.UtcNow}");
                return 0;
            }
            
        }

        public async Task<bool> DeletePollAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"DELETE FROM [yolo].[poll] WHERE id=@id;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id",id);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation($"Successfully Deleted the Poll {id} - {DateTime.UtcNow}");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to Delete the poll - {DateTime.UtcNow}");
                return false;
            }
        }

        public async Task<IEnumerable<Poll>> GetAllPollsAsync()
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[poll];";
            using SqlCommand cmd = new SqlCommand(query, connection);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            List<Poll> polls = new List<Poll>();
            while (await reader.ReadAsync())
            {
                int id = (int)reader["id"];
                string title = reader["title"].ToString() ?? "";
                string description = reader["description"].ToString() ?? "";
                int created_by = (int)reader["created_by"];
                DateTime created_at = reader["created_at"] != DBNull.Value ?
                    Convert.ToDateTime(reader["created_at"]) : DateTime.MinValue;
                DateTime close_at = reader["close_at"] != DBNull.Value ?
                    Convert.ToDateTime(reader["close_at"]) : DateTime.MinValue;
                char is_closed = 'N';
                if (reader["is_closed"] != DBNull.Value)
                {
                    string flag = reader["is_closed"].ToString();
                    if (!string.IsNullOrEmpty(flag))
                    {
                        is_closed = flag[0];
                    }
                }
                decimal latitude = reader["latitude"] != DBNull.Value
                ? Convert.ToDecimal(reader["latitude"]) : 0m;
                decimal longitude = reader["longitude"] != DBNull.Value
                ? Convert.ToDecimal(reader["latitude"]) : 0m;
                char is_shareable = 'N';
                if (reader["is_shareable"] != DBNull.Value)
                {
                    string flag = reader["is_shareable"].ToString();
                    if (!string.IsNullOrEmpty(flag))
                    {
                        is_shareable = flag[0];
                    }
                }

                polls.Add(new Poll(id,title,description,new Users(created_by),
                    created_at,close_at,is_closed,latitude,longitude,is_shareable));
            }
            await connection.CloseAsync();
            return polls;
        }

        public async Task<IEnumerable<Poll>> GetPollByUserIdAsync(int userId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[poll] WHERE created_by=@created_by;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@created_by", userId);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            List<Poll> polls = new List<Poll>();
            while (await reader.ReadAsync())
            {
                int id = (int)reader["id"];
                string title = reader["title"].ToString() ?? "";
                string description = reader["description"].ToString() ?? "";
                int created_by = (int)reader["created_by"];
                DateTime created_at = reader["created_at"] != DBNull.Value ?
                    Convert.ToDateTime(reader["created_at"]) : DateTime.MinValue;
                DateTime close_at = reader["close_at"] != DBNull.Value ?
                    Convert.ToDateTime(reader["close_at"]) : DateTime.MinValue;
                char is_closed = 'N';
                if (reader["is_closed"] != DBNull.Value)
                {
                    string flag = reader["is_closed"].ToString();
                    if (!string.IsNullOrEmpty(flag))
                    {
                        is_closed = flag[0];
                    }
                }
                decimal latitude = reader["latitude"] != DBNull.Value
                ? Convert.ToDecimal(reader["latitude"]) : 0m;
                decimal longitude = reader["longitude"] != DBNull.Value
                ? Convert.ToDecimal(reader["latitude"]) : 0m;
                char is_shareable = 'N';
                if (reader["is_shareable"] != DBNull.Value)
                {
                    string flag = reader["is_shareable"].ToString();
                    if (!string.IsNullOrEmpty(flag))
                    {
                        is_shareable = flag[0];
                    }
                }

                polls.Add(new Poll(id, title, description, new Users(created_by),
                    created_at, close_at, is_closed, latitude, longitude, is_shareable));
            } 
            await connection.CloseAsync();
            return polls;
            }

        public async Task<bool> UpdatePollAsync(int id, int created_by, Poll poll)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"UPDATE [yolo].[poll] SET title=@title, description=@description,
            created_at=@created_at, close_at=@close_at, is_closed=@is_closed, latitude=@latitude,
            is_shareable=@is_shareable WHERE id=@id AND created_by=@created_by;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@title", poll._title);
            cmd.Parameters.AddWithValue("@description", poll._description);
            cmd.Parameters.AddWithValue("@created_at", poll._created_at);
            cmd.Parameters.AddWithValue("@close_at", poll._close_at);
            cmd.Parameters.AddWithValue("@is_closed", poll._is_closed);
            cmd.Parameters.AddWithValue("@latitude", poll._latitude);
            cmd.Parameters.AddWithValue("@longitude", poll._longitude);
            cmd.Parameters.AddWithValue("@is_shareable", poll._is_shareable);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@created_by", created_by);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation($"Successfully Update the Poll by {created_by} at {DateTime.UtcNow}");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to Add the Poll at {DateTime.UtcNow}");
                return false;
            }
        }
    }
}

