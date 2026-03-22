using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class CoachController : Controller
{
    private readonly ICoachService coachService;

    public CoachController(ICoachService coachService)
    {
        this.coachService = coachService;
    }

    [HttpGet("coaches")]
    [SwaggerOperation(Summary = "Get all coaches", Description = "Returns a list of all coaches.")]
    public async Task<ActionResult<IEnumerable<CoachReadDto>>> GetAllCoaches()
    {
        var coaches = await coachService.GetAllCoaches();
        return Ok(coaches);
    }

    [HttpGet("coaches/{id}")]
    [SwaggerOperation(Summary = "Get coach by id", Description = "Returns coach details by id.")]
    public async Task<ActionResult<CoachReadDto>> GetCoachDetailsById(int id)
    {
        try
        {
            var coach = await coachService.GetCoachDetailsById(id);
            return Ok(coach);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("coaches")]
    [SwaggerOperation(Summary = "Create coach", Description = "Creates a new coach record.")]
    public async Task<ActionResult> AddCoach([FromBody] CreateCoachDto dto)
    {
        try
        {
            await coachService.AddCoach(dto);
            return Ok("Coach added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }

    [HttpPut("coaches/{id}")]
    [SwaggerOperation(Summary = "Update coach", Description = "Updates an existing coach by id.")]
    public async Task<ActionResult> UpdateCoach(int id, [FromBody] UpdateCoachDto dto)
    {
        try
        {
            await coachService.UpdateCoach(id, dto);
            return Ok("Coach updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }
}