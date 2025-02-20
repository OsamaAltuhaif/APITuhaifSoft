using System.ComponentModel.DataAnnotations;

namespace TuhaifSoftAPI.Models.DTO
{
    public class UsersDTO
    {
        [Key]
        
        public int Id { get; set; }
        public string UserName { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
