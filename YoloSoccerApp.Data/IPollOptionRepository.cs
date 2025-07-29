using System;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data
{
	public interface IPollOptionRepository
	{
		public Task<IEnumerable<PollOption>> GetAllPollByPollIdAsync(int pollId);
		public Task<bool> AddPollOptionByPollIdAsync(PollOption option);
		public Task<bool> DeletePollOptionByOptionIdAndPollIdAsync(int id, int pollId);
		public Task<bool> UpdatePollOptionByIdAsync(int id, PollOption option);
	}
}