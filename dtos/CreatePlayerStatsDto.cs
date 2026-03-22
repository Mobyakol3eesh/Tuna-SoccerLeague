using System.ComponentModel.DataAnnotations;

public class CreatePlayerStatsDto
{
    [Range(0, int.MaxValue, ErrorMessage = "Goals must be a non-negative integer.")]
    public int Goals { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Assists must be a non-negative integer.")]
    public int Assists { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Appearances must be a non-negative integer.")]
    public int Appearances { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "PlayerId must be a positive integer.")]
    public int PlayerId { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "MatchId must be a positive integer.")]
    public int MatchId { get; set; }
}