using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YoloSoccerApp.API.Services;
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
        private readonly JwtService  _jwtService;
        private readonly JwtSettings _jwtSettings;

        public UserController(IUserRepository IUserrepo,
            ILogger<UserController> logger, JwtService jwtService, JwtSettings jwtSettings)
        {
            this._IUserrepo = IUserrepo;
            this._logger = logger;
            this._jwtService = jwtService;
            this._jwtSettings = jwtSettings;
        }

        //get all user details
        [Authorize]
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
            bool result = false;
            try
            {
                
                result = await _IUserrepo.ValidatePassword(username, password);
                if (result)
                {
                    var userId = "123"; // Get from DB based on login

                    var accessToken = _jwtService.GenerateAccessToken(userId);
                    var refreshToken = _jwtService.GenerateRefreshToken();

                    // 2. Save refresh token to DB

                    // 3. Set cookie
                    Response.Cookies.Append("access_token", accessToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
                    });

                    Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
                    });
                    return StatusCode(200);
                }
                else
                {
                return StatusCode(401, "Username or password is incorrect");
                    
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
            
        }
        
    }
}

