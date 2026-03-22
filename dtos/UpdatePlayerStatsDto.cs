using System.ComponentModel.DataAnnotations;

public class UpdatePlayerStatsDto
{
    [Range(0, int.MaxValue, ErrorMessage = "Goals must be a non-negative integer.")]
    public int Goals { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Assists must be a non-negative integer.")]
    public int Assists { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Appearances must be a non-negative integer.")]
    public int Appearances { get; set; }
}