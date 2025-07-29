using System;
namespace YoloSoccerApp.Logic
{
	public class UserLogin
	{

		public int? _id { get; set; }
		public Users? _userId { get; set; } //From this object we can get userId
		public char? _isLoggedIn { get; set; }
		public DateTime? _loginStartedAt { get; set; }
		public DateTime? _loginEndsAt { get; set; }


		public UserLogin() { }

		public UserLogin(int id)
		{
			this._id = id;
		}

		public UserLogin(int id, Users userId, char isLoggedIn, DateTime loginStartedAt, DateTime loginEndsAt)
		{
			this._id = id;
			this._userId = userId;
			this._isLoggedIn = isLoggedIn;
			this._loginStartedAt = loginStartedAt;
			this._loginEndsAt = loginEndsAt;
		}

        public UserLogin(Users userId, char isLoggedIn, DateTime loginStartedAt, DateTime loginEndsAt)
        {
            this._userId = userId;
            this._isLoggedIn = isLoggedIn;
            this._loginStartedAt = loginStartedAt;
            this._loginEndsAt = loginEndsAt;
        }

        public override string ToString()
        {
            return $@"
			Id: {this._id}
			UserId: {this._userId?._id}
			isLoggedIn: {this._isLoggedIn},
			login Time: {this._loginStartedAt},
			logout Time: {this._loginEndsAt}
			";
        }
    }
}

