using System;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using YoloSoccerApp.Logic;
using System.Text;

namespace YoloSoccerApp.Data
{
    public class UserSqlRepository : IUserRepository
	{
        //fields
        private readonly string _connectionString;
        private readonly ILogger<UserSqlRepository> _logger;

		public UserSqlRepository(string connectionString, ILogger<UserSqlRepository> logger)
		{
            this._connectionString = connectionString;
            this._logger = logger;
		}

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {


            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"SELECT * FROM [yolo].[user];";
            using SqlCommand cmd = new SqlCommand(query, connection);
            using SqlDataReader read = await cmd.ExecuteReaderAsync();
            List<Users> users = new List<Users>();
            while (await read.ReadAsync())
            {
                int Id = (int)read["id"];
                string email = read["email"].ToString() ?? "";
                string password = read["password"].ToString() ?? "";
                string username = read["username"].ToString() ?? "";
                string firstName = read["firstName"].ToString() ?? "";
                string middleName = read["middleName"].ToString() ?? "";
                string lastName = read["lastName"].ToString() ?? "";
                int phoneNo = (int)read["phoneNo"];
                string city = read["city"].ToString() ?? "";
                int zipCode = (int)read["zipCode"];
                string state = read["state"].ToString() ?? "";
                string country = read["country"].ToString() ?? "";

                users.Add(new Users(Id,email,username,password,firstName,lastName,middleName,phoneNo,city,zipCode,state,country));
            }
            await connection.CloseAsync();
            return users; 
         }

        public async Task AddUserAsync(Users user)
            
        {
            string? password = PasswordHasher.HashPassword(user._password);
            Console.WriteLine("ADD password" + password);
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"INSERT INTO [yolo].[user] (email,username,password,firstName,lastName,middleName,phoneNo,city,zipCode,state,country) VALUES (@email,@username,@password,@firstName,@lastName,@middleName,@phoneNo,@city,@zipCode,@state,@country);";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@email", user._email);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@username", user._username);
            cmd.Parameters.AddWithValue("@firstName", user._firstName);
            cmd.Parameters.AddWithValue("@middleName", user._middleName);
            cmd.Parameters.AddWithValue("@lastName", user._lastName);
            cmd.Parameters.AddWithValue("@phoneNo", user._phoneNo);
            cmd.Parameters.AddWithValue("@city", user._city);
            cmd.Parameters.AddWithValue("@zipCode", user._zipCode);
            cmd.Parameters.AddWithValue("@state", user._state);
            cmd.Parameters.AddWithValue("@country", user._country);
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

        }

        public async Task UpdateUserAsync(string username, Users user)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            string query = @"UPDATE [yolo].[user] SET email=@email, password=@password,username=@username, firstName=@firstName,middleName=@middleName,lastName=@lastName, phoneNo=@phoneNo, city=@city, zipCode=@zipCode, state=@state, country=@country WHERE username=@condition;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            if(user._email != null)
            {
                cmd.Parameters.AddWithValue("@email", user._email);
            }
            if(user._password != null)
            {
                cmd.Parameters.AddWithValue("@password", user._password);
            }
            if (user._username != null)
            {
                cmd.Parameters.AddWithValue("@username", user._username);
            }
            if(user._firstName != null)
            {
                cmd.Parameters.AddWithValue("@firstName", user._firstName);
            }
            if(user._middleName != null)
            {
                cmd.Parameters.AddWithValue("@middleName", user._middleName);
            }
            if(user._lastName != null)
            {
                cmd.Parameters.AddWithValue("@lastName", user._lastName);
            }
            if(user._phoneNo != null)
            {
                cmd.Parameters.AddWithValue("@phoneNo", user._phoneNo);
            }
            if(user._city != null)
            {
                cmd.Parameters.AddWithValue("@city", user._city);
            }
            if(user._zipCode != null)
            {
                cmd.Parameters.AddWithValue("@zipCode", user._zipCode);
            }
            if(user._state != null)
            {
                cmd.Parameters.AddWithValue("@state", user._state);
            }
            if(user._country != null)
            {
                cmd.Parameters.AddWithValue("@country", user._country);
            }
            cmd.Parameters.AddWithValue("@condition", user._username);
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }


        public async Task<bool> ValidatePassword(string username, string password)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            bool result = false;
            if (await CheckUserExists(username))
            {
                string query = @"SELECT password FROM [yolo].[user] WHERE username=@user;";
                using SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@user", username);
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
               
                if (await reader.ReadAsync())
                {
                    string storedPassword = (string)reader["password"];
                    Console.WriteLine("userInput pass:" + password);
                    Console.WriteLine("Data Stored pass:" + storedPassword);

                    if (PasswordHasher.VerifyPassword(password, storedPassword)){
                        
                        result = true;
                    }
                }
                
            }
            await connection.CloseAsync();
            return result;
        }

        

        public async Task<bool> CheckUserExists(string username)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync();
            String query = @"SELECT username FROM [yolo].[user] where username=@username;";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", username);
            using SqlDataReader read = await cmd.ExecuteReaderAsync();
            bool result = false;
            if (read.HasRows)
            {
                result = true;
            }
            await connection.CloseAsync();
            return result;
        }
    }
}
