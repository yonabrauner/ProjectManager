using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProjectManager.Api.Helpers
{
    public static class JwtHelper
    {
        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            var key = configuration["Jwt:Key"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                ValidateLifetime = false,
                // ClockSkew = TimeSpan.Zero
            };
        }
    }
}