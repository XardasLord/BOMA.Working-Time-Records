using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiBaseController
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        
        if (result.Succeeded)
            return Ok();
        
        return BadRequest(result.Errors);
    }
    
    [HttpPost("user/roles")]
    public async Task<IActionResult> AssignRole(string roleName, string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        
        if (user is null)
            return BadRequest($"User with email '{userEmail}' does not exist.");
        
        var role = await _roleManager.FindByNameAsync(roleName);
        
        if (role is null)
            return BadRequest($"Role '{roleName}' does not exist.");

        var result = await _userManager.AddToRoleAsync(user, roleName);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok();
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        
        return Ok(roles);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyDetails()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
            return BadRequest("User is not logged in");

        var userRoles = await _userManager.GetRolesAsync(user);

        var role = userRoles.FirstOrDefault() ?? "User";
        
        return Ok(new UserDetails
        {
            Id = user.Id,
            Email = user.Email,
            Role = role,
            Activated = user.EmailConfirmed
        });
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        var results = new List<UserDetails>();
        
        foreach (var user in users)
        {
            results.Add(new UserDetails
            {
                Id = user.Id,
                Email = user.Email,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User",
                Activated = user.EmailConfirmed
            });
        }
        
        return Ok(results);
    }

    [HttpPatch("users/{id}/activation")]
    public async Task<IActionResult> GetUsers(Guid id, UserActivationStatus userActivationStatus)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        
        if (user is null)
            return BadRequest($"User with ID '{id}' does not exist.");

        user.EmailConfirmed = userActivationStatus == UserActivationStatus.Activate;

        await _userManager.UpdateAsync(user);
        
        return Ok();
    }
}

public enum UserActivationStatus
{
    Deactivate = 0,
    Activate = 1,
}

public class UserDetails
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool Activated { get; set; }
}