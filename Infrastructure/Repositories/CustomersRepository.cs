using Core.Dto;
using Core.Repositories;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomersRepository : ICustomerRepository
{
    
    private readonly StiveContext  _context;

    public CustomersRepository(StiveContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
    {
        var customers = _context.Customers;
        return await customers.Select(e => new CustomerDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email
        }).ToListAsync();
    }

    public async Task<CustomerDto> GetCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if(customer == null) throw new Exception("Customer not found");
        
        return new CustomerDto()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email
        };
    }

    public async Task CreateCustomer(string firstName, string lastName, string email)
    {
        var entity = new CustomerEntity()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };
        await _context.Customers.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCustomer(int id, string? firstName, string? lastName, string? email)
    {
        var customer = await _context.Customers.FindAsync(id);
        if(customer == null) throw new Exception("Customer not found");
        
        customer.FirstName = firstName ?? customer.FirstName;
        customer.LastName = lastName ?? customer.LastName;
        customer.Email = email ?? customer.Email;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if(customer == null) throw new Exception("Customer not found");
        
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}