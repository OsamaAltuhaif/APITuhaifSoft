using System.IdentityModel.Tokens.Jwt;

namespace TuhaifSoftAPI.Models
{
    public class TokenUser
    {
        public static JwtSecurityToken token
        {
            get; set;
        }
    }
}
