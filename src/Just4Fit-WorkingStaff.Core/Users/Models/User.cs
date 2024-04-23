using Microsoft.AspNetCore.Identity;

namespace Just4Fit_WorkingStaff.Users.Models;

public class User : IdentityUser
{
    public int? Age { get; set; }
    public string? Surname { get; set; }
}