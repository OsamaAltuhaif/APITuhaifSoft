using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuhaifSoftAPI.Models
{
    [Table("Users")] // هذا الربط يجعل EF يعرف أن الكيان مرتبط بالجدول
    public class Users:IdentityUser
    {
        //[Key]
        [Column("Id")]
        public string Id { get; set; }

        [MaxLength(100)]
        [Column("UserName")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("Email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("Password")]
        public string Password { get; set; }




        // أضف أي أعمدة إضافية حسب الحاجة
    }
}
