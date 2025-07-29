using System;
namespace YoloSoccerApp.Logic
{
	public class PlayerStatics
	{
        //fields or properties and getter setter
        public int? _id { get; set; }
        public Player? _player { get; set; } //In db save only id as a foreign key
        public int? _goals { get; set; }
        public int? _assists { get; set; }
        public int? _save { get; set; }
        public char? _attendance { get; set; }

		public PlayerStatics() { }
		public PlayerStatics(int id)
        {
            this._id = id;
        }

        public PlayerStatics(int id, Player player, int goals, int assists, int save, char attendance)
        {
            this._id = id;
            this._player = player;
            this._goals = goals;
            this._assists = assists;
            this._save = save;
            this._attendance = attendance;
        }

        public PlayerStatics(Player player, int goals, int assists, int save, char attendance)
        {
            this._player = player;
            this._goals = goals;
            this._assists = assists;
            this._save = save;
            this._attendance = attendance;
        }

        public override string ToString()
        {
            return @$"
            ID: {this._id}
            Player ID: {this._player._id}
            Goals: {this._goals}
            Assists: {this._assists}
            Save: {this._save}
            Attendance: {this._attendance}
            ";
        }
    }
}

