

using System.ComponentModel.DataAnnotations.Schema;

public class Team
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? Points { get; set; }

    public Coach? Coach { get; set; }
    public IEnumerable<Player> Players { get; set; } = new List<Player>();

    public IEnumerable<MatchTeam> Matches { get; set; } = new List<MatchTeam>();


    

}