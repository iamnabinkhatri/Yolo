using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<ActionResult> LoginAsync([FromBody] Users user)
        {
            Console.WriteLine("password");
            bool result = false;
            try
            {
                Console.WriteLine("username"+user._username+" password"+user._password);
                result = await _IUserrepo.ValidatePassword(user?._username, user._password);
                if (result)
                {
                    var userId = user._username; // Get from DB based on login
                    var accessToken = _jwtService.GenerateAccessToken(userId);
                    var refreshToken = _jwtService.GenerateRefreshToken();
                    Console.WriteLine("cookie");
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

        [HttpGet("check-session")]
        public async Task<ActionResult<bool>> CheckSessionAsync()
        {
            var token = Request.Cookies["access_token"];
            //  If token is missing → reject
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(false);
            }

            //  If token is invalid → reject 
            var principal = _jwtService.ValidateToken(token);
            if (principal == null)
            {
                return Unauthorized(false);
            }

            return Ok(true);
            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Console.WriteLine(username);
            // if (string.IsNullOrEmpty(username))
            // {
            //     return Unauthorized();
            // }
            //
            // var IsUserExists = await _IUserrepo.CheckUserExists(username);
            // if (!IsUserExists)
            // {
            //     return Unauthorized();
            // }
            // else
            // {
            //     return Ok(new { isAuthenticated = true });
            // }
        }
    }
}

