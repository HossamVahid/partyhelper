using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace party_helperbe.Common.Utils
{
    public class TokenHelper
    {
        public static string GenerateJwtToken(Claim[] claim)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(

                issuer: Environment.GetEnvironmentVariable("ISSUER"),
                audience: Environment.GetEnvironmentVariable("AUDIENCE"),
                claims: claim,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials

                );

           var tokenHandler=new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(tokenDescriptor);


        }
    }
}
