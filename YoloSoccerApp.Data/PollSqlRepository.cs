using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
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

        public async Task<IEnumerable<Poll>> GetPollByPollIdAsync(int pollId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[poll] WHERE id=@pollId;";
            await using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@pollId", pollId);
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
                DateTime start_at = reader["start_at"] != DBNull.Value? Convert.ToDateTime(reader["start_at"]) : DateTime.MinValue;
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
                int communityId = reader["communityId"] != DBNull.Value ? Convert.ToInt32(reader["communityId"]) : 0;
                Community community = new Community(communityId);
                

                polls.Add(new Poll(id, title, description, new Users(created_by),
                    created_at, start_at, close_at, is_closed, latitude, longitude, is_shareable, community));
            } 
            await connection.CloseAsync();
            return polls;
        }

        public async Task<int> AddPollAsync(Poll poll)
        {
            DateTime created_at = poll._created_at;
            DateTime start_at = poll._start_at;
            DateTime close_at = poll._close_at;
            // Ensure within SQL Server datetime range
            if (created_at < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                created_at = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

            if (created_at > (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue)
                created_at = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;
            
            if (start_at < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                start_at = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

            if (start_at > (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue)
                start_at = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;
            
            if (close_at < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                close_at = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

            if (close_at > (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue)
                close_at = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;
            
            Console.WriteLine("adding poll" + poll._created_by._id + "sqlddate: "+created_at + " start date: " + start_at + " end date: "+ close_at);
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"INSERT INTO [yolo].[poll] (title, description, created_by, created_at, start_at, close_at,
            is_closed, latitude, longitude, is_shareable, communityId) VALUES (@title, @description, @created_by,
            @created_at, @start_at, @close_at, @is_closed, @latitude, @longitude, @is_shareable, @communityId);
            SELECT CAST(SCOPE_IDENTITY() AS int);";
            using SqlCommand cmd = new SqlCommand(query, connection);
            
            cmd.Parameters.AddWithValue("@title", poll._title);
            cmd.Parameters.AddWithValue("@description", poll._description);
            cmd.Parameters.AddWithValue("@created_by", poll._created_by?._id);
            cmd.Parameters.AddWithValue("@created_at", SqlDbType.DateTime).Value = created_at;
            cmd.Parameters.AddWithValue("@start_at", SqlDbType.DateTime).Value = start_at;
            cmd.Parameters.AddWithValue("@close_at", SqlDbType.DateTime).Value = close_at;
            cmd.Parameters.AddWithValue("@is_closed", poll._is_closed);
            cmd.Parameters.AddWithValue("@latitude", poll._latitude);
            cmd.Parameters.AddWithValue("@longitude", poll._longitude);
            cmd.Parameters.AddWithValue("@is_shareable", poll._is_shareable);
            cmd.Parameters.AddWithValue("@ communityId", poll._community._id);
            //int affectedRows = await cmd.ExecuteNonQueryAsync();
            //int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            object? result = await cmd.ExecuteScalarAsync();
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
                DateTime start_at = reader["start_at"] != DBNull.Value ?
                    Convert.ToDateTime(reader["start_at"]) : DateTime.MinValue;
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
                int  communityId = reader["community_id"] != DBNull.Value ?
                    Convert.ToInt32(reader["community_id"]) : 0;
                Community community = new Community(communityId);
                

                polls.Add(new Poll(id,title,description,new Users(created_by),
                    created_at, start_at, close_at,is_closed,latitude,longitude,is_shareable,community));
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
                DateTime start_at = reader["start_at"] != DBNull.Value ?
                    Convert.ToDateTime(reader["start_at"]) : DateTime.MinValue;
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
                int   communityId = reader["community_id"] != DBNull.Value ?   
                    Convert.ToInt32(reader["community_id"]) : 0;
                Community community = new Community(communityId);

                polls.Add(new Poll(id, title, description, new Users(created_by),
                    created_at, start_at, close_at, is_closed, latitude, longitude, is_shareable, community));
            } 
            await connection.CloseAsync();
            return polls;
            }

        public async Task<bool> UpdatePollAsync(int id, int created_by, Poll poll)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            var updates = new List<string>();
            var parameters = new List<SqlParameter>();

            if (poll._title != null)
            {
                updates.Add("title = @title");
                parameters.Add(new SqlParameter("@title", poll._title));
            }
            if (poll._description != null)
            {
                updates.Add("description = @description");
                parameters.Add(new SqlParameter("@description", poll._description));
            }
            if (poll._created_at != null)
            {
                updates.Add("created_at = @created_at");
                parameters.Add(new SqlParameter("@created_at", poll._created_at));
            }
            if (poll._start_at != null)
            {
                updates.Add("start_at = @start_at");
                parameters.Add(new SqlParameter("@start_at", poll._created_at));
            }
            if (poll._close_at != null)
            {
                updates.Add("close_at = @close_at");
                parameters.Add(new SqlParameter("@close_at", poll._close_at));
            }
            if (poll._is_closed != null)
            {
                updates.Add("is_closed = @is_closed");
                parameters.Add(new SqlParameter("@is_closed", poll._is_closed));
            }
            if (poll._latitude != null)
            {
                updates.Add("latitude = @latitude");
                parameters.Add(new SqlParameter("@latitude", poll._latitude));
            }
            if (poll._longitude != null)
            {
                updates.Add("longitude = @longitude");
                parameters.Add(new SqlParameter("@longitude", poll._longitude));
            }
            if (poll._is_shareable != null)
            {
                updates.Add("is_shareable = @is_shareable");
                parameters.Add(new SqlParameter("@is_shareable", poll._is_shareable));
            }

            if (updates.Count == 0)
            {
                _logger.LogWarning("No fields to update.");
                return false; // Nothing to update
            }

            string setClause = string.Join(", ", updates);
            string query = $@"
                UPDATE [yolo].[poll]
                SET {setClause}
                WHERE id = @id AND created_by = @created_by;
            ";

            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddRange(parameters.ToArray());
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@created_by", created_by);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation($"Successfully updated the poll by {created_by} at {DateTime.UtcNow}");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to update the poll at {DateTime.UtcNow}");
                return false;
            }
        }
    }
}

