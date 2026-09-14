using System.Collections.Concurrent;
using OnlineResortBooking.Application.Interfaces;
using OnlineResortBooking.Domain.ValueObjects;

namespace OnlineResortBooking.Infrastructure.Inventory;

public class InMemoryInventoryService : IInventoryService
{
    private readonly ConcurrentDictionary<Guid, int> _inventory = new();
    private readonly IResortRepository _resortRepository;


    private readonly ConcurrentDictionary<
        Guid,
        List<DateRange>> _reservations = new();

    public InMemoryInventoryService(IResortRepository resortRepository)
    {
        _resortRepository = resortRepository;
     
        SeedInventory();
    }

    public bool IsAvailable(
        Guid roomTypeId,
        DateRange stay)
    {
        if (!_inventory.TryGetValue(roomTypeId, out var totalUnits))
        {
            return false;
        }

        if (!_reservations.TryGetValue(roomTypeId, out var reservations))
        {
            return true;
        }

        var overlappingReservations = reservations
            .Count(existingStay =>
                existingStay.CheckIn < stay.CheckOut &&
                stay.CheckIn < existingStay.CheckOut);

        return overlappingReservations < totalUnits;
    }

    public void Reserve(
        Guid roomTypeId,
        DateRange stay)
    {
        if (!IsAvailable(roomTypeId, stay))
        {
            throw new InvalidOperationException(
                "Room type is not available for the requested dates.");
        }

        var reservations = _reservations.GetOrAdd(
            roomTypeId,
            _ => new List<DateRange>());

        lock (reservations)
        {
            if (!IsAvailable(roomTypeId, stay))
            {
                throw new InvalidOperationException(
                    "Room type is no longer available.");
            }

            reservations.Add(stay);
        }
    }

    public void Release(
        Guid roomTypeId,
        DateRange stay)
    {
        if (!_reservations.TryGetValue(
                roomTypeId,
                out var reservations))
        {
            return;
        }

        lock (reservations)
        {
            reservations.RemoveAll(existingStay =>
                existingStay.CheckIn == stay.CheckIn &&
                existingStay.CheckOut == stay.CheckOut);
        }
    }

    public void AddInventory(
        Guid roomTypeId,
        int totalUnits)
    {
        if (totalUnits <= 0)
        {
            throw new ArgumentException(
                "Total units must be greater than zero.");
        }

        _inventory[roomTypeId] = totalUnits;
    }

    private void SeedInventory()
{
    var resorts = _resortRepository.GetAll();

    foreach (var resort in resorts)
    {
        foreach (var roomType in resort.RoomTypes)
        {
            var totalUnits = roomType.Name switch
            {
                "Royal Beachfront Pool Villa" => 5,
                "Ocean View Suite" => 10,
                "Garden Villa" => 8,
                _ => 1
            };

            AddInventory(
                roomType.Id,
                totalUnits);
        }
    }
}
}