namespace OnlineResortBooking.Domain.Entities;

public class RoomType
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public decimal PricePerNight { get; private set; }

    public bool HasPrivatePool { get; private set; }

    public string Description { get; private set; }

    public List<string> Amenities { get; private set; }

    public RoomType(
        Guid id,
        string name,
        decimal pricePerNight,
        bool hasPrivatePool,
        string description,
        List<string> amenities)
    {
        Id = id;
        Name = name;
        PricePerNight = pricePerNight;
        HasPrivatePool = hasPrivatePool;
        Description = description;
        Amenities = amenities;
    }
}