using System.Security.Cryptography;
using System.Text;
using Core.Dto;
using Core.Interfaces;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace STIVE.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly StiveContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(StiveContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<CustomerDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Email))
            return BadRequest("Email is taken");

        using var hmac = new HMACSHA512();

        var customer = new CustomerEntity
        {
            Email = registerDto.Email.ToLower(),
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password))), 
            Role = "Customer"
        };
        
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return new CustomerDto
        {
            Id = customer.Id,
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {
        var customer = await _context.Customers
            .SingleOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());

        if (customer == null) return Unauthorized("Invalid username");
        
        var customerDto = new CustomerDto
        {
            Id = customer.Id,
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName
        };

        return new LoginResponseDto
        {
            User = customerDto,
            Token = _tokenService.CreateToken(customerDto)
        };
    }

    private async Task<bool> UserExists(string email)
    {
        return await _context.Customers.AnyAsync(x => x.Email == email.ToLower());
    }
}
