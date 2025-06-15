using Microsoft.AspNetCore.Identity;
using StaffManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace StaffManagement.ViewModels
{
    public class UserViewModel
    {
        public string? Id { get; set; }
        [Display(Name = "Email Address")]
        public string? Email { get; set; }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        public string Password { get; set; }
        [Display(Name = "Address")]
        public string? Address { get; set; }
        [Display(Name = "User Name")]
        public string? UserName { get; set; }
        [Display(Name = "National ID")]
        public string? NationalId { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? FullName
        {
            get
            {
                return $"{FirstName} {MiddleName} {LastName}";
            }
        }

        [Display(Name = "User Role")]
        public IEnumerable<IdentityRole> Roles { get; set; } = [];

        public Employee? Employee { get; set; }
    }
}
