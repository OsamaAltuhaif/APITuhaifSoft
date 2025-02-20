using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuhaifSoftAPI.Models
{
    [Table("RefreshTokens")]
    public class RefreshTokens
    {
        [Key]
        public int id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
