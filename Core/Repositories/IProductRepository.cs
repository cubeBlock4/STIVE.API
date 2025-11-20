using Core.Dto;

namespace Core.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetAllProducts();
    Task<ProductDto> GetProduct(int id);
    Task CreateProduct(string name, string reference, string price, int familleId, int supplierId);
    Task UpdateProduct(int id, string? name, string? reference, string? price);
    Task DeleteProduct(int id);
}