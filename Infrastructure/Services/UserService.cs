using System.Security.Cryptography;
using System.Text;
using Core.Dto;
using Core.Interfaces;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly StiveContext _context;
    private readonly ITokenService _tokenService;

    public UserService(StiveContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<CustomerDto> RegisterAsync(RegisterDto registerDto)
    {
        using var hmac = new HMACSHA512();

        var customer = new CustomerEntity
        {
            Email = registerDto.Email.ToLower(),
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password))),
            PasswordSalt = hmac.Key,
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

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var customer = await _context.Customers
            .SingleOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());

        if (customer == null)
            throw new UnauthorizedAccessException("Invalid username");

        using var hmac = new HMACSHA512(customer.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        var computedHashString = Convert.ToBase64String(computedHash);

        if (computedHashString != customer.PasswordHash)
            throw new UnauthorizedAccessException("Invalid password");

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

    public async Task<CustomerDto?> GetUserByIdAsync(int userId)
    {
        var customer = await _context.Customers.FindAsync(userId);

        if (customer == null) return null;

        return new CustomerDto
        {
            Id = customer.Id,
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName
        };
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var customer = await _context.Customers
            .SingleOrDefaultAsync(x => x.Email == resetPasswordDto.Email.ToLower());

        if (customer == null) return false;

        // Verify current password
        using var hmacVerify = new HMACSHA512(customer.PasswordSalt);
        var computedHash = hmacVerify.ComputeHash(Encoding.UTF8.GetBytes(resetPasswordDto.CurrentPassword));
        var computedHashString = Convert.ToBase64String(computedHash);

        if (computedHashString != customer.PasswordHash) return false;

        // Set new password
        using var hmacNew = new HMACSHA512();
        customer.PasswordHash = Convert.ToBase64String(hmacNew.ComputeHash(Encoding.UTF8.GetBytes(resetPasswordDto.NewPassword)));
        customer.PasswordSalt = hmacNew.Key;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _context.Customers.AnyAsync(x => x.Email == email.ToLower());
    }
}
