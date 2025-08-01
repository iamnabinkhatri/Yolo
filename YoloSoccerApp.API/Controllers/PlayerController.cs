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
    public class PlayerController : ControllerBase
    {

        private readonly IPlayerRepository? _iPlayer;
        private ILogger<PlayerController>? _logger;

        public PlayerController(IPlayerRepository iplayer, ILogger<PlayerController> logger)
        {
            this._iPlayer = iplayer;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayerAsync()
        {
            IEnumerable<Player> players;
            try
            {
                players = await _iPlayer.GetAllPlayerAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
            return players.ToList();
        }

        [HttpPost]
        public async Task<ActionResult> AddPlayerAsync([FromBody] Player player)
        {
            try
            {
                Console.WriteLine($"userId: {player._userId._id}");
                await _iPlayer.AddPlayerAsync(player);
            }
            catch (Exception e)
            {
               _logger.LogError(e, e.Message);
                return StatusCode(500,e.Message);
            }
            return StatusCode(200);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdatePlayerAsync(int userId, [FromBody] Player player)
        {
            try
            {
                await _iPlayer.UpdatePlayerAsync(userId, player);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
            return StatusCode(200);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<bool>> DeletePlayerAsync(int userId)
        {
         
            try
            {
                bool result = await _iPlayer.DeletePlayerAsync(userId);
                if (!result)
                {
                    return NotFound(false);
                }
                //return StatusCode(200, true);
                return Ok(true);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500,false);
            }
            
        }
    }
}

