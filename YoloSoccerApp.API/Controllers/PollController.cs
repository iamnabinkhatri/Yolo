using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YoloSoccerApp.Data;
using YoloSoccerApp.Logic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YoloSoccerApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {
        private readonly IPollRepository? _ipoll;
        private readonly IUserRepository? _iuser;
        private readonly IPollOptionRepository? _ioption;
        private readonly ILogger<PollController>? _logger;
        public PollController(IPollRepository ipoll, IUserRepository iuser, IPollOptionRepository ioption, ILogger<PollController> logger)
        {
            this._ipoll = ipoll;
            this._iuser = iuser;
            this._ioption = ioption;
            this._logger = logger;
        }
        
        
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            // This will read the "sub" claim (username in your case)
            var username = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("No username found in token.");
            }

            return Ok(new { Username = username });
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollWithOption>>> GetAllPollDetailsAsync()
        {
            try
            {
                IEnumerable<Poll> polls;
                IEnumerable<PollOption> options;
                var pollWithOptions = new List<PollWithOption>();
                polls = await _ipoll.GetAllPollsAsync();
                foreach (Poll poll in polls)
                {
                    int id = (int)poll._id;
                    options = await _ioption.GetAllPollByPollIdAsync(id);
                    pollWithOptions.Add(new PollWithOption { Poll=poll, Options=options});
                }
                return Ok(pollWithOptions);

            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return StatusCode(500,e.Message);
            }
        }
        
        /*Getting All the polls by specific user*/
        [HttpGet("[action]/{userId}")]
        public async Task<ActionResult<IEnumerable<PollWithOption>>> GetAllPollDetailsByUserId(int userId)
        {
            try
            {
                IEnumerable<Poll> polls;
                IEnumerable<PollOption> options;
                var pollWithOptions = new List<PollWithOption>();
                polls = await _ipoll.GetPollByUserIdAsync(userId);
                foreach (Poll poll in polls)
                {
                    int id = (int)poll._id;
                    options = await _ioption.GetAllPollByPollIdAsync(id);
                    pollWithOptions.Add(new PollWithOption { Poll = poll, Options = options });

                }
                return Ok(pollWithOptions);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{pollId}")]
        public async Task<ActionResult<IEnumerable<PollWithOption>>> GetAllPollDetailsByPollId(int pollId)
        {
            try
            {
                IEnumerable<Poll> polls;
                IEnumerable<PollOption> options;
                var pollWithOptions = new List<PollWithOption>();
                polls = await _ipoll.GetPollByPollIdAsync(pollId);
                foreach (Poll poll in polls)
                {
                    int id = (int)poll._id;
                    options = await _ioption.GetAllPollByPollIdAsync(id);
                    pollWithOptions.Add(new PollWithOption { Poll = poll, Options = options });

                }
                return Ok(pollWithOptions);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
        }
        
        /**
        Adding poll header/title and options for first time
        */
        [HttpPost]
        public async Task<ActionResult<bool>> AddPollWithDetailsAsync([FromBody] PollWithOption dto)
        {
            try
            {
                var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
                PollOption opt;
                Poll poll = dto.Poll;
                
                Users user = await _iuser!.GetUserByUsername(username);
                int id = (int)user._id;
                poll._created_by = new Users(id);
                int newId = await _ipoll!.AddPollAsync(poll);
                bool result = true;
                foreach (PollOption d in dto.Options)
                {
                    opt = new PollOption(new Poll(newId), d._option);
                    result = await _ioption!.AddPollOptionByPollIdAsync(opt);
                }
                if (result)
                {
                    return StatusCode(200, true);
                }
                else
                {
                    return StatusCode(500, false);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
        }
        
        /**
        Adding more options after adding it for first time
        */
        [HttpPost("poll-option/{pollId}")]
        public async Task<ActionResult<bool>> AddPollByPollIdAsync([FromBody] IEnumerable<PollOption> pollOptionList, int pollId)
        {
            try
            {
                PollOption opt= new PollOption();
                foreach (PollOption p in pollOptionList)
                {
                    // int PollId = (int)p._pollId._id;
                    opt = new PollOption(new Poll(pollId),p._option!);
                    bool result = await _ioption!.AddPollOptionByPollIdAsync(opt);
                    if (!result)
                    {
                       return StatusCode(500, false);
                    }
                }
                return StatusCode(200, true);
            }
            catch (Exception e)
            {
                _logger!.LogError(e, e.Message);
                return StatusCode(500, false);
            }
        }
        
        [HttpPut("{pollId}")]
        public async Task<ActionResult<bool>> UpdatePollWithDetailsAsync([FromBody] PollWithOption dto, int pollId)
        {
            try
            {
                int userId = Convert.ToInt32(dto.Poll._created_by?._id);
                if (!string.IsNullOrWhiteSpace(dto.Poll._title))
                {
                    bool result1 = await _ipoll!.UpdatePollAsync(pollId, userId, dto.Poll);
                    if (!result1)
                    {
                        return StatusCode(500, "Failed to update poll");
                    }
                }
                foreach (PollOption opt in dto.Options)
                {
                    int id = Convert.ToInt32(opt._id);
                    bool result2 = await _ioption!.UpdatePollOptionByIdAsync(id, opt);
                    if (!result2)
                    {
                        return StatusCode(500, $"Failed  to update option with Id: {id}");
                    }
                }
                return StatusCode(200, true);
            }
            catch (Exception e)
            {
                _logger!.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
        }
    }
    
    
}

