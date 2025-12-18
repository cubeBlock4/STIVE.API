namespace Core.Dto;

public class LoginResponseDto
{
    public CustomerDto User { get; set; }
    public string Token { get; set; }
}
