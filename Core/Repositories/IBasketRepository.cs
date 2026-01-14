using Core.Dto;

namespace Core.Repositories;

public interface IBasketRepository
{
    Task<IEnumerable<BasketDto>> GetAllBaskets();
    Task<BasketDto> GetBasket(int id);
    Task CreateBasket(int customerId, int productId, int quantity);
    Task UpdateBasket(int id, int customerId, int productId, int quantity);
    Task DeleteBasket(int id);
}