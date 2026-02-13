using Core.Dto;

namespace Core.Interfaces;

public interface IUserService
{
    Task<CustomerDto> RegisterAsync(RegisterDto registerDto);
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<CustomerDto?> GetUserByIdAsync(int userId);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<bool> UserExistsAsync(string email);
}
