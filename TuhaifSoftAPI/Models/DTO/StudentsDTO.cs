using System.ComponentModel.DataAnnotations;

namespace TuhaifSoftAPI.Models.DTO
{
    public class StudentsDTO
    {
        
        public int id { get; set; }
        [Required]
        [MaxLength(30)]
        public string f_Name { get; set; }
        public string l_Name { get; set; }
        public int Major_id { get; set; }
        public int Age { get; set; }
        public DateTime birthDate { get; set; }
        public string imageURl { get; set; }

    }
}
