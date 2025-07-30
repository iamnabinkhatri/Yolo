using System;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public interface IPollRepository
	{
		public Task<IEnumerable<Poll>> GetAllPollsAsync();
		public Task<IEnumerable<Poll>> GetPollByUserIdAsync(int userId);
		public Task<IEnumerable<Poll>> GetPollByPollIdAsync(int pollId);
		public Task<int> AddPollAsync(Poll poll);
		public Task<bool> UpdatePollAsync(int id, int created_by, Poll poll);
		public Task<bool> DeletePollAsync(int id);
	}
}

