using System;
namespace YoloSoccerApp.Logic
{
	public class PollOption
	{

		public int? _id { get; set; }
		public Poll? _pollId { get; set; }
		public string? _option { get; set; }

		public PollOption() { }

		public PollOption(int id)
		{
			this._id = id;
		}

		public PollOption(int id, Poll pollId, string option)
		{
			this._id = id;
			this._pollId = pollId;
			this._option = option;
		}
        public PollOption(Poll pollId, string option)
        {
            this._pollId = pollId;
            this._option = option;
        }

        public PollOption(string option)
        {
            this._option = option;
        }

        public override string ToString()
        {
			return $@"
			Poll Option ID: {this._id}
			Poll ID: {this._pollId?._id}
			Option: {this._option}
			";
        }
    }
}

