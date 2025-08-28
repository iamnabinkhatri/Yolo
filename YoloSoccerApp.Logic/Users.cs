using System;

namespace YoloSoccerApp.Logic
{
	public class Users
	{
        //Fields
        public int? _id { get; set; }
        public string? _email { get; set; }
        public string? _password { get; set; }
        public string? _username { get; set; }
        public string? _firstName { get; set; }
        public string? _lastName { get; set; }
        public string? _middleName { get; set; }
        public int? _phoneNo { get; set; }
        public string? _city { get; set; }
        public int? _zipCode { get; set; }
        public string? _state { get; set; }
        public string? _country { get; set; }
        public UserRole? _roleId { get; set; }
        
        public DateOnly? _dob { get; set; }
        
        public char? _gender { get; set; }
        


        //constructor
        public Users() { }
        public Users(int id) {
            this._id = id;
        }

        public Users(string username, string password)
        {
            this._username = username;
            this._password = password;
        }
        public Users(int id, string email, string username, string password,
            string firstName, string lastName, string middleName, int phoneNo,
            string city, int zipCode, string state, string country, UserRole roleId, DateOnly? dob, char? gender) {
            this._id = id;
            this._email = email;
            this._password = password; 
            this._username = username;
            this._firstName = firstName;
            this._lastName = lastName;
            this._middleName = middleName;
            this._phoneNo = phoneNo;
            this._city = city;
            this._zipCode = zipCode;
            this._state = state;
            this._country = country;
            this._roleId = roleId;
            this._dob = dob;
            this._gender = gender;
        }
        public Users( string email, string username, string password,
            string firstName, string lastName, string middleName, int phoneNo,
            string city, int zipCode, string state, string country, UserRole roleId, DateOnly? dob, char? gender)
        {
            
            this._email = email;
            this._password = password;
            this._username = username;
            this._firstName = firstName;
            this._lastName = lastName;
            this._middleName = middleName;
            this._phoneNo = phoneNo;
            this._city = city;
            this._zipCode = zipCode;
            this._state = state;
            this._country = country;
            this._roleId = roleId;
            this._dob = dob;
            this._gender = gender;
        }

        //methods
        public override string ToString()
        {
            return $@"
            User ID: {this._id}
            Email: {this._email}
            Password: {this._password}
            Username: {this._username} 
            FirstName:{this._firstName}
            Middle Name: {this._middleName}
            Last Name: {this._lastName}
            Phone No: {this._phoneNo}
            City: {this._city}
            Zipcode: {this._zipCode}
            State: {this._state}
            Country: {this._country}
            RoleId: {this._roleId?._id}
            DOB: {this._dob}
            Gender: {this._gender}
            ";
        }
    }
}

