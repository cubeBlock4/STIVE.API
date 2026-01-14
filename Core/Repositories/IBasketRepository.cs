using Core.Dto;

namespace Core.Repositories;

public interface IBasketRepository
{
    Task<IEnumerable<BasketDto>> GetAllBaskets();
    Task<BasketDto> GetBasket(int id);
    Task CreateBasket(int customerId, int productId, int quantity);
    Task UpdateBasket(int id, int customerId, int productId, int quantity);
    Task DeleteBasket(int id);

    // User-centric methods
    Task<BasketDto> GetBasketByCustomerId(int customerId);
    Task AddItemToBasket(int customerId, int productId, int quantity);
    Task UpdateItemInBasket(int customerId, int productId, int quantity);
    Task RemoveItemFromBasket(int customerId, int productId);
    Task ClearBasket(int customerId);
}