


using Microsoft.EntityFrameworkCore;

public class MatchService : IMatchService
{
    private readonly TunaLeagueContext tunaLeagueContext;

    public MatchService(TunaLeagueContext tunaLeagueContext)
    {
        this.tunaLeagueContext = tunaLeagueContext;
    }

    public async Task<IEnumerable<MatchReadDto>> GetAllMatches()
    {
        var matches = await tunaLeagueContext.MatchTeams
        .AsNoTracking()
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
        return matches;
    }
    public async Task CreateMatch(CreateMatchDto dto)
    {
        if (dto.HomeTeamId == dto.AwayTeamId)
        {
            throw new Exception("Home team and away team must be different.");
        }

        var homeTeamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == dto.HomeTeamId);

        var awayTeamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == dto.AwayTeamId);

        if (!homeTeamExists || !awayTeamExists)
        {
            throw new Exception("Invalid team id provided for match creation.");
        }

        var nextMatchId = (await tunaLeagueContext.Matches
            .Select(m => (int?)m.Id)
            .MaxAsync() ?? 0) + 1;

        var match = new Match
        {
            Id = nextMatchId,
            Date = dto.Date,
            Location = dto.Location
        };
        tunaLeagueContext.Matches.Add(match);
        await tunaLeagueContext.SaveChangesAsync();

        var nextMatchTeamId = (await tunaLeagueContext.MatchTeams
            .Select(mt => (int?)mt.Id)
            .MaxAsync() ?? 0) + 1;

        var matchTeam = new MatchTeam
        {
            Id = nextMatchTeamId,
            MatchId = match.Id,
            HomeTeamId = dto.HomeTeamId,
            AwayTeamId = dto.AwayTeamId,
            HomeTeamScore = dto.HomeTeamScore,
            AwayTeamScore = dto.AwayTeamScore
        };
        tunaLeagueContext.MatchTeams.Add(matchTeam);
        await tunaLeagueContext.SaveChangesAsync();


    }


    public async Task<MatchReadDto> GetMatchDetailsById(int id)
    {
        var match = await tunaLeagueContext.MatchTeams.AsNoTracking()
        .Where(mt => mt.MatchId == id)
        .Select(mtdto => new MatchReadDto
        {
            Id = mtdto.MatchId,
            Date = mtdto.Match != null ? mtdto.Match.Date : default,
            Location = mtdto.Match != null ? mtdto.Match.Location : string.Empty,
            HomeTeamName = mtdto.HomeTeam != null ? mtdto.HomeTeam.Name : string.Empty,
            HomeTeamScore  = mtdto.HomeTeamScore,
            AwayTeamName = mtdto.AwayTeam != null ? mtdto.AwayTeam.Name : string.Empty,
            AwayTeamScore = mtdto.AwayTeamScore
        }).FirstOrDefaultAsync();
        return match ?? throw new Exception("Match not found");
    }

    public async Task UpdateMatch(int id, UpdateMatchDto dto)
    {
        if (dto.HomeTeamId == dto.AwayTeamId)
        {
            throw new Exception("Home team and away team must be different.");
        }

        var match = await tunaLeagueContext.Matches
            .FirstOrDefaultAsync(m => m.Id == id);

        if (match == null)
        {
            throw new Exception("Match not found");
        }

        var matchTeam = await tunaLeagueContext.MatchTeams
            .FirstOrDefaultAsync(mt => mt.MatchId == id);

        if (matchTeam == null)
        {
            throw new Exception("Match teams data not found for this match");
        }

        var homeTeamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == dto.HomeTeamId);

        var awayTeamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == dto.AwayTeamId);

        if (!homeTeamExists || !awayTeamExists)
        {
            throw new Exception("Invalid team id provided for match update.");
        }

        match.Date = dto.Date  != default ? dto.Date : match.Date;
        match.Location = dto.Location ?? match.Location;

        matchTeam.HomeTeamId = dto.HomeTeamId != 0 ? dto.HomeTeamId : matchTeam.HomeTeamId;
        matchTeam.AwayTeamId = dto.AwayTeamId != 0 ? dto.AwayTeamId : matchTeam.AwayTeamId;
        matchTeam.HomeTeamScore = dto.HomeTeamScore != 0 ? dto.HomeTeamScore : matchTeam.HomeTeamScore;
        matchTeam.AwayTeamScore = dto.AwayTeamScore != 0 ? dto.AwayTeamScore : matchTeam.AwayTeamScore;

        await tunaLeagueContext.SaveChangesAsync();
    }

    
}
    