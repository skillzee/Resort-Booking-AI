using OnlineResortBooking.Domain.Entities;
using OnlineResortBooking.Domain.ValueObjects;

namespace OnlineResortBooking.Application.Interfaces;

public interface IPricingService
{
    MonetaryAmount CalculateTotal(
        RoomType roomType,
        DateRange stay);
}