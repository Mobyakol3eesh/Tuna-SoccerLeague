using MySqlConnector;

internal static class AppConfiguration
{
    internal static string BuildConnectionString(IConfiguration configuration)
    {
        var baseConnectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        var connBuilder = new MySqlConnectionStringBuilder(baseConnectionString)
        {
            Password = password
        };

        return connBuilder.ConnectionString;
    }

    internal static JwtConfig LoadJwtConfig(IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured.");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT audience is not configured.");

        return new JwtConfig(key, issuer, audience);
    }
}

internal sealed record JwtConfig(string Key, string Issuer, string Audience);