using Core.Dto;
using Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STIVE.Requests;

namespace STIVE.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository  _basketRepository;

    public BasketController(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    [HttpGet("/basket")]
    public async Task<BasketDto> GetMyBasket()
    {
        var customerId = GetCurrentCustomerId();
        var basket = await _basketRepository.GetBasketByCustomerId(customerId);
        return basket;
    }

    [HttpPost("/basket/items")]
    public async Task AddItemToBasket([FromBody] AddBasketItemRequest request)
    {
        var customerId = GetCurrentCustomerId();
        await _basketRepository.AddItemToBasket(customerId, request.ProductId, request.Quantity);
    }

    [HttpPut("/basket/items/{productId}")]
    public async Task UpdateItemInBasket(int productId, [FromBody] UpdateBasketItemRequest request)
    {
        var customerId = GetCurrentCustomerId();
        await _basketRepository.UpdateItemInBasket(customerId, productId, request.Quantity);
    }

    [HttpDelete("/basket/items/{productId}")]
    public async Task RemoveItemFromBasket(int productId)
    {
        var customerId = GetCurrentCustomerId();
        await _basketRepository.RemoveItemFromBasket(customerId, productId);
    }

    [HttpDelete("/basket")]
    public async Task ClearBasket()
    {
        var customerId = GetCurrentCustomerId();
        await _basketRepository.ClearBasket(customerId);
    }
    
    private int GetCurrentCustomerId()
    {
        var idClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
        if (idClaim == null) throw new Exception("User ID not found in token");
        return int.Parse(idClaim.Value);
    }
}
// DTOs for Requests (Inner classes or separate? Separate is better but for speed I can put them here or use existing if compatible)
// The existing requests were: AddBasketRequest (All fields), UpdateBasketRequest.
// I need simpler requests only for ProductId/Quantity.
// Let's create new Request classes in the same file for now or in Requests namespace if I can multiple tool call. 
// I'll create them in a separate tool call to be clean.