using System;
namespace YoloSoccerApp.Logic
{
	public class PollWithOption
	{
		public Poll Poll { get; set; }
		public IEnumerable<PollOption> Options { get; set; }

        public PollWithOption() { }
		
	}
}

