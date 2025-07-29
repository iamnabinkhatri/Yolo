using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoloSoccerApp.Data;
using YoloSoccerApp.Logic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YoloSoccerApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerStaticsController : ControllerBase
    {
        // Fields
        private readonly IPlayerStaticsRepository _ips;
        private readonly ILogger<PlayerController> _logger;

        public PlayerStaticsController(IPlayerStaticsRepository ips, ILogger<PlayerController> logger)
        {
            this._ips = ips;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerStatics>>> GetAllPlayerStaticsAsync()
        {
            IEnumerable<PlayerStatics> playerStatics;
            try
            {
                playerStatics = await _ips.GetAllPlayerStaticsAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return playerStatics.ToList();
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<PlayerStatics>> GetPlayerStaticsByPlayerIdAsync(int playerId )
        {
            PlayerStatics pst = new PlayerStatics();
            try
            {
                pst = await _ips.GetPlayerStaticById(playerId);
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return StatusCode(500, e.Message);
            }
            return pst;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddPlayerStaticsAsync([FromBody] PlayerStatics playerStatics)
        {
            try
            {
                bool addPlayer = await _ips.AddPlayerStaticsAsync(playerStatics);
                if (!addPlayer)
                {
                    return Unauthorized(false);
                }
                return Ok(true);
            }catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{playerId}")]
        public async Task<ActionResult> UpdatePlayerStaticsAsync(int playerId, [FromBody] PlayerStatics pst)
        {
            try
            {
                await _ips.UpdatePlayerStaticsAsync(playerId, pst);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return StatusCode(200, "Updated Successfully");
        }

        [HttpDelete("{playerId}")]
        public async Task<ActionResult<bool>> DeletePlayerStaticsAsync(int playerId)
        {
            try
            {
                bool isDeleted = await _ips.DeletePlayerStaticById(playerId);
                if (!isDeleted)
                {
                    return Unauthorized(false);
                }
                return Ok(true);
            }catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
        }
    }
}

