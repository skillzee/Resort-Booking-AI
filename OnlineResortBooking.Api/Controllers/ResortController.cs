using Microsoft.AspNetCore.Mvc;
using OnlineResortBooking.Application.Interfaces;

namespace OnlineResortBooking.Api.Controllers;

[ApiController]
[Route("api/v1/resorts")]
public class ResortController : ControllerBase
{
    private readonly IResortRepository _resortRepository;

    public ResortController(IResortRepository resortRepository)
    {
        _resortRepository = resortRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var resorts = _resortRepository.GetAll();

        return Ok(resorts);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var resort = _resortRepository.GetById(id);

        if (resort is null)
        {
            return NotFound();
        }

        return Ok(resort);
    }

    [HttpGet("search")]
    public IActionResult Search(
        [FromQuery] string? location,
        [FromQuery] string? roomType,
        [FromQuery] bool? hasPrivatePool)
    {
        var resorts = _resortRepository.Search(
            location,
            roomType,
            hasPrivatePool);

        return Ok(resorts);
    }
}