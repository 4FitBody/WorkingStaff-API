namespace Just4Fit_WorkingStaff.Presentation.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Just4Fit_WorkingStaff.Core.Tokens.Models;
using Just4Fit_WorkingStaff.Infrastructure.Data;
using Just4Fit_WorkingStaff.Presentation.Options;
using Just4Fit_WorkingStaff.Presentation.Tokens.Dtos;
using Just4Fit_WorkingStaff.Presentation.Users.Dtos;
using Just4Fit_WorkingStaff.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]/[action]")]
public class IdentityController : ControllerBase
{
    private readonly JwtOptions jwtOptions;
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;
    private readonly JustForFitWorkingStaffDbContext dbContext;

    public IdentityController(
        IOptionsSnapshot<JwtOptions> jwtOptions,
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        JustForFitWorkingStaffDbContext dbContext)
    {
        this.signInManager = signInManager;

        this.userManager = userManager;

        this.dbContext = dbContext;

        this.jwtOptions = jwtOptions.Value;
    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        var user = await this.userManager.FindByEmailAsync(loginDto.Email!);

        if (user is null)
        {
            return base.BadRequest(new
            {
                Message = "Incorrect email or password!",
            });
        }

        var signInResult = await this.signInManager.PasswordSignInAsync(user, loginDto.Password!, false, true);

        if (signInResult.IsLockedOut)
        {
            return base.BadRequest(new
            {
                Message = "Account was locked! Try later.",
            });
        }

        if (signInResult.Succeeded == false)
        {
            return base.BadRequest(new
            {
                Message = "Incorrect login or password!",
            });
        }

        var roles = await userManager.GetRolesAsync(user);

        var claims = roles
            .Select(role => new Claim(ClaimTypes.Role, role))
            .Append(new Claim(ClaimTypes.Email, loginDto.Email!))
            .Append(new Claim(ClaimTypes.Name, user.UserName!))
            .Append(new Claim(ClaimTypes.Surname, user.Surname!))
            .Append(new Claim("Age", user.Age.ToString()!))
            .Append(new Claim(ClaimTypes.NameIdentifier, user.Id));

        var securityKey = new SymmetricSecurityKey(this.jwtOptions.KeyInBytes);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: this.jwtOptions.Issuers.First(),
            audience: this.jwtOptions.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(this.jwtOptions.LifetimeInMinutes),
            signingCredentials: signingCredentials
        );

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        var jwt = jwtSecurityTokenHandler.WriteToken(securityToken);

        var refreshToken = new RefreshToken()
        {
            Token = Guid.NewGuid(),
            UserId = user.Id
        };

        await this.dbContext.RefreshTokens.AddAsync(refreshToken);

        await this.dbContext.SaveChangesAsync();

        return base.Ok(
            new
            {
                accessToken = jwt,
                refreshToken = refreshToken.Token
            }
        );
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegistrationAsync(RegistrationDto registrationDto)
    {
        var user = await this.userManager.FindByEmailAsync(registrationDto.Email!);

        if (user is not null)
        {
            return base.BadRequest($"Email {registrationDto.Email} is in use!");
        }

        var newUser = new User
        {
            UserName = registrationDto.Name,
            Surname = registrationDto.Surname,
            Age = registrationDto.Age,
            Email = registrationDto.Email,
        };

        var signUpResult = await this.userManager.CreateAsync(newUser, registrationDto.Password!);

        if (signUpResult.Succeeded == false)
        {
            return base.BadRequest(signUpResult.Errors.Select(error => error.Description));
        }

        var roleResult = await userManager.AddToRoleAsync(newUser, registrationDto.Role.ToString());

        return base.Created(base.HttpContext.Request.GetDisplayUrl(), null);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTokenAsync(UpdateTokenDto updateTokenDto)
    {
        var handler = new JwtSecurityTokenHandler();

        var validationResult = await handler.ValidateTokenAsync(
            updateTokenDto.AccessToken,
            new TokenValidationParameters()
            {
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(this.jwtOptions.KeyInBytes),

                ValidateAudience = true,
                ValidAudience = this.jwtOptions.Audience,

                ValidateIssuer = true,
                ValidIssuers = this.jwtOptions.Issuers,
            }
        );

        if (validationResult.IsValid == false)
        {
            return base.BadRequest("Token is invalid!");
        }

        var securityToken = handler.ReadJwtToken(updateTokenDto.AccessToken);

        var idClaim = securityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (idClaim == null)
        {
            return base.BadRequest("JWT Token must contain 'id' claim!");
        }

        var id = idClaim.Value;

        var user = await this.userManager.FindByIdAsync(id);

        if (user == null)
        {
            return base.NotFound($"Couldn't update the token. User with id '{id}' doesn't exist!");
        }

        var roles = await userManager.GetRolesAsync(user);

        var claims = roles
            .Select(role => new Claim(ClaimTypes.Role, role))
            .Append(new Claim(ClaimTypes.Email, user.Email ?? "notset"))
            .Append(new Claim(ClaimTypes.Name, user.UserName!))
            .Append(new Claim(ClaimTypes.Surname, user.Surname!))
            .Append(new Claim("Age", user.Age.ToString()!))
            .Append(new Claim(ClaimTypes.NameIdentifier, user.Id));

        var securityKey = new SymmetricSecurityKey(this.jwtOptions.KeyInBytes);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var newSecurityToken = new JwtSecurityToken(
            issuer: this.jwtOptions.Issuers.First(),
            audience: this.jwtOptions.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(this.jwtOptions.LifetimeInMinutes),
            signingCredentials: signingCredentials
        );

        var newJwt = handler.WriteToken(newSecurityToken);

        var refreshTokenToChange = this.dbContext.RefreshTokens.FirstOrDefault(refreshToken => refreshToken.Token == updateTokenDto.RefreshToken && refreshToken.UserId == id);

        if (refreshTokenToChange == null)
        {
            var refreshTokensToDelete = await this.dbContext.RefreshTokens.Where(rf => rf.UserId == id)
                .ToListAsync();

            this.dbContext.RefreshTokens.RemoveRange(refreshTokensToDelete);

            await this.dbContext.SaveChangesAsync();

            return base.BadRequest($"Refresh token '{updateTokenDto.RefreshToken}' doesn't exist for userid '{id}'");
        }

        refreshTokenToChange.Token = Guid.NewGuid();

        await this.dbContext.SaveChangesAsync();

        return base.Ok(new
        {
            accessToken = newJwt,
            refreshToken = refreshTokenToChange.Token
        });
    }
}