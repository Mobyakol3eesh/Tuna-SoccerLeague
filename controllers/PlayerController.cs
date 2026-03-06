

using Microsoft.AspNetCore.Mvc;

public class PlayerController : Controller
{
    private IPlayerService playerService;

    public PlayerController(IPlayerService playerService)
    {
        this.playerService = playerService;
    }

    [HttpGet("players")]
    public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayers()
    {
        var players = await playerService.GetAllPlayers();
        return Ok(players);
    }

    [HttpGet("players/{id}")]
    public async Task<ActionResult<Player>> GetPlayerDetailsById(int id)
    {
        try
        {
            var player = await playerService.GetPlayerDetailsById(id);
            return Ok(player);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }

    }
    [HttpPost("players")]
    public async Task<ActionResult> AddPlayer([FromBody] Player player)
    {
        try
        {
            await playerService.AddPlayer(player.Name, player.MarketValue, player.TeamId);
            return Ok("Player added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> Index()
    {
        var players = await playerService.GetAllPlayers();
        return View(players);  
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var player = await playerService.GetPlayerDetailsById(id);
            return View(player);
        }
        catch (Exception )
        {
            return NotFound();
        }
    }
}