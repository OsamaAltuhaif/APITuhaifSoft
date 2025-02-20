using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace TuhaifSoftAPI.NewFolder
{
    public class Authintication
    {
       
            
        public static JwtSecurityToken isitAuthoriz(string UserName,string Password, IConfiguration _configuration)
        {
           /* if ( db.Users.FirstOrDefault(us => us.Email == UserName && us.Password == Password) != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

                var Token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(2),
                    signingCredentials: creds);
                    Models.TokenUser.token = Token;
                
            }*/
            return null;
        }
    }
}
