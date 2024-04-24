namespace Just4Fit_WorkingStaff.Presentation.Users.Dtos;

using Just4Fit_WorkingStaff.Presentation.Roles.Models;

public class RegistrationDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int? Age { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public DefaultRoles Role { get; set; }
}