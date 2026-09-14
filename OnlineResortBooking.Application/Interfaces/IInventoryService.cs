using OnlineResortBooking.Domain.ValueObjects;

namespace OnlineResortBooking.Application.Interfaces;

public interface IInventoryService
{
    bool IsAvailable(
        Guid roomTypeId,
        DateRange stay);

    void Reserve(
        Guid roomTypeId,
        DateRange stay);

    void Release(
        Guid roomTypeId,
        DateRange stay);
}