

public interface ITeamService
{
    Task<IEnumerable<Player>> GetALLTeamPlayers(int teamId);

    Task<Team> GetTeamDetailsById(int id);
    Task<Player> GetMostValuablePlayerinTeam(int teamId);

    Task<IEnumerable<Team>> GetAllTeams();


    
}