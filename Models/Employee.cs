using System.ComponentModel.DataAnnotations;


namespace StaffManagement.Models
{
    public class Employee: UserActivity
    {
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();

        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        [Required]
        public string EmployeeCode { get; set; }

        [StringLength(20)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string? MiddleName { get; set; }

        [StringLength(20)]
        [Required]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim();

        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10)]
        [Required]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        [Required]
        public string Nationality { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateOnly DateOfBirth { get; set; }

        [StringLength(300)]
        [Required]
        public string Address { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int PositionId { get; set; }
        //Set relationships*******
        //One from an Employee to one Department, and the Department is not null
        public Department Department { get; set; } = null!;

        //One from an Employee to one Position, and the Position is not null
        public Position Position { get; set; } = null!;

        public ICollection<LeaveApplication> LeaveApplications { get; set; } = new List<LeaveApplication>();

    



        public ApplicationUser? User { get; set; }
    }
}
