using Core.Dto;

namespace Core.Repositories;

public interface ISupplierRepository
{
    Task<IEnumerable<SupplierDto>> GetAllSuppliers();
    Task<SupplierDto> GetSupplier(int id);
    Task CreateSupplier(string entrepriseName, string email, string phone);
    Task UpdateSupplier(int id, string? entrepriseName, string? email, string? phone);
    Task DeleteSupplier(int id);
}