using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Module;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Helpers.Common
{
    public class Ultil : IUltil
    {
        private readonly JwtSettings _jwtSettings;

        public Ultil(IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
     

        public string GenerateToken(Guid id, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            if (key.Length < 32)
            {
                Array.Resize(ref key, 32);
            }
            string rolesClaim = string.Join(",", roles);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", id.ToString()),
                    new Claim("roles", rolesClaim.ToString())}),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken(Guid id, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            if (key.Length < 32)
            {
                Array.Resize(ref key, 32);
            }
            string rolesClaim = string.Join(",", roles);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[] { new Claim("id", id.ToString()) ,
                 new Claim("roles", rolesClaim.ToString())}),
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                // Different grant type for refresh token
                // You can add more claims if needed, such as token type ("refresh_token")
                Claims = new Dictionary<string, object> { { "grant_type", "refresh_token" } }
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string? GenerateTokenFromRefreshToken(string refreshToken)
        {
            var validationResult = ValidateToken(refreshToken);

            if (validationResult == (null,null))
                return null;

             Guid userId = validationResult.Item1.Value;
             List<string> roles = validationResult.Item2;

            return GenerateToken(userId, roles);
        }

        public (Guid?, List<string>) ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return (null, null);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            if (key.Length < 32)
            {
                Array.Resize(ref key, 32);
            }
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                var rolesClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "roles")?.Value;
                var roles = rolesClaim?.Split(',').ToList() ?? new List<string>();

                return (userId, roles);
            }
            catch (Exception)
            {
                return (null, null);
            }
        }

        public bool IsAccessTokenExpired(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = tokenHandler.ReadJwtToken(accessToken);

                var expDate = jwtToken.ValidTo;
                var now = DateTime.UtcNow;

                return now >= expDate;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
