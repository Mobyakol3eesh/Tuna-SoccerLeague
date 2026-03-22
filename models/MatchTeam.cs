
using System.ComponentModel.DataAnnotations.Schema;
public class MatchTeam
{
    public int Id { get; set; }

    [ForeignKey("Match")]
    public int MatchId { get; set; }
    public  Match? Match { get; set; }
    
    public int HomeTeamId { get; set; }
    public  Team? HomeTeam { get; set; }

    public int AwayTeamId { get; set; }
    public  Team? AwayTeam { get; set; }

    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }

}