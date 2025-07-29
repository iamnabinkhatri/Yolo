using System;
namespace YoloSoccerApp.Logic
{
	public class Poll
	{
		public int? _id { get; set; }
		public string? _title { get; set; }
		public string? _description { get; set; }
		public Users? _created_by { get; set; }
		public DateTime _created_at { get; set; }
		public DateTime _close_at { get; set; }
		public char? _is_closed { get; set; }
		public decimal _latitude { get; set; }
		public decimal _longitude { get; set; }
		public char? _is_shareable { get; set; }

		public Poll() { }


        public Poll(int id)
		{
			this._id = id;
		}

		public Poll(int id, string title, string description, Users created_by,
			DateTime created_at, DateTime close_at, char is_closed,
			decimal latitude, decimal longitude, char is_shareable
		)
		{
			this._id = id;
			this._title = title;
			this._description = description;
			this._created_by = created_by;
			this._created_at = created_at;
			this._close_at = close_at;
			this._is_closed = is_closed;
			this._latitude = latitude;
			this._longitude = longitude;
			this._is_shareable = is_shareable;
		}

        public Poll(string title, string description, Users created_by,
            DateTime created_at, DateTime close_at, char is_closed,
            decimal latitude, decimal longitude, char is_shareable
        )
        {
            this._title = title;
            this._description = description;
            this._created_by = created_by;
            this._created_at = created_at;
            this._close_at = close_at;
            this._is_closed = is_closed;
            this._latitude = latitude;
            this._longitude = longitude;
            this._is_shareable = is_shareable;
        }

        public Poll(string title, string description,
            DateTime created_at, DateTime close_at, char is_closed,
            decimal latitude, decimal longitude, char is_shareable
        )
        {
            this._title = title;
            this._description = description;
            this._created_at = created_at;
            this._close_at = close_at;
            this._is_closed = is_closed;
            this._latitude = latitude;
            this._longitude = longitude;
            this._is_shareable = is_shareable;
        }

        public override string ToString()
        {
            return $@"
			ID: {this._id}
			Title: {this._title}
			Description: {this._description}
			Created By: {this._created_by}
			Created AT: {this._created_at}
			Closed AT: {this._close_at}
			Latitude: {this._latitude}
			Longitude: {this._longitude}
			Is Shareable: {this._is_shareable}
			";
        }
    }
}


