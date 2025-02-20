
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuhaifSoftAPI.Models
{
    [Table("Students")]
    public class Students
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string f_Name { get; set; }
        public string l_Name { get; set; }
        
        public int Major_id { get; set; }
        public int Age { get; set; }
        public DateTime birthDate { get; set; }
        public string imageURl { get; set; }
        

    }
}
