namespace OnlineResortBooking.Domain.Entities;

public class Resort
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Location { get; private set; }

    public string Description { get; private set; }

    public List<RoomType> RoomTypes { get; private set; }

    public List<string> Amenities { get; private set; }

    public Resort(
        Guid id,
        string name,
        string location,
        string description,
        List<string> amenities)
    {
        Id = id;
        Name = name;
        Location = location;
        Description = description;
        Amenities = amenities;
        RoomTypes = new List<RoomType>();
    }

    public void AddRoomType(RoomType roomType)
    {
        RoomTypes.Add(roomType);
    }
}