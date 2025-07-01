using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YoloSoccerApp.Data;
using YoloSoccerApp.Logic;

namespace YoloSoccerApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _IUserrepo;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository IUserrepo, ILogger<UserController> logger)
        {
            this._IUserrepo = IUserrepo;
            this._logger = logger;
        }

        //get all user details
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsersAsync()
        {
            
            IEnumerable<Users> users;
            try
            {
                users = await _IUserrepo.GetAllUsersAsync();
              
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
            return users.ToList();
        }

        [HttpPost]
        public async Task<ActionResult> AddUsersAsync([FromBody] Users value)
        {
            try
            {
                Console.WriteLine("sdfdsfdsfdsf"+value._email);
                await _IUserrepo.AddUserAsync(value);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
            return StatusCode(200);
        }

        [HttpPut("[action]/{username}")]
        public async Task<ActionResult> UpdateUserAsync(string username, [FromBody] Users user)
            
        {
          
            try
            {
                await _IUserrepo.UpdateUserAsync(username, user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
            return StatusCode(200);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(string username, string password)
        {
            try
            {
                bool result = await _IUserrepo.ValidatePassword(username, password);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
            return StatusCode(200);
        }
        
    }
}

