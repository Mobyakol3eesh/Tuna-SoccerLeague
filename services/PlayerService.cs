
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

public class PlayerService : IPlayerService
{
    
    

   FootballContext footballContext;
    public PlayerService(FootballContext footballContext)
    {
        this.footballContext = footballContext;
    }
    public async Task<Player> GetPlayerDetailsById(int id)
    {
        var player = await footballContext.players.Where(p => p.Id == id).FirstOrDefaultAsync();
        if (player == null)
        {
            throw new Exception($"Player with ID {id} not found.");
        }
        return player;
    }
    public async Task AddPlayer(String name, int marketValue, int? teamID)
    {
        var newPlayer = new Player
        {
            Id = footballContext.players.Max(p => p.Id) + 1,
            Name = name,
            MarketValue = marketValue,
            TeamId = teamID
        };
        footballContext.players.Add(newPlayer);
        await footballContext.SaveChangesAsync();
    }
   

    public async Task<IEnumerable<Player>> GetAllPlayers()
    {
        return await footballContext.players.ToListAsync();
    }
}

