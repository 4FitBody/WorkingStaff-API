namespace Just4Fit_WorkingStaff.Presentation.Users.Dtos;

using Just4Fit_WorkingStaff.Presentation.Roles.Models;

public class LoginDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public DefaultRoles Role { get; set; }
}