using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoloSoccerApp.Data;
using YoloSoccerApp.Logic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YoloSoccerApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserLoginController : ControllerBase
    {
        private readonly IUserLoginRepository? _iulrepo;
        private readonly ILogger<UserLoginController>? _logger;

        public UserLoginController(IUserLoginRepository iulrepo, ILogger<UserLoginController> logger)
        {
            this._iulrepo = iulrepo;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLogin>>> GetAllUserLoginAsync()
        {
            IEnumerable<UserLogin> ul;
            UserLogin uldetails = new UserLogin();
            try
            {
                ul = await _iulrepo.GetAllUserLoginAsync();

            } catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            if (ul.Count() > 0)
            {
                _logger.LogInformation($"user login retrieved successfully");
                return ul.ToList();
            }
            else
            {
                _logger.LogError($"Database is empty, start filling data");
                return Ok("Empty Table"); ;

            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserLogin>>> GetUserLoginByUserId(int userId)
        {
            IEnumerable<UserLogin> ul;
            try
            {
                ul = await _iulrepo.GetUserLoginByUserIdAsync(userId);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            if (ul.Count() > 0)
            {
                _logger.LogInformation($"user login retrieved successfully for {userId}");
                return ul.ToList();
            }
            else
            {
                _logger.LogError($"Database is empty for {userId}, start filling data");
                return Ok($"Empty Table for {userId}"); ;

            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddUserLoginAsync([FromBody] UserLogin ulogin)
        {
            try
            {
                bool is_added = await _iulrepo.AddNewUserLoginAsync(ulogin);
                if (!is_added)
                {
                    //StatusCode(400,false) -> this is same to the implemented return statement
                    _logger.LogError($"Cannot add to the database due to missing information {BadRequest(400)}");
                    return BadRequest(false);
                }
                else
                {
                    _logger.LogInformation($"User login successfully added to database");
                    return Ok(true);
                }
            } catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}/{userId}")]
        public async Task<ActionResult<bool>> UpdateUserLogoutTimeAsync(int id, int userId)
        {
            try
            {
                bool is_updated = await _iulrepo.UpdateUserLoginDetailsLogOutTime(id,userId);
                if (!is_updated)
                {
                    //StatusCode(400,false) -> this is same to the implemented return statement
                    _logger.LogError($"Cannot Update to the database due to missing information {BadRequest(400)}");
                    return BadRequest(false);
                }
                else
                {
                    _logger.LogInformation($"User {userId} Logout time successfully Updated/added to database");
                    return Ok(true);
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e,e.Message);
                return StatusCode(500, e.Message);
            }
        }
    }
}

