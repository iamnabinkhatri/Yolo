using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public class PollOptionSqlRepository : IPollOptionRepository
	{
        private readonly string _connectionString;
        private readonly ILogger<PollOptionSqlRepository> _logger;

		public PollOptionSqlRepository(string connectionString, ILogger<PollOptionSqlRepository> logger)
		{
            this._connectionString = connectionString;
            this._logger = logger;
		}

        public async Task<bool> AddPollOptionByPollIdAsync(PollOption p)
        {
            Console.WriteLine($"pollId:{p._pollId?._id}\nOption:{p._option}");
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"INSERT INTO [yolo].[pollOption] (pollId,[option]) VALUES (@pollId,@option);";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@pollId", p._pollId?._id);
            cmd.Parameters.AddWithValue("@option", p._option);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation($"Successfully added the Poll Option by {p._pollId?._id} at {DateTime.UtcNow}");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to Add the Poll Option at {DateTime.UtcNow}");
                return false;
            }

        }

        public async Task<bool> DeletePollOptionByOptionIdAndPollIdAsync(int id, int pollId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"DELETE FROM [yolo].[pollOption] WHERE id=@id AND pollId=@pollId;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id",id);
            cmd.Parameters.AddWithValue("@pollId", pollId);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation($"Successfully Deleted the Poll Option at {DateTime.UtcNow}");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to Delete the Poll Option at {DateTime.UtcNow}");
                return false;
            }
        }

        public async Task<IEnumerable<PollOption>> GetAllPollByPollIdAsync(int pollId)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[pollOption] WHERE pollId=@pollId;";
            using SqlCommand cmd = new SqlCommand(query,connection);
            cmd.Parameters.AddWithValue("@pollId", pollId);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            List<PollOption> options = new List<PollOption>();
            while (await reader.ReadAsync())
            {
                int id = (int)reader["id"];
                int pId = (int)reader["pollId"];
                string option = reader["option"].ToString() ?? "";
                options.Add(
                        new PollOption(id, new Poll(pollId), option)
                    );

            }
            await connection.CloseAsync();
            return options;

        }

        public async Task<bool> UpdatePollOptionByIdAsync(int id, PollOption option)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"UPDATE [yolo].[pollOption] SET option=@option WHERE id=@id;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@option", option._option);
            cmd.Parameters.AddWithValue("@id", id);
            int affectedRows = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            if (affectedRows > 0)
            {
                _logger.LogInformation($"Successfully Updated the Poll Option for id: {id} at {DateTime.UtcNow}");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to Update the Poll Option at {DateTime.UtcNow}");
                return false;
            }
        }
    }
}

