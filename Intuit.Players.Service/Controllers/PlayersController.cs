using Intuit.Players.BL;
using Intuit.Players.Common;
using Microsoft.AspNetCore.Mvc;

namespace Intuit.Players.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayersHandler _playersHandler;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(IPlayersHandler playersHandler, ILogger<PlayersController> logger)
    {
        _playersHandler = playersHandler;
        _logger = logger;
    }

    [HttpGet(Name = "GetAllPlayers")]
    public IActionResult GetAll(int limit, int offset)
    {
        if (limit <= 0 || offset <= 0)
        {
            return BadRequest();
        }

        var players = _playersHandler.GetAllPlayers(limit, offset);
        if (players is null || players.Count == 0)
        {
            return NotFound();
        }

        return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest();
        }

        try
        {
            var player = await _playersHandler.GetById(id);
            if (player is null)
            {
                return NotFound();
            }

            return Ok(player);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}
