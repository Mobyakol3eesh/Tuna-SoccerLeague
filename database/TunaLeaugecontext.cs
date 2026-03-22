using Microsoft.EntityFrameworkCore;

public class TunaLeagueContext : DbContext {
    public TunaLeagueContext(DbContextOptions<TunaLeagueContext> options) : base(options) { }
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }

    public DbSet<Coach> Coaches { get; set; }

    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchTeam> MatchTeams { get; set; }
    
    public DbSet<PlayerStats> PlayerStats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MatchTeam>()
            .HasOne(mt => mt.Match)
            .WithMany()
            .HasForeignKey(mt => mt.MatchId);

        modelBuilder.Entity<MatchTeam>()
            .HasOne(mt => mt.HomeTeam)
            .WithMany()
            .HasForeignKey(mt => mt.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MatchTeam>()
            .HasOne(mt => mt.AwayTeam)
            .WithMany()
            .HasForeignKey(mt => mt.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}