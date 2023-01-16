using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace UserService.Helpers
{
    public class JwtTokenValidator
    {
        public static TokenValidationParameters CreateTokenValidationParameters()
        {
            TokenValidationParameters parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = false,
                SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                {
                    var jwt = new JwtSecurityToken(token);
                    return jwt;
                },
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
            parameters.RequireSignedTokens = false;
            return parameters;
        }
    }
}
