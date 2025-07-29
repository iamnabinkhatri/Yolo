using System;
namespace YoloSoccerApp.Logic
{
	public class PlayerRole
	{
		//Fields
		public int? _id { get; set; }
		public string? _playerRole { get; set; }

		//constructor
		public PlayerRole() { }
		public PlayerRole(int id)
		{
			this._id = id;
		}

		public PlayerRole(int id, string playerRole)
		{
			this._id = id;
			this._playerRole = playerRole;
		}
        public PlayerRole(string playerRole)
        {
            this._playerRole = playerRole;
        }

        public override string ToString()
        {
			return $@"
			ID: {this._id}
			PlayerRole: {this._playerRole}
			";
        }
    }
}

