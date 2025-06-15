using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } 
        
        [Required]
        public string Name { get; set; }

        public int CountryId { get; set; }

        public Country? Country { get; set; } = null!;
    }
}
