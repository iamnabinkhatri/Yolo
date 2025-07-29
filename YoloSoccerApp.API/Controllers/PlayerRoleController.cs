
using Microsoft.AspNetCore.Mvc;
using YoloSoccerApp.Data;
using YoloSoccerApp.Logic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YoloSoccerApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerRoleController : ControllerBase
    {
        private readonly IPlayerRoleRepository _iprole;
        private readonly ILogger<PlayerRoleController> _logger;

        public PlayerRoleController(IPlayerRoleRepository iprole, ILogger<PlayerRoleController> logger)
        {
            this._iprole = iprole;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerRole>>> GetAllPlayerRoles()
        {
            IEnumerable<PlayerRole> proles = new List<PlayerRole>();
            try
            {
                proles = await _iprole.GetAllPlayerRoles();
            }catch(Exception e)
            {
                _logger.LogInformation(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return proles.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerRole>> GetPlayerRole(int id)
        {
            PlayerRole pr = new PlayerRole();
            try
            {
                pr = await _iprole.GetPlayerRoleById(id);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, e.Message);
                return StatusCode(500);
            }
            return pr;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdatePlayerRole(int id, [FromBody] PlayerRole pr)
        {
            try
            {

               bool isUpdated =  await _iprole.UpdatePlayerRole(id, pr);
                if (!isUpdated)
                {
                    return StatusCode(403, false);
                }
                else
                {
                    return Ok(true);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddPlayerRole([FromBody] PlayerRole prole) {
            try
            {
              bool isAdded =   await _iprole.AddPlayerRole(prole);
                if (!isAdded)
                {
                    return StatusCode(403, false);
                }
                else
                {
                    return Ok(true);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeletePlayerRole(int id)
        {
            try
            {
                bool isDeleted = await _iprole.DeletePlayerRole(id);
                if (isDeleted)
                {
                    return StatusCode(403, false);
                }
                else
                {
                    return Ok(true);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
        }

    }
}

