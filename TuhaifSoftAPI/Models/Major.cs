using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuhaifSoftAPI.Models
{
    
    public class Major
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Major_id { get; set; }
        [Required]
        [MaxLength(100)]
        public string name { get; set; }

    }
}
