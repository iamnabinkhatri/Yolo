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
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleRepository _IUserRepo;
        private readonly ILogger<IUserRoleRepository> _logger;

        public UserRoleController(IUserRoleRepository IUserRepo, ILogger<IUserRoleRepository> logger)
        {
            this._IUserRepo = IUserRepo;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetAllUserRole()
        {
            IEnumerable<UserRole> userRoles;
            try
            {
                userRoles = await _IUserRepo.GetAllUserRoles();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
            return userRoles.ToList();
        }

        [HttpPost]
        public async Task<ActionResult> AddUserRole([FromBody] UserRole role)
        {
            try {
                await _IUserRepo.AddUserRole(role);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
            return StatusCode(200);
        }
    }
}

