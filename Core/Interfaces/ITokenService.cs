using Core.Dto;

namespace Core.Interfaces;

public interface ITokenService
{
    string CreateToken(CustomerDto user);
}
