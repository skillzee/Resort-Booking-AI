namespace OnlineResortBooking.Domain.Entities;

public class InventoryItem
{
    public Guid RoomTypeId { get; private set; }
    public int TotalUnits { get; private set; }

    public InventoryItem(
        Guid roomTypeId,
        int totalUnits)
    {
        if (totalUnits <= 0)
        {
            throw new ArgumentException(
                "Total units must be greater than zero.");
        }

        RoomTypeId = roomTypeId;
        TotalUnits = totalUnits;
    }
}