using System;
namespace YoloSoccerApp.Logic
{
	public class Player
	{
		public int? _id { get; set; }
		public Users _userId { get; set; }
		public string? _nickname { get; set; }
		public int? _playerNumber { get; set; }

		public Player() { }
		public Player(int id, Users userId, string nickname, int playerNumber)
		{
			this._id = id;
			this._userId = userId;
			this._nickname = nickname;
			this._playerNumber = playerNumber;
		}

        public Player(Users userId, string nickname, int playerNumber)
        {
            this._userId = userId;
            this._nickname = nickname;
            this._playerNumber = playerNumber;
        }

        public override string ToString()
        {
            return $"@\nId: {this._id}\nUser Id: {this._userId}\nNickname: {this._nickname}\n" +
				$"Playername: {this._playerNumber}";
        }
    }
}

