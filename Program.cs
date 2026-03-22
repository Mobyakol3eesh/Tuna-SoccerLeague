using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

var baseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connBuilder = new MySqlConnector.MySqlConnectionStringBuilder(baseConnectionString ?? "")
{
	Password = password
};

var connectionString = connBuilder.ConnectionString;

builder.Services.AddDbContext<TunaLeagueContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Tuna League API",
		Version = "v1",
		Description = "API for teams, players, coaches, matches, and player stats."
	});
});

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ICoachService, CoachService>();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tuna League API v1");
});




app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Team}/{action=Index}/{id?}");
app.MapControllers();


app.Run();
