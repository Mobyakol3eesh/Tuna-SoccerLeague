using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration configuration;

    public AuthController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var role = ValidateUser(dto.Username, dto.Password);
        if (role == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = GenerateToken(dto.Username, role);
        return Ok(new
        {
            accessToken = token,
            role
        });
    }

    private string? ValidateUser(string username, string password)
    {
        if (username == "admin" && password == "admin123")
        {
            return "Admin";
        }

        if (username == "user" && password == "user123")
        {
            return "User";
        }

        return null;
    }

    private string GenerateToken(string username, string role)
    {
        var jwt = configuration.GetSection("Jwt");
        var key = jwt["Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
        var issuer = jwt["Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured.");
        var audience = jwt["Audience"] ?? throw new InvalidOperationException("JWT audience is not configured.");
        var expiryMinutes = int.TryParse(jwt["ExpiryMinutes"], out var minutes) ? minutes : 120;

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}