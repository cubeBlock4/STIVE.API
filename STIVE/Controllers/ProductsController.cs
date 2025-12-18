using Core.Dto;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using STIVE.Requests.Products;

namespace STIVE.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository  _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet("/products")]
    public async Task<IEnumerable<ProductDto>> GetAll()
    {
        var products = await _productRepository.GetAllProducts();
        return products;
    }

    [HttpGet("/products/{id:int}", Name = "GetProductById")]
    public async Task<ProductDto> GetProductById(int id)
    {
        var product = await _productRepository.GetProduct(id);
        return product;
    }

    [HttpPost("/products")]
    public async Task AddProduct(AddProductRequest productRequest)
    {
        await _productRepository.CreateProduct(
            productRequest.Name,
            productRequest.Reference,
            productRequest.Price,
            productRequest.FamilleId,
            productRequest.SupplierId);
    }

    [HttpPut("/products")]
    public async Task UpdateProduct(UpdateProductRequest productRequest)
    {
        await _productRepository.UpdateProduct(
            productRequest.Id,
            productRequest.Name,
            productRequest.Reference,
            productRequest.Price);
    }

    [HttpDelete("/products/{id}")]
    public async Task DeleteProduct(int id)
    {
        await _productRepository.DeleteProduct(id);
    }
}