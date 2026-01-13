using Core.Dto;
using Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STIVE.Requests.Suppliers;

namespace STIVE.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierRepository  _supplierRepository;

    public SuppliersController(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    [HttpGet("/suppliers")]
    public async Task<IEnumerable<SupplierDto>> GetAll()
    {
        var suppliers = await _supplierRepository.GetAllSuppliers();
        return suppliers;
    }

    [HttpGet("/suppliers/{id:int}", Name = "GetSupplierById")]
    public async Task<SupplierDto> GetSupplierById(int id)
    {
        var supplier = await _supplierRepository.GetSupplier(id);
        return supplier;
    }

    [HttpPost("/suppliers")]
    public async Task AddSupplier(AddSupplierRequest supplierRequest)
    {
        await _supplierRepository.CreateSupplier(
            supplierRequest.EntrepriseName,
            supplierRequest.Email,
            supplierRequest.Phone);
    }

    [HttpPut("/suppliers")]
    public async Task UpdateSupplier(UpdateSupplierRequest supplierRequest)
    {
        await _supplierRepository.UpdateSupplier(
            supplierRequest.Id,
            supplierRequest.EntrepriseName,
            supplierRequest.Email,
            supplierRequest.Phone);
    }

    [HttpDelete("/suppliers/{id}")]
    public async Task DeleteSupplier(int id)
    {
        await _supplierRepository.DeleteSupplier(id);
    }
}