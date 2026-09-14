using System.Collections.Concurrent;
using OnlineResortBooking.Application.Interfaces;
using OnlineResortBooking.Domain.Entities;

namespace OnlineResortBooking.Infrastructure.Repositories;

public class InMemoryResortRepository : IResortRepository
{
    private readonly ConcurrentDictionary<Guid, Resort> _resorts = new();

    public InMemoryResortRepository()
    {
        SeedData();
    }

    public IReadOnlyCollection<Resort> GetAll()
    {
        return _resorts.Values.ToList();
    }

    public Resort? GetById(Guid id)
    {
        _resorts.TryGetValue(id, out var resort);

        return resort;
    }

    public IEnumerable<Resort> Search(
        string? location,
        string? roomType,
        bool? hasPrivatePool)
    {
        var resorts = _resorts.Values.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(location))
        {
            resorts = resorts.Where(r =>
                r.Location.Contains(
                    location,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(roomType))
        {
            resorts = resorts.Where(r =>
                r.RoomTypes.Any(room =>
                    room.Name.Contains(
                        roomType,
                        StringComparison.OrdinalIgnoreCase)));
        }

        if (hasPrivatePool.HasValue)
        {
            resorts = resorts.Where(r =>
                r.RoomTypes.Any(room =>
                    room.HasPrivatePool == hasPrivatePool.Value));
        }

        return resorts;
    }

    private void SeedData()
    {
        var resort = new Resort(
            Guid.NewGuid(),
            "Azure Lagoon Ocean Resort",
            "Goa",
            "A luxury beachfront resort offering private pool villas and ocean view suites.",
            new List<string>
            {
                "Beachfront",
                "Swimming Pool",
                "Spa",
                "Restaurant",
                "Airport Transfer"
            });

        resort.AddRoomType(
            new RoomType(
                Guid.NewGuid(),
                "Royal Beachfront Pool Villa",
                420m,
                true,
                "Luxury beachfront villa with a private infinity pool and ocean view.",
                new List<string>
                {
                    "Private Pool",
                    "Ocean View",
                    "King Bed",
                    "Beach Access",
                    "Breakfast"
                }));

        resort.AddRoomType(
            new RoomType(
                Guid.NewGuid(),
                "Ocean View Suite",
                280m,
                false,
                "Spacious suite overlooking the ocean.",
                new List<string>
                {
                    "Ocean View",
                    "King Bed",
                    "Breakfast"
                }));

        resort.AddRoomType(
            new RoomType(
                Guid.NewGuid(),
                "Garden Villa",
                220m,
                false,
                "Peaceful villa surrounded by tropical gardens.",
                new List<string>
                {
                    "Garden View",
                    "King Bed",
                    "Breakfast"
                }));

        _resorts.TryAdd(resort.Id, resort);
    }
}