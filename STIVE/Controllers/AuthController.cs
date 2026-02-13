using Core.Dto;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace STIVE.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<CustomerDto>> Register(RegisterDto registerDto)
    {
        if (await _userService.UserExistsAsync(registerDto.Email))
            return BadRequest("Email is taken");

        var customer = await _userService.RegisterAsync(registerDto);
        return customer;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            var response = await _userService.LoginAsync(loginDto);
            return response;
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CustomerDto>> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst("id")?.Value;

        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var customer = await _userService.GetUserByIdAsync(userId);

        if (customer == null) return NotFound();

        return customer;
    }

    [Authorize]
    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var result = await _userService.ResetPasswordAsync(resetPasswordDto);

        if (!result)
            return BadRequest("Invalid email or current password");

        return Ok("Password has been reset successfully");
    }
}
