namespace Just4Fit_WorkingStaff.Infrastructure.Data;

using Just4Fit_WorkingStaff.Core.Exercises.Models;
using Just4Fit_WorkingStaff.Core.News.Models;
using Just4Fit_WorkingStaff.Core.Food.Models;
using Just4Fit_WorkingStaff.Core.SportSupplements.Models;
using Just4Fit_WorkingStaff.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class JustForFitWorkingStaffDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Food> Food { get; set; }
    public DbSet<SportSupplement> SportSupplements { get; set; }

    public JustForFitWorkingStaffDbContext(DbContextOptions<JustForFitWorkingStaffDbContext> options) : base(options) { }
}