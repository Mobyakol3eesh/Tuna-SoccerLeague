public class PlayerStatsReadDto
{
    public int Id { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int Appearances { get; set; }

    public string PlayerName { get; set; } = string.Empty;
    public DateTime MatchDate { get; set; }
    public string MatchLocation { get; set; } = string.Empty;
}