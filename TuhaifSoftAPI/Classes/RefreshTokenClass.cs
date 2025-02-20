using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TuhaifSoftAPI.Classes
{
    public class RefreshTokenClass
    {
        public static string GeneratorRfreshToken(IConfiguration _configuration,string UserName,string SecretKey)
        {
           
            var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub,UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())


                };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[SecretKey]));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken(
                   issuer: _configuration["JwtSettings:Issuer"],
                   audience: _configuration["JwtSettings:Audience"],
                   claims: claims,
                   expires: DateTime.Now.AddMinutes(2),
                   signingCredentials: creds);
            var refreshToken = new JwtSecurityTokenHandler().WriteToken(Token);
            return refreshToken.ToString();

        }
    }
}
