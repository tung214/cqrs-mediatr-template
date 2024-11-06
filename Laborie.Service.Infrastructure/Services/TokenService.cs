using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Laborie.Service.Infrastructure.Services;
public class TokenService(ILogger<TokenService> logger, IConfiguration configuration)
: ITokenService
{
    public string GenerateToken(LaborieAgency agency, string deviceToken)
    {
        try
        {
            List<Claim> claims =
            [
                new Claim("sub", agency.Id),
                new Claim("device", deviceToken),
            ];
            if (!string.IsNullOrEmpty(agency.Name)) claims.Add(new Claim("name", agency.Name));

#pragma warning disable CS8604 // Possible null reference argument.
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                            configuration.GetSection("Authentication:SecretKey").Value));
#pragma warning restore CS8604 // Possible null reference argument.
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var token = new JwtSecurityToken(
                                   claims: claims,
                                   expires: DateTime.UtcNow.AddDays(1),
                                   signingCredentials: cred
                    );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Error GenerateToken {message}", ex.Message);
            throw;
        }
    }
}
