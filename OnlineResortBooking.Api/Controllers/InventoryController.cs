using Microsoft.AspNetCore.Mvc;
using OnlineResortBooking.Application.Interfaces;
using OnlineResortBooking.Domain.ValueObjects;

namespace OnlineResortBooking.Api.Controllers;

[ApiController]
[Route("api/v1/inventory")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("availability")]
    public IActionResult CheckAvailability(
        [FromQuery] Guid roomTypeId,
        [FromQuery] DateOnly checkIn,
        [FromQuery] DateOnly checkOut)
    {
        DateRange stay;

        try
        {
            stay = new DateRange(checkIn, checkOut);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }

        var isAvailable = _inventoryService.IsAvailable(
            roomTypeId,
            stay);

        return Ok(new
        {
            roomTypeId,
            checkIn,
            checkOut,
            nights = stay.Nights,
            isAvailable
        });
    }
}