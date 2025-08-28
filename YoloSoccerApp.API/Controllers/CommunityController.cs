using Microsoft.AspNetCore.Mvc;
using YoloSoccerApp.Data;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommunityController : ControllerBase
{
    private readonly ILogger<CommunityController> _logger;
    private readonly ICommunityRepository _communityRepository;
    
    public CommunityController(ILogger<CommunityController> logger, ICommunityRepository communityRepository)
    {
        _logger = logger;
        _communityRepository = communityRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Community>>> GetAllCommunities()
    {
        try
        {
            IEnumerable<Community> communities = await _communityRepository.GetAllCommunitiesAsync();
            if (communities == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return communities.ToList();;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            // throw;
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("{communityId}")]
    public async Task<ActionResult<Community>> GetSingleCommunityAsync(int communityId)
    {
        try
        {
            IEnumerable<Community> communities = await _communityRepository.GetAllCommunitiesAsync();
            Community community = communities.Single(x=>x._id==communityId);
            if (community == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return Ok(community);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<bool>> AddCommunityDataAsync([FromBody] Community community)
    {
        try
        {
            bool result = await _communityRepository.AddCommunityAsync(community);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            // throw;
            return StatusCode(500, e.Message);
        }
    }
}
