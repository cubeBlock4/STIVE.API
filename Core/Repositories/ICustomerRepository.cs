using Core.Dto;

namespace Core.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<CustomerDto>>  GetAllCustomers();
    Task<CustomerDto> GetCustomer(int id);
    Task CreateCustomer(string firstName, string lastName,  string email);
    Task UpdateCustomer(int Id, string? firstName, string? lastName, string? email);
    Task DeleteCustomer(int id);
}