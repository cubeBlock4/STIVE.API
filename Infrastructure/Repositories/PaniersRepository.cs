using Core.Dto;
using Core.Repositories;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaniersRepository : IPanierRepository
{
    
    private readonly StiveContext  _context;

    public PaniersRepository(StiveContext context)
    {
        _context = context;
    }

    public async Task CreatePanier(CustomerDto customerDto)
    {
        var entity = new PanierEntity()
        {
            CustomerId = customerDto.Id,
            Customer = new CustomerEntity()
            {
                Id = customerDto.Id,
                Email = customerDto.Email,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
            }
        };
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task VoidPanier(PanierDto panierDto)
    {
        panierDto.Products = null;
        await _context.SaveChangesAsync();
    }

    public async Task AddProduct(PanierDto panierDto, ProductDto productDto)
    {
        panierDto.Products.Append(productDto);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveProduct(PanierDto panierDto, ProductDto productDto)
    {
        panierDto.Products = panierDto.Products.Where(p => p.Id != productDto.Id);
        await _context.SaveChangesAsync();
    }
}