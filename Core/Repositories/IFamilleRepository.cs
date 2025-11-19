using Core.Dto;

namespace Core.Repositories;

public interface IFamilleRepository
{
    Task<IEnumerable<FamilleDto>> GetAllFamilles();
    Task<FamilleDto> GetFamille(int id);
    Task CreateFamille(string nom);
    Task UpdateFamille(int id, string? name);
    Task DeleteFamille(int id);
}