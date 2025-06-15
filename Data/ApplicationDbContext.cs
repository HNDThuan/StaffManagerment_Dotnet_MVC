using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Models;

namespace StaffManagement.contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<SystemCode> SystemCodes { get; set; }
        public DbSet<SystemCodeDetail> SystemCodeDetails { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceToken> AttendanceTokens { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<EmployeeShift> EmployeeShifts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //set Primary Key
            modelBuilder.Entity<EmployeeShift>()
                .HasKey(e => new { e.ShiftId, e.EmployeeId, e.StartDate });

            //Set relationship


            // === ApplicationUser – Employee (1:0..1)
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Employee)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // === Shift – AttendanceToken (1:N)
            modelBuilder.Entity<AttendanceToken>()
                .HasOne(t => t.Shift)
                .WithMany(s => s.AttendanceTokens)
                .HasForeignKey(t => t.ShiftId);

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Position>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Position)
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            // === Employee - Attendance (1:N)
            modelBuilder.Entity<Attendance>()
                .HasOne(at => at.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(at => at.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // === Shift - Attendance (1:N)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Shift)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.ShiftId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeShift>()
               .HasOne(es => es.Employee)
               .WithMany(e => e.EmployeeShifts)
               .HasForeignKey(es => es.EmployeeId);

            modelBuilder.Entity<EmployeeShift>()
                .HasOne(es => es.Shift)
                .WithMany(s => s.EmployeeShifts)
                .HasForeignKey(es => es.ShiftId);


            modelBuilder.Entity<LeaveApplication>()
                .HasOne(la => la.Status)
                .WithMany(e => e.LeaveApplications)
                .HasForeignKey(la => la.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SystemCodeDetail>()
               .HasOne(scd => scd.SystemCode)
               .WithMany(sc => sc.SystemCodeDetails)
               .HasForeignKey(scd => scd.SystemCodeId);

            modelBuilder.Entity<City>()
               .HasOne(ct => ct.Country)
               .WithMany(c => c.Cities)
               .HasForeignKey(ct => ct.CountryId);

            modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<string>>(
                j => j.HasOne<IdentityRole>()
                       .WithMany()
                       .HasForeignKey(ur => ur.RoleId)
                       .HasConstraintName("FK_UserRoles_RoleId"),
                j => j.HasOne<ApplicationUser>()
                       .WithMany()
                       .HasForeignKey(ur => ur.UserId)
                       .HasConstraintName("FK_UserRoles_UserId"),
                j => j.ToTable("AspNetUserRoles"));


            //Set indices
            modelBuilder.Entity<Employee>().HasIndex(e => e.PhoneNumber).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.EmployeeCode).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();

            modelBuilder.Entity<Department>().HasIndex(d => d.DepartmentCode).IsUnique();

            modelBuilder.Entity<Position>().HasIndex(p => p.PositionCode).IsUnique();

            modelBuilder.Entity<Bank>().HasIndex(b => b.Code).IsUnique();
            modelBuilder.Entity<Bank>().HasIndex(b => b.AccountNumber).IsUnique();

            modelBuilder.Entity<SystemCode>().HasIndex(sc => sc.Code).IsUnique();

            modelBuilder.Entity<SystemCodeDetail>().HasIndex(scd => scd.Code).IsUnique();

            modelBuilder.Entity<LeaveType>().HasIndex(lt => lt.Code).IsUnique();

            modelBuilder.Entity<Country>().HasIndex(c => c.Code).IsUnique();

            modelBuilder.Entity<City>().HasIndex(ct => ct.Code).IsUnique();

            modelBuilder.Entity<Attendance>().HasIndex(a => new { a.EmployeeId, a.ShiftId, a.Date }).IsUnique();

            modelBuilder.Entity<AttendanceToken>()
              .HasIndex(t => t.ExpiresAt);
        }
    }
}
