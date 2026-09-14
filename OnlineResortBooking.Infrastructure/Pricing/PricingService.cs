using OnlineResortBooking.Application.Interfaces;
using OnlineResortBooking.Domain.Entities;
using OnlineResortBooking.Domain.ValueObjects;

namespace OnlineResortBooking.Infrastructure.Pricing;

public class PricingService : IPricingService
{
    public MonetaryAmount CalculateTotal(
        RoomType roomType,
        DateRange stay)
    {
        var total = roomType.PricePerNight * stay.Nights;

        return new MonetaryAmount(
            total,
            "USD");
    }
}