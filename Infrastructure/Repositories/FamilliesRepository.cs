using Core.Dto;
using Core.Repositories;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FamilliesRepository : IFamilleRepository
{
    
    private readonly StiveContext  _context;

    public FamilliesRepository(StiveContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<FamilleDto>> GetAllFamilles()
    {
        var familles = _context.Familles;
        return await familles.Select(e => new FamilleDto
        {
            Id = e.Id,
            Name = e.Name
        }).ToListAsync();
    }

    public async Task<FamilleDto> GetFamille(int id)
    {
        var famille = await _context.Familles.FindAsync(id);
        if(famille == null) throw new Exception("Famille not found");
        
        return new FamilleDto()
        {
            Id = famille.Id,
            Name = famille.Name
        };
    }

    public async Task CreateFamille(string name)
    {
        var entity = new FamilleEntity()
        {
            Name = name
        };
        await _context.Familles.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFamille(int id, string? name)
    {
        var famille = await _context.Familles.FindAsync(id);
        if(famille == null) throw new Exception("Famille not found");
        
        famille.Name = name ?? famille.Name;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFamille(int id)
    {
        var famille = await _context.Familles.FindAsync(id);
        if(famille == null) throw new Exception("Famille not found");
        
        _context.Familles.Remove(famille);
        await _context.SaveChangesAsync();
    }
}