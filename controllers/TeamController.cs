

using Microsoft.AspNetCore.Mvc;

public class TeamController : Controller
{
    private ITeamService teamService;

    public TeamController(ITeamService teamService)
    {
        this.teamService = teamService;
    }

    [HttpGet("teams/MVP/{teamId}")]
    public async Task<ActionResult<Player>> GetMostValuablePlayerInTeam(int teamId)
    {
        try
        {
            var mostValuablePlayer = await teamService.GetMostValuablePlayerinTeam(teamId);
            return Ok(mostValuablePlayer);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams/{id}")]
    public async Task<ActionResult<Team>> GetTeamDetailsById(int id)
    {
        try
        {
            var team = await teamService.GetTeamDetailsById(id);
            return Ok(team);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams/teamplayers/{teamId}")]
    public async Task<ActionResult<IEnumerable<Player>>> GetAllTeamPlayers(int teamId)
    {
        try
        {
            var players = await teamService.GetALLTeamPlayers(teamId);
            return Ok(players);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams")]
    public async Task<ActionResult<IEnumerable<Team>>> GetAllTeams()
    {
        var teams = await teamService.GetAllTeams();
        return Ok(teams);
    }
    public async Task<IActionResult> Index()
    {
        var teams = await teamService.GetAllTeams();
        return View(teams);  
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var team = await teamService.GetTeamDetailsById(id);
            return View(team);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

}