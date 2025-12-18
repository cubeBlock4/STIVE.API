using Core.Dto;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using STIVE.Requests;
using STIVE.Requests.Familles;

namespace STIVE.Controllers;

[ApiController]
[Route("[controller]")]
public class FamillesController : ControllerBase
{
    private readonly IFamilleRepository  _familleRepository;

    public FamillesController(IFamilleRepository familleRepository)
    {
        _familleRepository = familleRepository;
    }

    [HttpGet("/familles")]
    public async Task<IEnumerable<FamilleDto>> GetAll()
    {
        var familles = await _familleRepository.GetAllFamilles();
        return familles;
    }

    [HttpGet("/familles/{id:int}", Name = "GetFamilleById")]
    public async Task<FamilleDto> GetFamilleById(int id)
    {
        var famille = await _familleRepository.GetFamille(id);
        return famille;
    }

    [HttpPost("/familles")]
    public async Task AddFamille(AddFamilleRequest familleRequest)
    {
        await _familleRepository.CreateFamille(
            familleRequest.Name);
    }

    [HttpPut("/familles")]
    public async Task UpdateFamille(UpdateFamilleRequest familleRequest)
    {
        await _familleRepository.UpdateFamille(
            familleRequest.Id,
            familleRequest.Name);
    }

    [HttpDelete("/familles/{id}")]
    public async Task DeleteFamille(int id)
    {
        await _familleRepository.DeleteFamille(id);
    }
}