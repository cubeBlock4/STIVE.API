using Core.Dto;

namespace Core.Repositories;

public interface IPanierRepository
{
    Task CreatePanier(CustomerDto customerDto);
    Task VoidPanier(PanierDto panierDto);
    Task AddProduct(PanierDto panierDto, ProductDto productDto);
    Task RemoveProduct(PanierDto panierDto, ProductDto productDto);
}