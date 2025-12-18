using Core.Dto;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using STIVE.Requests;

namespace STIVE.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository  _customerRepository;

    public CustomersController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet("/customers")]
    public async Task<IEnumerable<CustomerDto>> GetAll()
    {
        var customers = await _customerRepository.GetAllCustomers();
        return customers;
    }

    [HttpGet("/customers/{id:int}", Name = "GetCustomerById")]
    public async Task<CustomerDto> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetCustomer(id);
        return customer;
    }

    [HttpPost("/customers")]
    public async Task AddCustomer(AddCustomerRequest customerRequest)
    {
        await _customerRepository.CreateCustomer(
            customerRequest.FirstName,
            customerRequest.LastName,
            customerRequest.Email);
    }

    [HttpPut("/customers")]
    public async Task UpdateCustomer(UpdateCustomerRequest customerRequest)
    {
        await _customerRepository.UpdateCustomer(
            customerRequest.Id,
            customerRequest.FirstName,
            customerRequest.LastName,
            customerRequest.Email);
    }

    [HttpDelete("/customers/{id}")]
    public async Task DeleteCustomer(int id)
    {
        await _customerRepository.DeleteCustomer(id);
    }
}