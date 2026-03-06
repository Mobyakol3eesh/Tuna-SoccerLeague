


public interface IPlayerService
{
   
    Task<IEnumerable<Player>> GetAllPlayers();
    Task<Player> GetPlayerDetailsById(int id);

    Task AddPlayer(String name, int marketValue, int? teamID);

}