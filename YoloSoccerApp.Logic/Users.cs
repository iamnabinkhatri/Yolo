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

        //constructor
        public Users() { }
        public Users(int id, string email, string username, string password,
            string firstName, string lastName, string middleName, int phoneNo,
            string city, int zipCode, string state, string country) {
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
        }
        public Users( string email, string username, string password,
            string firstName, string lastName, string middleName, int phoneNo,
            string city, int zipCode, string state, string country)
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
        }

        //methods
        public override string ToString()
        {
            return $@"\nUser ID: {this._id} \n Email: {this._email}\n
            Password: {this._password}\n Username: {this._username}\n FirstName:{this._firstName}\n
            Middle Name: {this._middleName}\n Last Name: {this._lastName}\n
            Phone No: {this._phoneNo}\n City: {this._city}\n Zipcode: {this._zipCode}\n
            State: {this._state}\n Country: {this._country}
            ";
        }
    }
}

