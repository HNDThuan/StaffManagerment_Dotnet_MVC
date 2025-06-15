using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
