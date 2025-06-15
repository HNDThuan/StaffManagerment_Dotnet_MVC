using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using StaffManagement.contexts;
using StaffManagement.Models;

namespace StaffManagement.Data
{
    public class DataSeeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Make sure the migration has done
            await context.Database.MigrateAsync();


            List<Shift> shifts = new List<Shift>
            {
                  new Shift { Name = "Morning", StartTime = new TimeOnly(8, 0), CutoffTime = new TimeOnly(8, 30), EndTime = new TimeOnly(12, 0) },
                  new Shift { Name = "Afternoon", StartTime = new(13, 0), CutoffTime = new(13, 30), EndTime = new(17, 0) },
                  new Shift { Name = "Evening", StartTime = new(18, 0), CutoffTime = new(18, 15), EndTime = new(21, 0) }
            };

            foreach (Shift shift in shifts)
            {
                if (!await context.Shifts.AnyAsync(e => e.Name == shift.Name))
                {
                    context.Shifts.Add(shift);
                }
            }

            List<Department> departments = new List<Department>
            {
                  new Department { DepartmentCode="D-IT-001",Name = "Information Technology",Location= "unknown", Description= "..." },
                  new Department {DepartmentCode="D-MK-002", Name = "Marketing" ,Location= "unknown", Description= "..."},
                  new Department {DepartmentCode="D-HR-003", Name = "Human Resource",Location= "unknown" , Description= "..."}
            };

            foreach (Department department in departments)
            {
                if (!await context.Departments.AnyAsync(e => e.DepartmentCode == department.DepartmentCode))
                {
                    context.Departments.Add(department);
                }
            }

            List<Position> positions = new List<Position>
            {
                  new Position {PositionCode= "P-D-001", Name="Director", Description=".." },
                  new Position {PositionCode= "P-L-002", Name="Leader", Description=".." },
                  new Position {PositionCode= "P-M-003", Name="Manager", Description=".." },
                  new Position {PositionCode= "P-M-002", Name="Member", Description=".." }
            };

            foreach (Position position in positions)
            {
                if (!await context.Positions.AnyAsync(e => e.PositionCode == position.PositionCode))
                {
                    context.Positions.Add(position);
                }
            }

            await context.SaveChangesAsync();

            // Seeding some roles
            var roles = new[] { "Admin", "Manager", "Staff" };

            foreach (var role in roles)
            {
                //just create the role if it is not exist in database
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }


            // Seed the admin account
            var email = "admin@admin.com";
            var password = "123admin";
            var username = "admin";
            if (await userManager.FindByEmailAsync(email) == null && await userManager.FindByNameAsync(username) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true,

                };

                var result = await userManager.CreateAsync(admin, password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Không thể tạo tài khoản admin: {string.Join("; ", result.Errors.Select(e => e.Description))}"
                    );
                }

                result = await userManager.AddToRoleAsync(admin, "Admin");
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Không thể gán role Admin: {string.Join("; ", result.Errors.Select(e => e.Description))}"
                    );
                }
            }


            // Seed the normal account
            email = "user@user.com";
            password = "123user";
            username = "user";
            if (await userManager.FindByEmailAsync(email) == null && await userManager.FindByNameAsync(username) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true,

                };

                var result = await userManager.CreateAsync(admin, password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Không thể tạo tài khoản user: {string.Join("; ", result.Errors.Select(e => e.Description))}"
                    );
                }

                result = await userManager.AddToRoleAsync(admin, "Staff");
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Không thể gán role Staff: {string.Join("; ", result.Errors.Select(e => e.Description))}"
                    );
                }
            }
        }
    }
}
