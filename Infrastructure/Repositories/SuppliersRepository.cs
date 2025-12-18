using Core.Dto;
using Core.Repositories;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SuppliersRepository : ISupplierRepository
{
    
    private readonly StiveContext  _context;

    public SuppliersRepository(StiveContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<SupplierDto>> GetAllSuppliers()
    {
        var suppliers = _context.Suppliers;
        return await suppliers.Select(e => new SupplierDto
        {
            Id = e.Id,
            EntrepriseName = e.EntrepriseName,
            Email = e.Email,
            Phone = e.Phone
        }).ToListAsync();
    }

    public async Task<SupplierDto> GetSupplier(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if(supplier == null) throw new Exception("Supplier not found");
        
        return new SupplierDto()
        {
            Id = supplier.Id,
            EntrepriseName = supplier.EntrepriseName,
            Email = supplier.Email,
            Phone = supplier.Phone
        };
    }

    public async Task CreateSupplier(string entrepriseName, string email, string phone)
    {
        var entity = new SupplierEntity()
        {
            EntrepriseName = entrepriseName,
            Email = email,
            Phone = phone
        };
        await _context.Suppliers.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSupplier(int id, string? entrepriseName, string? email, string? phone)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if(supplier == null) throw new Exception("Supplier not found");
        
        supplier.EntrepriseName = entrepriseName ?? supplier.EntrepriseName;
        supplier.Email = email ?? supplier.Email;
        supplier.Phone = phone ?? supplier.Phone;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSupplier(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if(supplier == null) throw new Exception("Supplier not found");
        
        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
    }
}