using Core.Dto;
using Core.Repositories;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductsRepository : IProductRepository
{
    private readonly StiveContext _context;

    public ProductsRepository(StiveContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        var products = _context.Products;
        return await products
            .Include(p => p.Famille)
            .Include(p => p.Supplier)
            .Select(e => new ProductDto
            {
                Id = e.Id,
                Name = e.Name,
                Reference = e.Reference,
                Price = e.Price,
                Famille = new FamilleDto()
                {
                    Id = e.Famille.Id,
                    Name = e.Famille.Name,
                },
                Supplier = new  SupplierDto()
                {
                    Id = e.Supplier.Id,
                    Email = e.Supplier.Email,
                    EntrepriseName = e.Supplier.EntrepriseName,
                    Phone = e.Supplier.Phone
                },
            }).ToListAsync();
    }

    public async Task<ProductDto> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) throw new Exception("Product not found");

        return new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            Reference = product.Reference,
            Price = product.Price,
            Famille = new FamilleDto()
            {
                Id = product.Famille.Id,
                Name = product.Famille.Name
            },
            Supplier = new SupplierDto()
            {
                Id = product.Supplier.Id,
                Email = product.Supplier.Email,
                EntrepriseName = product.Supplier.EntrepriseName,
                Phone = product.Supplier.Phone
            }
        };
    }

    public async Task CreateProduct(string name, string reference, string price, int familleId, int supplierId)
    {
        var famille = await _context.Familles.FindAsync(familleId);
        if (famille == null) throw new Exception("Famille not found");

        var supplier = await _context.Suppliers.FindAsync(supplierId);
        if (supplier == null) throw new Exception("Supplier not found");

        var entity = new ProductEntity()
        {
            Name = name,
            Reference = reference,
            Price = price,
            Famille = famille,
            FamilleId = familleId,
            SupplierId = supplierId,
            Supplier = supplier
        };
        await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProduct(int id, string? name, string? reference, string? price)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) throw new Exception("Product not found");

        product.Name = name ?? product.Name;
        product.Reference = reference ?? product.Reference;
        product.Price = price ?? product.Price;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) throw new Exception("Product not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}