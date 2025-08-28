using YoloSoccerApp.Logic;

namespace YoloSoccerApp.Data;

public interface ICommunityRepository
{
    public Task<IEnumerable<Community>> GetAllCommunitiesAsync();
    public Task<Boolean> AddCommunityAsync(Community community);
    
}
