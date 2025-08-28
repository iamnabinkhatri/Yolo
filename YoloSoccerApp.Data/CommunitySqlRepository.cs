using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data;

public class CommunitySqlRepository : ICommunityRepository
{
    private readonly string _connectionString;
    private readonly ILogger<CommunitySqlRepository> _logger;

    public CommunitySqlRepository(string connectionString, ILogger<CommunitySqlRepository> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }
    public async Task<IEnumerable<Community>> GetAllCommunitiesAsync()
    {
        using SqlConnection connection = new SqlConnection(_connectionString) ;
        await connection.OpenAsync();
        string query = @"SELECT * FROM [yolo].[community]";
        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        List<Community> communities = new List<Community>();
        while (await reader.ReadAsync())
        {
            int id = (int)reader["id"];
            string name = (string)reader["communityName"];
            string description = (string)reader["description"];
            communities.Add(new Community(id, name, description));
        }

        await connection.CloseAsync();
        return communities;
    }

    public async Task<bool> AddCommunityAsync(Community community)
    {
        using SqlConnection connection = new SqlConnection(_connectionString) ;
        await connection.OpenAsync();
        string query = @"INSERT INTO [yolo].[community] (communityName, description) VALUES (@communityName, @description)";
        using SqlCommand command = new SqlCommand(query, connection) ;
        command.Parameters.AddWithValue("@communityName", community._communityName);
        command.Parameters.AddWithValue("@description", community._description);
        int result = await command.ExecuteNonQueryAsync();
        bool checkResult = false;
        if (result > 0)
        {
            checkResult = true; 
        }
        else
        {
            checkResult = false;
        }
        await connection.CloseAsync();
        return checkResult;
    }
}
