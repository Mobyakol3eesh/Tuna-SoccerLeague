
using Microsoft.EntityFrameworkCore;

public class TeamService : ITeamService
{
   
    
    
    private readonly TunaLeagueContext tunaLeagueContext;
    public TeamService(TunaLeagueContext tunaLeagueContext)
    {
        
        this.tunaLeagueContext = tunaLeagueContext;

        
    }

    public async Task CreateTeam(CreateTeamDto dto)
    {
        var nextTeamId = (await tunaLeagueContext.Teams
            .Select(t => (int?)t.Id)
            .MaxAsync() ?? 0) + 1;

        var newTeam = new Team
        {
            Id = nextTeamId,
            Name = dto.Name,
            Points = 0,
        };
        tunaLeagueContext.Teams.Add(newTeam);
        await tunaLeagueContext.SaveChangesAsync();
    }
    public async Task<CoachReadDto> GetTeamCoach(int teamId)
    {
        var teamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == teamId);

        if (!teamExists)
        {
            throw new Exception($"Team with ID {teamId} not found.");
        }

        var coach = await tunaLeagueContext.Coaches
            .AsNoTracking()
            .Where(c => c.TeamId == teamId)
            .Select(c => new CoachReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Age = c.Age,
                ExperienceYrs = c.ExperienceYrs,
                TeamName = c.Team != null ? c.Team.Name : string.Empty
            }).FirstOrDefaultAsync();

        if (coach == null)
        {
            throw new Exception($"No coach assigned to Team with ID {teamId}.");
        }
        return coach;
    }
    public async Task UpdateTeam(int id, UpdateTeamDto dto)
    {
        var team = await tunaLeagueContext.Teams
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team == null)
        {
            throw new Exception("Team not found");
        }

        team.Name = dto.Name;
        team.Points = dto.Points;

        await tunaLeagueContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<MatchReadDto>> GetTeamMatches(int teamId)
    {
        var mathes = await tunaLeagueContext.MatchTeams
        .AsNoTracking()
        .Where(mt => mt.HomeTeamId == teamId || mt.AwayTeamId == teamId)
        .Select(mtdto => new MatchReadDto
        {
            Id = mtdto.MatchId,
            Date = mtdto.Match != null ? mtdto.Match.Date : default,
            Location = mtdto.Match != null ? mtdto.Match.Location : string.Empty,
            HomeTeamName = mtdto.HomeTeam != null ? mtdto.HomeTeam.Name : string.Empty,
            HomeTeamScore  = mtdto.HomeTeamScore,
            AwayTeamName = mtdto.AwayTeam != null ? mtdto.AwayTeam.Name : string.Empty,
            AwayTeamScore = mtdto.AwayTeamScore
        }).ToListAsync();
        return mathes;
    }

    public async Task<IEnumerable<TeamReadDto>> GetAllTeams()
    {
        var tdtos = await tunaLeagueContext.Teams.AsNoTracking().Select(t => new TeamReadDto
        {
            Id = t.Id,
            Name = t.Name,
            Points = t.Points,
            Players = t.Players.Select(p => new PlayerReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Position = p.Position,
                MarketValue = p.MarketValue,
                TeamName = t.Name
            }).ToList(),
            CoachName = t.Coach != null ? t.Coach.Name : string.Empty,
        }
                    ).ToListAsync();
        return tdtos;
    }



    public async Task<IEnumerable<PlayerReadDto>> GetALLTeamPlayers(int teamId)
    {
       var tplayersdtos = await tunaLeagueContext.Players.AsNoTracking().Where(p => p.TeamId == teamId).Select(p => new PlayerReadDto
       {
           Id = p.Id,
           Name = p.Name,
           Age = p.Age,
           Position = p.Position,
           MarketValue = p.MarketValue,
          TeamName = p.Team != null ? p.Team.Name : string.Empty
       }).ToListAsync();
       return tplayersdtos;
    }
    
    
    public async Task<TeamReadDto> GetTeamDetailsById(int id)
    {
        var team = await tunaLeagueContext.Teams.AsNoTracking()
        .Where(t => t.Id == id)
        .Select(t => new TeamReadDto
        {
            Id = t.Id,
            Name = t.Name,
            Points = t.Points,
            Players = t.Players.Select(p => new PlayerReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Position = p.Position,
                MarketValue = p.MarketValue,
                TeamName = t.Name
            }).ToList(),
            CoachName = t.Coach != null ? t.Coach.Name : string.Empty,
        }).FirstOrDefaultAsync();
        return team ?? throw new Exception("Team not found");
    }

    public async Task<PlayerReadDto> GetMostValuablePlayerinTeam(int teamID)
    {
        var teamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == teamID);

        if (!teamExists)
        {
            throw new Exception($"Team with ID {teamID} not found.");
        }

        var mostValuablePlayer = await tunaLeagueContext.Players
            .AsNoTracking()
            .Where(p => p.TeamId == teamID)
            .OrderByDescending(p => p.MarketValue)
            
            .Select(p => new PlayerReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Position = p.Position,
                MarketValue = p.MarketValue,
                TeamName = p.Team != null ? p.Team.Name : string.Empty
            }).FirstOrDefaultAsync();
                

        if (mostValuablePlayer == null)
        {
            throw new Exception($"No players found in Team with ID {teamID}.");
        }
        return mostValuablePlayer;
    }

    
}