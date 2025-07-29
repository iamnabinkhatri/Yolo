using System;
namespace YoloSoccerApp.Logic
{
	public class Player
	{
		public int? _id { get; set; }
		public Users _userId { get; set; }
		public string? _nickname { get; set; }
		public int? _playerNumber { get; set; }
		public PlayerRole? _playerRole { get; set; }

		public Player() { }
		public Player(int id) { this._id = id; }
		public Player(int id, Users userId, string nickname, int playerNumber, PlayerRole playerRole)
		{
			this._id = id;
			this._userId = userId;
			this._nickname = nickname;
			this._playerNumber = playerNumber;
			this._playerRole = playerRole;
		}

        public Player(Users userId, string nickname, int playerNumber, PlayerRole playerRole)
        {
            this._userId = userId;
            this._nickname = nickname;
            this._playerNumber = playerNumber;
            this._playerRole = playerRole;
        }

        public override string ToString()
        {
            return @$"\nId: {this._id}\nUser Id: {this._userId}\nNickname: {this._nickname}\n" +
				$"Playername: {this._playerNumber}" +
				$"PlayerRole: {this._playerRole}";
        }
    }
}

